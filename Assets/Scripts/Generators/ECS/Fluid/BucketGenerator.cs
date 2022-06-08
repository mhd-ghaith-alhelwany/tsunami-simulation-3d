using UnityEngine;
using Main;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Components;

namespace Generators.ECS.Fluid{
    public class BucketGenerator: Generator
    {
        private ECSGame game;
        private Vector3 gridSize;
        private Vector3 startingPoint;
        private System.Random random;

        public BucketGenerator(ECSGame game, Vector3 gridSize, Vector3 startingPoint) : base()
        {
            this.game = game;
            this.gridSize = gridSize;
            this.startingPoint = startingPoint;
            this.random = new System.Random();
        }

        public override void start()
        {
            float particleSize = this.game.getParticleSize();

            for(int i = 0; i < this.gridSize[0]; i++){
                for(int j = 0; j < this.gridSize[1]; j++){
                    for(int k = 0; k < this.gridSize[2]; k++){
                        float3 position = new float3((i * particleSize) + startingPoint[0] + getRand(),(j * particleSize) + startingPoint[1] + getRand(),(k * particleSize) + startingPoint[2] + getRand());
                        this.game.createParticle(position, new float3(0, 0, 0));
                    }
                }
            }
        }


        private float getRand()
        {
            return random.Next(-(int)this.game.getParticleSize(), (int)this.game.getParticleSize()) / 16;
        }

        override
        public void update()
        {
        }
    }
}