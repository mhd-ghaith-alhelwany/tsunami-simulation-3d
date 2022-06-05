using UnityEngine;
using Generators.ECS;
using Models;
using  Controllers;

using System.Collections.Generic;

namespace Main {

    public class ECSGame
    {
        private float particleSize;
        private Vector3 boxSize;
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
            this.boxSize = new Vector3(500, 500, 500);
            this.generators = new List<Generator>();

            this.generators.Add(new BucketGenerator(this, new Vector3(10, 10, 10), new Vector3(0, 250, 0)));
        }

        public GameObject getFluidPrefab()
        {
            return this.fluidPrefab;
        }

        public Transform getTransform()
        {
            return this.transform;
        }
        
        public Vector3 getBoxSize()
        {
            return this.boxSize;
        }

        public float getParticleSize()
        {
            return this.particleSize;
        }

        public bool isOutsideBox(Vector3 point)
        {
            return 
                point[0] >= this.boxSize[0] / 2 || 
                point[1] >= this.boxSize[1] || 
                point[2] >= this.boxSize[2] / 2 || 
                point[0] <= -1 * this.boxSize[0] / 2 || 
                point[1] <= 0 ||
                point[2] <= -1 * this.boxSize[2] / 2;
        }

        private void startController()
        {
            if(this.controller != null)
                this.controller.start();
        }

        private void startGenerators()
        {
            foreach(Generator generator in this.generators)
                generator.start();
        }

        private void updateController()
        {
            if(this.controller != null)
                this.controller.update();
        }

        private void updateGenerators()
        {
            foreach(Generator generator in this.generators)
                generator.update();
        }

        private void initEnvironment()
        {
            this.boxPrefab.transform.localScale = this.boxSize;
            this.boxObject = UnityEngine.Object.Instantiate(this.boxPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            this.fluidPrefab.transform.localScale = new Vector3(this.particleSize, this.particleSize, this.particleSize);
        }

        public void start()
        {
            this.initEnvironment();
            this.startGenerators();
            this.startController();
        }

        public void update()
        {
            this.updateController();
            this.updateGenerators();
        }
    }
}