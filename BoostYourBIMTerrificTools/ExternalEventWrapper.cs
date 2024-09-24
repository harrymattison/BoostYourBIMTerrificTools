﻿using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostYourBIMTerrificTools
{
    abstract public class RevitEventWrapper<T>
      : IExternalEventHandler
    {
        private object @lock;
        private T savedArgs;
        private ExternalEvent revitEvent;

        public RevitEventWrapper()
        {
            revitEvent = ExternalEvent.Create(this);
            @lock = new object();
        }

        public void Execute(UIApplication app)
        {
            T args;

            lock (@lock)
            {
                args = savedArgs;
                savedArgs = default(T);
            }
            Execute(app, args);
        }

        public string GetName()
        {
            return GetType().Name;
        }

        public void Raise(T args)
        {
            lock (@lock)
            {
                savedArgs = args;
            }
            revitEvent.Raise();
        }

        abstract public void Execute(
          UIApplication app, T args);
    }
}
