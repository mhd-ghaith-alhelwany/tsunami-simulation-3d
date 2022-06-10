using Config;
using Unity.Mathematics;
using System.Collections.Generic;
using UnityEngine;
using Models;

namespace Generators{
    public class SeaGenerator: FluidGenerator
    {
        private int layers;

        public SeaGenerator(GameObject prefab, int layers) : base(prefab)
        {
            this.layers = layers;
        }

        public override List<Particle> start(List<Particle> particles)
        {
            int I = (int)(Simulation.floorX/Simulation.particleSize) - 1;
            int J = (int)(Simulation.floorY/Simulation.particleSize) - 1; 
            int K = layers;
            for(int i = 1; i < I; i++)
                for(int j = 1; j < J; j++)
                    for(int k = 0; k < K; k++)
                        particles.Add(this.create(this.getPositionVector(i, j, k)));
            return particles;
        }

        public float3 getPositionVector(int i, int j, int k)
        {
            return new float3(
                i * Simulation.particleSize - (Simulation.floorX / 2), 
                (k + 1) * Simulation.particleSize + (Simulation.wallsThickness / 2) - (Simulation.wallSize / 2), 
                j * Simulation.particleSize - (Simulation.floorY / 2)
            );
        }
        public override List<Particle> update(List<Particle> particles)
        {
            return particles;
        }
    }
}