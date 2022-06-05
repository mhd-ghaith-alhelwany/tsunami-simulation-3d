using Models;
using System.Collections.Generic;
using UnityEngine;

namespace Generators.ECS.Fluid{
    public abstract class Generator
    {
        public abstract void start();
        public abstract void update();
    }
}