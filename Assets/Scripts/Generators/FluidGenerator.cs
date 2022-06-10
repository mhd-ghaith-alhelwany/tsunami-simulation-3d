using Unity.Mathematics;
using Models;
using System.Collections.Generic;
using UnityEngine;

namespace Generators{
    public abstract class FluidGenerator : Generator
    {
        public FluidGenerator(GameObject prefab) : base(prefab){}

        public abstract List<Particle> start(List<Particle> particles);
        public abstract List<Particle> update(List<Particle> particles);

        public Particle create(float3 position)
        {
            return new Particle(base.createGameObject(position + this.getNoiseVector()));
        }
    }
}