using Config;
using Unity.Mathematics;
using System.Collections.Generic;
using UnityEngine;
using Models;

namespace Generators{
    public class SeaGenerator: FluidGenerator
    {
        private int layers;

        public SeaGenerator(GameObject prefab) : base(prefab)
        {
        }

        public override List<Particle> start(List<Particle> particles)
        {
            int I = Config.Simulation.numberOfParticlesX;
            int J = Config.Simulation.numberOfParticlesY; 
            int K = Config.Simulation.numberOfParticlesZ;
            for(int i = 0; i < I; i++)
                for(int j = 0; j < J; j++)
                    for(int k = 0; k < K; k++)
                        particles.Add(this.create(this.getPositionVector(i, j, k)));
            return particles;
        }

        public float3 getPositionVector(int i, int j, int k)
        {
            float s = SPH.H + 1;
            return new float3(
                -Simulation.RoomSizeX/2 + Simulation.wallsThickness + (i * s),
                -Simulation.RoomSizeY/2 + Simulation.wallsThickness + ((j + 1) * s),
                -Simulation.RoomSizeZ/2 + Simulation.wallsThickness + (k * s)
            ) + this.getNoiseVector();
        }
        public override List<Particle> update(List<Particle> particles)
        {
            return null;
        }
    }
}