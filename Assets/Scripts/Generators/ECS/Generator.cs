using Models;
using System.Collections.Generic;
using UnityEngine;

namespace Generators.ECS{
    public abstract class Generator
    {
        public abstract void start();
        public abstract void update();
    }
}