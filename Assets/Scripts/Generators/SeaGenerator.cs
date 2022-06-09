using UnityEngine;
using Main;
using Unity.Mathematics;

namespace Generators{
    public class SeaGenerator: Generator
    {
        private ECSGame game;
        private Vector3 boxSize;
        private System.Random random;
        private float depth, width, height;
        private int layers;

        public SeaGenerator(ECSGame game, float width, float height, float depth, int layers) : base()
        {
            this.game = game;
            this.height = height;
            this.width = width;
            this.depth = depth;
            this.layers = layers;
            this.random = new System.Random();
        }

        public override void start()
        {
            float particleSize = this.game.getParticleSize();
            int I = (int)(this.width/particleSize) - 1;
            int J = (int)(this.height/particleSize) - 1; 
            int K = layers; 
            for(int i = 1; i < I; i++){
                for(int j = 1; j < J; j++){
                    for(int k = 0; k < K; k++){
                        float3 position = new float3(i*particleSize - (this.width/2) + this.getRand(), -(this.depth/2) + (k+1)*particleSize + 10, j*particleSize - (this.height/2) + this.getRand());
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