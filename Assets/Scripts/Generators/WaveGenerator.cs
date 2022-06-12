using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Config;
using System.Collections.Generic;
using Models;

namespace Generators{
    public class WaveGenerator: FluidGenerator
    {

        public WaveGenerator(GameObject prefab) : base(prefab)
        {
        }

        public override List<Particle> start(List<Particle> particles)
        {
            return particles;
        }

        public float3 getPositionVector(int i, int j, int k)
        {
            return new float3(
                (i * SPH.H),
                (j * SPH.H),
                (k * SPH.H)
            ) + this.getNoiseVector();
        }


        public override List<Particle> update(List<Particle> particles)
        {
            return particles;
        }
    }
}