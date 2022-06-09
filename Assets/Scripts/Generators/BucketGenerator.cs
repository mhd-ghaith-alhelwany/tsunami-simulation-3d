using UnityEngine;
using Main;
using Unity.Mathematics;

namespace Generators{
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

            for(int j = 0; j < this.gridSize[1]; j++){
                float randi = 0;
                float randk = 0;
                for(int i = 0; i < this.gridSize[0]; i++){
                    for(int k = 0; k < this.gridSize[2]; k++){
                        randi += getRand();
                        randk += getRand();
                        float3 position = new float3((i * particleSize) + startingPoint[0] + randi,(j * particleSize) + startingPoint[1] + getRand(),(k * particleSize) + startingPoint[2] + randk);
                        this.game.createParticle(position, new float3(0, 0, 0));
                    }
                }
            }
        }


        private float getRand()
        {
            return random.Next((int)this.game.getParticleSize()) / 8;
        }

        override
        public void update()
        {
        }
    }
}