using Models;
using System.Collections.Generic;
using UnityEngine;

namespace Generators.ECS.Objects{
    public abstract class ObjectGenerator
    {
        protected GameObject objectPrefab;
        public ObjectGenerator(GameObject objectPrefab)
        {
            this.objectPrefab = objectPrefab;
        }
        public abstract void generate();
    }
}