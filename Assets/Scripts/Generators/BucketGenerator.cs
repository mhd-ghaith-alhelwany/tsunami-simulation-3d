using UnityEngine;
using Main;
using Models;
using System.Collections.Generic;

namespace Generators{
    public class BucketGenerator: Generator
    {
        private Game game;
        private Vector3 gridSize;
        private Vector3 startingPoint;

        public BucketGenerator(Game game, Vector3 gridSize, Vector3 startingPoint) : base()
        {
            this.game = game;
            this.gridSize = gridSize;
            this.startingPoint = startingPoint;
            
        }

        override
        public List<Particle> start(List<Particle> existingParticles)
        {
            Vector3 boxSize = this.game.getBoxSize();
            float particleSize = this.game.getParticleSize();
            for(int i = 0; i < this.gridSize[0]; i++){
                float randi = 0;
                randi += getRand();
                for(int j = 0; j < this.gridSize[1]; j++){
                    for(int k = 0; k < this.gridSize[2]; k++){
                        float randk = 0;
                        randk += getRand();
                        existingParticles.Add(
                            new Particle(
                                new Vector3(
                                    i * particleSize + startingPoint[0] + randi,
                                    j * particleSize + startingPoint[1],
                                    k * particleSize + startingPoint[2] + randk
                                ), 
                                game.getFluidPrefab()
                            )
                        );
                    }
                }
            }
            return existingParticles;
        }

        private float getRand()
        {
            return new System.Random().Next((int)this.game.getParticleSize() / 4);
        }

        override
        public List<Particle> update(List<Particle> existingParticles)
        {
            return existingParticles;
        }
    }
}