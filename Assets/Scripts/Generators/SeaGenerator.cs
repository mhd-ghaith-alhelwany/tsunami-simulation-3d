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
            int I = (int)(Simulation.RoomSizeX/SPH.H) - 1;
            int J = (int)(Simulation.RoomSizeY/SPH.H) - 1; 
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
                i * SPH.H - (Simulation.RoomSizeX / 2), 
                (k + 5) * SPH.H + (Simulation.wallsThickness / 2) - (Simulation.RoomSizeZ / 2),
                j * SPH.H - (Simulation.RoomSizeY / 2)
            );
        }
        public override List<Particle> update(List<Particle> particles)
        {
            return particles;
        }
    }
}