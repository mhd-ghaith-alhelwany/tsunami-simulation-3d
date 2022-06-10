using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Config;
using System.Collections.Generic;
using Models;

namespace Generators{
    public class BucketGenerator: FluidGenerator
    {
        private Vector3 gridSize;
        private Vector3 startingPoint;

        public BucketGenerator(GameObject prefab, Vector3 gridSize, Vector3 startingPoint) : base(prefab)
        {
            this.gridSize = gridSize;
            this.startingPoint = startingPoint;
        }

        public override List<Particle> start(List<Particle> particles)
        {
            for(int j = 0; j < this.gridSize[1]; j++)
                for(int i = 0; i < this.gridSize[0]; i++)
                    for(int k = 0; k < this.gridSize[2]; k++)
                        particles.Add(this.create(this.getPositionVector(i, j, k)));
            return particles;
        }

        public float3 getPositionVector(int i, int j, int k)
        {
            return new float3(
                (i * Simulation.particleSize) + startingPoint[0],
                (j * Simulation.particleSize) + startingPoint[1],
                (k * Simulation.particleSize) + startingPoint[2]
            ) + this.getNoiseVector();
        }


        public override List<Particle> update(List<Particle> particles)
        {
            return particles;
        }
    }
}