using UnityEngine;
using Generators.ECS.Fluid;
using Generators.ECS.Objects;
using Models;
using  Controllers;

using System.Collections.Generic;

namespace Main {

    public class ECSGame
    {
        private float particleSize;
        private float wallSize;
        private float floorSize;
        private GameObject fluidPrefab;
        private GameObject boxPrefab;

        private List<Generator> generators;
        private Controller controller;

        private List<Particle> particles;
        private GameObject boxObject;
        private Transform transform;
        
        public ECSGame(GameObject fluidPrefab, GameObject boxPrefab, Transform transform)
        {
            this.fluidPrefab = fluidPrefab;
            this.boxPrefab = boxPrefab;
            this.transform = transform;

            this.particleSize = 16f;
            this.wallSize = 250;
            this.floorSize = 500;

            this.generators = new List<Generator>();

            this.generators.Add(new BucketGenerator(this, new Vector3(27, 27, 28), new Vector3(-200, 0, -200)));    
        }

        public GameObject getFluidPrefab()
        {
            return this.fluidPrefab;
        }

        public Transform getTransform()
        {
            return this.transform;
        }

        public float getParticleSize()
        {
            return this.particleSize;
        }

        private void startGenerators()
        {
            foreach(Generator generator in this.generators)
                generator.start();
        }

        private void updateGenerators()
        {
            foreach(Generator generator in this.generators)
                generator.update();
        }

        private void initEnvironment()
        {
            this.fluidPrefab.transform.localScale = new Vector3(this.particleSize, this.particleSize, this.particleSize);
            this.initRoom();
        }

        private void initRoom()
        {
            new WallGenerator(this.boxPrefab, this, new Vector3(20, this.wallSize, this.floorSize), new Vector3(+this.floorSize/2, 0, 0)).generate();
            new WallGenerator(this.boxPrefab, this, new Vector3(20, this.wallSize, this.floorSize), new Vector3(-this.floorSize/2, 0, 0)).generate();
            new WallGenerator(this.boxPrefab, this, new Vector3(this.floorSize, this.wallSize, 20), new Vector3(0, 0, -this.floorSize/2)).generate();
            new WallGenerator(this.boxPrefab, this, new Vector3(this.floorSize, this.wallSize, 20), new Vector3(0, 0, +this.floorSize/2)).generate();
            new WallGenerator(this.boxPrefab, this, new Vector3(this.floorSize, 20, this.floorSize), new Vector3(0, -this.wallSize/2, 0)).generate();
        }

        public void start()
        {
            this.initEnvironment();
            this.startGenerators();
        }

        public void update()
        {
            this.updateGenerators();
        }
    }
}