using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Components{
    public struct SphParticle : IComponentData{
        public int particleId;
        public float density, pressure;
        public float3 position, force, velocity;
    }
}
