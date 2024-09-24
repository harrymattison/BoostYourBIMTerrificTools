
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace WallOpeningArea
{
  class WallAreaFunctions
  {
    // Store the wall opening for each wall
    // Array of doubles
    // [0] PARAMETER_SMALL_OPEN_NAME
    // [1] PARAMETER_TOTAL_OPEN_NAME
    private static Dictionary<int, double[]> wallsOpeningArea;

    public static void CalculateWallOpeningAreas(UIDocument uidoc,
        double minOpeningValue)
    {
      Document doc = uidoc.Document;

      // Start the first disposable transaction hosted family 
      // instances will be removed to calculate their area, the the 
      // remaining openings will be calculated from profile openings 
      // this transaction will be aborted
      Transaction transSearch = new Transaction(doc);
      transSearch.Start("Wall Opening Search");

      // Create/Reset dictionary of values
      wallsOpeningArea = new Dictionary<int, double[]>();

      // First calculate opening element openings
      OpeningElementOpenings(doc, minOpeningValue);

      FilteredElementCollector wallCollector =
          new FilteredElementCollector(doc);
      wallCollector.OfClass(typeof(Wall)).Cast<Wall>().Where(q => q.WallType.Kind != WallKind.Stacked);

      if (new FilteredElementCollector(doc).OfClass(typeof(Wall)).Cast<Wall>().Where(q => q.WallType.Kind == WallKind.Stacked).Count() > 0)
      {
          TaskDialog.Show("Alert", "Stacked walls will not be evaluated.");
      }

      // Revit 2014 API code
      IList<Element> walls =
          wallCollector.ToList<Element>();
      
    string progressStatus = "Processing wall {0} of " + walls.Count() + "...";
    string progressCaption = "Progress";
    using (BoostYourBIMUtils.formProgress progressForm = new BoostYourBIMUtils.formProgress(progressCaption, progressStatus, walls.Count()))
    {
        foreach (Wall wall in walls)
        {
            progressForm.Increment();

            if (progressForm.getAbortFlag())
                break;

            // infill wall will be deleted
            try
                    {
                        ElementId id = wall.Id;
                    }
                    catch
                    {
                        continue;
                    }

            if (wall.WallType.Kind == WallKind.Stacked)
                continue;

            // Sum of openings smaller than max
            double wallOpeningArea = 0.0;
            // Sum of openings (total)
            double wallTotalOpeningArea = 0.0;

            // Calculate the total of openings on this wall
            // first openings by hosted elements
            HostedFamilyInstanceOpenings(wall, minOpeningValue);

            // then by profile editing
            // >> At this point, all calculations obtained by 
            //    deletings elements must be already performed, 
            //    or there is a risk of counting twice.
            ProfileOpenings(wall, minOpeningValue, uidoc);

            // Store the value for later...
            // If already there, just increase the are
            if (wallsOpeningArea.ContainsKey(wall.Id.IntegerValue))
            {
                wallsOpeningArea[wall.Id.IntegerValue][0] +=
                  wallOpeningArea;
                wallsOpeningArea[wall.Id.IntegerValue][1] +=
                  wallTotalOpeningArea;
            }
            else //not there yet, add a new key/value
                wallsOpeningArea.Add(wall.Id.IntegerValue,
                    new double[] { wallOpeningArea, wallTotalOpeningArea });
        }
    }

      // Abort transactions to un-delete all hosted family 
      // instances and openings
      transSearch.RollBack();

      // Apply all the information at the wall shared parameter
      SetSharedParameterValues(doc);

      // Suppressed for now
      // VerifyValues(doc);
    }

    //private static void VerifyValues(Document doc)
    //{
    //  StringBuilder errors = new StringBuilder();

    //  FilteredElementCollector wallCollector =
    //      new FilteredElementCollector(doc);
    //  wallCollector.OfClass(typeof(Wall));
    //  foreach (Wall wall in wallCollector)
    //  {
    //    double netArea = wall.get_Parameter(
    //        BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
    //    double openingArea = wall.
    //        get_Parameter(SharedParameterFunctions.
    //        PARAMETER_SMALL_OPEN_NAME).AsDouble();

    //    // If the opening area is bigger than the net area,
    //    if (openingArea > netArea)
    //    {
    //      errors.AppendFormat("\nWall {0}: {1:0}% opening",
    //          wall.Id.IntegerValue,
    //          (openingArea / (netArea + openingArea)) * 100); ;
    //    }
    //  }
    //  // Then the wall have 50%+ opening throw a warning
    //  if (errors.Length > 5)
    //    TaskDialog.Show("Walls with 50%+ opening",
    //        errors.ToString());
    //}

    private static void SetSharedParameterValues(Document doc)
    {
      // Start the second transaction to set the shared parameter
      Transaction transParam = new Transaction(doc);
      transParam.Start("Wall Opening Param");

      // open or create the shared parameter
      if (!SharedParameterFunctions.
        OpenOrCreateWallSharedParameter(doc))
      {
        transParam.RollBack();
        return; //something went wrong...
      }

      foreach (KeyValuePair<int, double[]> wallDicEntry
          in wallsOpeningArea)
      {
        Wall wall = doc.GetElement(
            new ElementId(wallDicEntry.Key)) as Wall;
        double openingArea = wallDicEntry.Value[0];
        double totalOpeningArea = wallDicEntry.Value[1];

        // check if the wall is valid
        if (wall == null)
        {
          TaskDialog.Show("Error accessing wall",
            string.Format("The plug-in could not locate wall {0}. " +
            "Operation cancelled.", wallDicEntry.Key));
          transParam.RollBack();
          return;
        }

        // Store the opening area as Shared Parameter
        Parameter openingAreaParam = SharedParameterFunctions.getParam(
            wall,
            SharedParameterFunctions.PARAMETER_SMALL_OPEN_NAME);
        if (openingAreaParam == null)
        {
          TaskDialog.Show("Error accessing wall parameter",
            string.Format("The plug-in could not locate parameter " +
            "{0} of wall {1}. Operation cancelled.",
            SharedParameterFunctions.PARAMETER_SMALL_OPEN_NAME,
            wallDicEntry.Key));
          transParam.RollBack();
          return;
        }
        if (!openingAreaParam.IsReadOnly)
            openingAreaParam.Set(openingArea);

        // Store the opening area as Shared Parameter
        Parameter totalOpeningAreaParam = SharedParameterFunctions.getParam(
            wall,
            SharedParameterFunctions.PARAMETER_TOTAL_OPEN_NAME);
        if (totalOpeningAreaParam == null)
        {
          TaskDialog.Show("Error accessing wall parameter",
            string.Format("The plug-in could not locate parameter " +
            "{0} of wall {1}. Operation cancelled.",
            SharedParameterFunctions.PARAMETER_TOTAL_OPEN_NAME,
            wallDicEntry.Key));
          transParam.RollBack();
          return;
        }
        if (!totalOpeningAreaParam.IsReadOnly)
            totalOpeningAreaParam.Set(totalOpeningArea + openingArea);
      }

      // Commit transaction to store the shared parameter
      transParam.Commit();
    }

    private static void OpeningElementOpenings(
        Document doc, double minOpeningValue)
    {
      FilteredElementCollector openingCollector =
          new FilteredElementCollector(doc);
      openingCollector.OfClass(typeof(Opening));

      // Get the IList for the collector - we cannot delete
      // elements using a collector iterator
      IList<Element> openings =
          openingCollector.ToList<Element>();

      foreach (Opening open in openings)
      {
        // Open the host wall
        Wall wall = open.Host as Wall;
        if (wall == null) continue; //safety

        // Get the original area to compare
        double previousArea = wall.get_Parameter(
            BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();

        // Delete the wall opening
        // Revit 2013 API code
        // doc.Delete(open);
        // Revit 2014 API code
        try
        {
          doc.Delete(open.Id);
        }
        catch{}

        doc.Regenerate();

        // Get the new area to compare
        double newArea = wall.get_Parameter(
            BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();

        // So the instance opening equals:
        double instanceAreaOnTheWall =
            (newArea - previousArea);

        if (instanceAreaOnTheWall <= minOpeningValue)
          AddWallArea(wall.Id, instanceAreaOnTheWall, 0.0);
        else
          AddWallArea(wall.Id, 0.0, instanceAreaOnTheWall);
      }
    }

    private static void HostedFamilyInstanceOpenings(Wall wall,
        double minOpeningValue)
    {
      double wallOpeningArea = 0.0;
      double wallTotalOpeningArea = 0.0;

      // Filter all Family Instances where the HOST_ID_PARAM 
      // equals the wall ID
      // 
      // More information at
      // http://thebuildingcoder.typepad.com/
      //                 blog/2010/06/parameter-filter.html#4
      BuiltInParameter testParam =
          BuiltInParameter.HOST_ID_PARAM;
      ParameterValueProvider pvp =
          new ParameterValueProvider(
              new ElementId((int)testParam));

      FilterNumericRuleEvaluator fnrv = new FilterNumericEquals();
      ElementId ruleValId = wall.Id;

      FilterRule paramFr = new FilterElementIdRule
        (pvp, fnrv, ruleValId);
      ElementParameterFilter epf = 
        new ElementParameterFilter(paramFr);
      FilteredElementCollector collector =
          new FilteredElementCollector(wall.Document);

      collector.OfClass(typeof(FamilyInstance)).WherePasses(epf);
      IList<Element> hostedFamilyInstances = collector.ToElements();

      // Now iterate through the collected family instances
      Document doc = wall.Document;
      double previousArea = wall.get_Parameter(
          BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();

      foreach (FamilyInstance instance in hostedFamilyInstances)
      {
        // Delete the hosted family instace and regenerate
        // Revit 2013 API code
        //doc.Delete(instance);
        // Revit 2014 API code
        try
        {
          doc.Delete(instance.Id);
        }
        catch{}
        doc.Regenerate();

        // Get the new area to compare
        double newArea = wall.get_Parameter(
            BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();

        // So the instance opening equals:
        double instanceAreaOnTheWall = 
          Math.Abs(newArea - previousArea);

        // The element area (on wall) is smaller than 
        // the minOpeningValue?
        if (instanceAreaOnTheWall <= minOpeningValue)
          wallOpeningArea += instanceAreaOnTheWall;
        else
          wallTotalOpeningArea += instanceAreaOnTheWall;

        //if (System.Diagnostics.Debugger.IsAttached)
        //  TaskDialog.Show(
        //      "Wall opening (by inst) found (in sq feet)",
        //      string.Format("Area: {0}", instanceAreaOnTheWall));

        previousArea = newArea;
      }

      AddWallArea(wall.Id, wallOpeningArea, wallTotalOpeningArea);
    }

    private static void ProfileOpenings(Wall wall,
        double minOpeningValue, UIDocument uiDoc)
    {
      double wallOpeningArea = 0.0;
      double wallTotalOpeningArea = 0.0;

      // Get the geometry
      GeometryElement geo = wall.get_Geometry(new Options());
      foreach (GeometryObject geoObj in geo)
      {
        // Get the solid (expected for walls)
        Solid solid = geoObj as Solid;
        if (solid != null)
        {
          // Get the faces
          foreach (Face solidFace in solid.Faces)
          {
            // Get the front face
            XYZ faceNormal = solidFace.ComputeNormal(new UV());
            if (faceNormal.IsAlmostEqualTo(wall.Orientation))
            {
              // Get the loop (external ou internal)
              foreach (EdgeArray ea in solidFace.EdgeLoops)
              {
                List<XYZ> loop;
                XYZ loopNormal;
                double loopArea;
                ComputerGeometryLoops(ea, out loop,
                    out loopNormal, out loopArea);

                // Skip the outermost loop
                // 
                // The outemost loop is equals or greater
                // than the face
                if (IsLoopValid(/*minOpeningValue,*/ solidFace,
                  faceNormal, loopNormal, loopArea))
                {
                  if (IsLoopOpen(wall, loop, loopNormal))
                  {
                    if (loopArea < minOpeningValue)
                      wallOpeningArea += loopArea;
                    else
                      wallTotalOpeningArea += loopArea;
                  }
                }
              }
            }
          }
        }
      }
      AddWallArea(wall.Id, wallOpeningArea, wallTotalOpeningArea);
    }


    private static void AddWallArea(ElementId wallId,
      double openingArea, double totalOpeningArea)
    {
      // Already there, just increase the are
      if (wallsOpeningArea.ContainsKey(
          wallId.IntegerValue))
      {
        if (openingArea > 0) //only if is needed...
          wallsOpeningArea[wallId.IntegerValue][0]
            += openingArea;
        if (totalOpeningArea > 0) //only if is needed...
          wallsOpeningArea[wallId.IntegerValue][1]
            += totalOpeningArea;
      }
      else // Not there yet, add a new key/value
        wallsOpeningArea.Add(wallId.IntegerValue,
            new double[] { openingArea, totalOpeningArea });
    }

    #region Loop related functions

    private static bool IsLoopOpen(Wall wall, List<XYZ> loop,
        XYZ loopNormal)
    {
      XYZ rayDirection = loopNormal.Negate();
      XYZ rayOrigin = GetCenterOfPolygon(loop).
          Add(rayDirection.Multiply(wall.Width).Negate());

      // Let check if this loop is really an opening
      // Revit 2013 API Code
      //IList<ReferenceWithContext> elementsOnTheOpening = wall.
      // Document.FindReferencesWithContextByDirection(rayOrigin,
      // rayDirection, (View3D)wall.Document.ActiveView);
      
      // Revit 2014 API code
      ReferenceIntersector refIntersector = 
        new ReferenceIntersector((View3D)wall.Document.ActiveView);
      IList<ReferenceWithContext> elementsOnTheOpening = 
        refIntersector.Find(rayOrigin, rayDirection);

      ReferenceWithContext closestReference =
        FindClosestReference(wall.Document, elementsOnTheOpening);

        Element closestElement = null;
        if (closestReference != null)
            closestElement = wall.Document.GetElement(closestReference.GetReference());

      // If there is nothing on the direction
      // or the closest element if more far than the wall width
      // then the loop is a 'full' profile opening
      bool loopIsOpen =
          (closestReference == null ||
          closestElement.Id.IntegerValue !=
          wall.Id.IntegerValue);

      return loopIsOpen;
    }

    private static bool IsLoopValid(//double minOpeningValue,
        Face f, XYZ faceNormal,
        XYZ loopNormal, double loopArea)
    {
      return loopArea < f.Area &&
          //loopArea < (minOpeningValue) &&
          (loopNormal.IsAlmostEqualTo(faceNormal)
          || loopNormal.Negate().IsAlmostEqualTo(faceNormal));
    }

    private static void ComputerGeometryLoops(EdgeArray ea,
        out List<XYZ> loop, out XYZ loopNormal,
        out double loopArea)
    {
      // Create a list of points of the loop
      loop = new List<XYZ>();
      foreach (Edge e in ea)
      {
        IList<XYZ> edgePoints = e.Tessellate();

        // Avoid duplicated points (generic approach)
        foreach (XYZ edgePoint in edgePoints)
          if (!ListContains(loop, edgePoint))
            loop.Add(edgePoint);
      }

      // Calculate the area and length of the loop
      //
      // More information at
      // http://thebuildingcoder.typepad.com/
      //                         blog/2008/12/3d-polygon-areas.html

      double loopLength;

      GetPolygonPlane(loop, out loopNormal, 
        out loopLength, out loopArea);
    }

    #endregion

    /// <summary>
    /// Check if the LIST contains the POINT
    /// </summary>
    private static bool ListContains(List<XYZ> list, XYZ point)
    {
      foreach (XYZ pt in list)
        if (pt.IsAlmostEqualTo(point))
          return true;
      return false;
    }

    #region GetCenterOfPolygon

    public static XYZ GetCenterOfPolygon(List<XYZ> polygon)
    {
      double minX, maxX, minY, maxY, minZ, maxZ;
      minX = minY = minZ = double.MaxValue;
      maxX = maxY = maxZ = double.MinValue;
      foreach (XYZ vertex in polygon)
      {
        if (vertex.X < minX) minX = vertex.X;
        if (vertex.X > maxX) maxX = vertex.X;

        if (vertex.Y < minY) minY = vertex.Y;
        if (vertex.Y > maxY) maxY = vertex.Y;

        if (vertex.Z < minZ) minZ = vertex.Z;
        if (vertex.Z > maxZ) maxZ = vertex.Z;
      }
      return new XYZ(
        minX + (maxX - minX) / 2,
        minY + (maxY - minY) / 2, 
        minZ + (maxZ - minZ) / 2);
    }

    #endregion

    #region MakeLine

    /// <summary>
    /// Draw a small line for debug porpouse
    /// </summary>
    //public static void MakeLine(Autodesk.Revit.UI.UIApplication app,
    //  Autodesk.Revit.DB.XYZ startpt, Autodesk.Revit.DB.XYZ endpt, 
    //  Autodesk.Revit.DB.XYZ direction, string style)
    //{
    //  try
    //  {
    //    Line line = app.Application.Create.NewLineBound(startpt,
    //      endpt);
    //    XYZ rotatedDirection = XYZ.BasisX;
    //    if (!direction.IsAlmostEqualTo(XYZ.BasisZ) && 
    //      !direction.IsAlmostEqualTo(-XYZ.BasisZ))
    //    {
    //      rotatedDirection = 
    //        direction.Normalize().CrossProduct(XYZ.BasisZ);
    //    }

    //    Plane geometryPlane = app.Application.Create.NewPlane
    //      (direction, rotatedDirection, startpt);
    //    SketchPlane skplane = app.ActiveUIDocument.Document.
    //      Create.NewSketchPlane(geometryPlane);
    //    ModelCurve mcurve = app.ActiveUIDocument.Document.Create.
    //      NewModelCurve(line, skplane);
    //    ICollection<ElementId> lsArr = mcurve.GetLineStyleIds();
    //    foreach (Autodesk.Revit.DB.ElementId eId in lsArr)
    //    {
    //      Element e = 
    //      if (e.Name == style)
    //      {
    //        mcurve.LineStyle = e;
    //        break;
    //      }
    //    }
    //  }
    //  catch (System.Exception e)
    //  {
    //  }
    //}

    #endregion

    #region FindClosestReference

    private static double epsilon = 0.00000001;
    public static Autodesk.Revit.DB.ReferenceWithContext FindClosestReference
      (Document doc,  IList<ReferenceWithContext> references)
    {
      double face_prox = Double.PositiveInfinity;
      double edge_prox = Double.PositiveInfinity;

      Face face;
      ReferenceWithContext closestReference = null;
      foreach (Autodesk.Revit.DB.ReferenceWithContext r in references)
      {
        Element e = doc.GetElement(r.GetReference());


        face = null;
        face = e.GetGeometryObjectFromReference(r.GetReference()) as Face;
        Edge edge = null;
        edge = e.GetGeometryObjectFromReference(r.GetReference()) as Edge;
        if (face != null)
        {
          if ((r.Proximity < face_prox) && 
            (r.Proximity > epsilon))
          {
            closestReference = r;
            face_prox = Math.Abs(r.Proximity);
          }
        }
        else if (edge != null)
        {
          if ((r.Proximity < edge_prox) && 
            (r.Proximity > epsilon))
          {
            edge_prox = Math.Abs(r.Proximity);
          }
        }
      }
      if (edge_prox <= face_prox)
      {
        // Stop bouncing if there is an edge at least as close as 
        // the neareast face - there is no single angle of reflection
        // for a ray striking a line
        closestReference = null;
      }
      return closestReference;
    }

    #endregion

    #region GetPolygonPlane
    private static bool GetPolygonPlane(List<XYZ> polygon, 
      out XYZ normal, out double dist, out double area)
    {
      normal = XYZ.Zero;
      dist = area = 0.0;
      int n = (null == polygon) ? 0 : polygon.Count;
      bool rc = (2 < n);
      if (3 == n)
      {
        XYZ a = polygon[0];
        XYZ b = polygon[1];
        XYZ c = polygon[2];
        XYZ v = b - a;
        normal = v.CrossProduct(c - a);
        dist = normal.DotProduct(a);
      }
      else if (4 == n)
      {
        XYZ a = polygon[0];
        XYZ b = polygon[1];
        XYZ c = polygon[2];
        XYZ d = polygon[3];

        normal = new XYZ(
        (c.Y - a.Y) * (d.Z - b.Z) + (c.Z - a.Z) * (b.Y - d.Y),
        (c.Z - a.Z) * (d.X - b.X) + (c.X - a.X) * (b.Z - d.Z),
        (c.X - a.X) * (d.Y - b.Y) + (c.Y - a.Y) * (b.X - d.X));

        dist = 0.25 *
          (normal.X * (a.X + b.X + c.X + d.X)
          + normal.Y * (a.Y + b.Y + c.Y + d.Y)
          + normal.Z * (a.Z + b.Z + c.Z + d.Z));
      }
      else if (4 < n)
      {
        XYZ a;
        XYZ b = polygon[n - 2];
        XYZ c = polygon[n - 1];
        XYZ s = XYZ.Zero;

        for (int i = 0; i < n; ++i)
        {
          a = b;
          b = c;
          c = polygon[i];

          normal = new XYZ(
          b.Y * (c.Z - a.Z),
          b.Z * (c.X - a.X),
          b.X * (c.Y - a.Y));

          s += c;
        }
        dist = s.DotProduct(normal) / n;
      }
      if (rc)
      {
        double length = normal.GetLength();
        rc = !normal.IsZeroLength();

        if (rc)
        {
          normal /= length;
          dist /= length;
          area = 0.5 * length;
        }
      }
      return rc;
    }
    #endregion
  }
}
