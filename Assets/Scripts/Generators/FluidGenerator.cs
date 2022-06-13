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
            return new Particle(base.createGameObject(position));
        }

        public Particle create(float3 position, float3 velocity)
        {
            Particle p = new Particle(base.createGameObject(position));
            p.setVelocity(velocity);
            return p;
        }
    }
}