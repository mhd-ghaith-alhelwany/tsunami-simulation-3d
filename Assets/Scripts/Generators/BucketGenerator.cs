using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Config;

namespace Generators{
    public class BucketGenerator: FluidGenerator
    {
        private Vector3 gridSize;
        private Vector3 startingPoint;

        public BucketGenerator(Entity prefab, EntityManager entityManager, Vector3 gridSize, Vector3 startingPoint) : base(prefab, entityManager)
        {
            this.gridSize = gridSize;
            this.startingPoint = startingPoint;
        }

        public override void start()
        {
            for(int j = 0; j < this.gridSize[1]; j++)
                for(int i = 0; i < this.gridSize[0]; i++)
                    for(int k = 0; k < this.gridSize[2]; k++)
                        this.create(this.getPositionVector(i, j, k), this.getEmptyVector());
        }

        public float3 getPositionVector(int i, int j, int k)
        {
            return new float3(
                (i * Simulation.particleSize) + startingPoint[0],
                (j * Simulation.particleSize) + startingPoint[1],
                (k * Simulation.particleSize) + startingPoint[2]
            );
        }

        override
        public void update()
        {
        }
    }
}