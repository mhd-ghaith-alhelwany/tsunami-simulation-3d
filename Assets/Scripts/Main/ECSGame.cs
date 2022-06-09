using UnityEngine;
using Generators;

using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Components;

namespace Main {

    public class ECSGame
    {
        private float particleSize;
        private float wallSize;
        private Vector2 floorSize;
        private GameObject fluidPrefab;
        private GameObject boxPrefab;

        private List<Generator> generators;

        private GameObject boxObject;
        private Transform transform;

        private GameObjectConversionSettings settings;
        private Entity particlePrefab;
        private EntityManager entityManager;
        
        private int particlesCount;

        private System.Random random;

        public ECSGame(GameObject fluidPrefab, GameObject boxPrefab, Transform transform)
        {

            this.fluidPrefab = fluidPrefab;
            this.boxPrefab = boxPrefab;
            this.transform = transform;

            this.particleSize = 16f;
            this.wallSize = Config.wallSize;
            this.floorSize = new Vector2(Config.floorX, Config.floorY);
            this.particlesCount = 0;

            this.settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            this.particlePrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(this.getFluidPrefab(), settings);
            this.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            this.generators = new List<Generator>();
            this.random = new System.Random();
        }

        public Vector2 getFloorSize()
        {
            return this.floorSize;
        }

        private int generateParticleId()
        {
            return this.particlesCount++;
        }
        
        public void createParticle(float3 position, float3 velocity)
        {         
            float size = this.particleSize;
            this.fluidPrefab.transform.localScale = new Vector3(this.particleSize, this.particleSize, this.particleSize);   
            var instance = this.entityManager.Instantiate(this.particlePrefab);
            this.entityManager.AddComponentData(instance, new SphParticle{
                particleId = this.generateParticleId(),
                force = new float3(0.0f, 0.0f, 0.0f),
                position = position,
                velocity = velocity,
                density = 0.0f,
                pressure = 0.0f
            });
        }

        private float getRand()
        {
            return this.random.Next(-(int)this.getParticleSize(), (int)this.getParticleSize()) / 4;
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
            //Solids
            this.generators.Add(new BoxGenerator(this.boxPrefab, this, new Vector3(this.floorSize[0], Config.wallsThickness, this.floorSize[1]), new Vector3(0, -this.wallSize/2, 0)));
            this.generators.Add(new BoxGenerator(this.boxPrefab, this, new Vector3(Config.wallsThickness, this.wallSize, this.floorSize[1]), new Vector3(+this.floorSize[0]/2, 0, 0)));
            this.generators.Add(new BoxGenerator(this.boxPrefab, this, new Vector3(Config.wallsThickness, this.wallSize, this.floorSize[1]), new Vector3(-this.floorSize[0]/2, 0, 0)));
            this.generators.Add(new BoxGenerator(this.boxPrefab, this, new Vector3(this.floorSize[0], this.wallSize, Config.wallsThickness), new Vector3(0, 0, -this.floorSize[1]/2)));
            this.generators.Add(new BoxGenerator(this.boxPrefab, this, new Vector3(this.floorSize[0], this.wallSize, Config.wallsThickness), new Vector3(0, 0, +this.floorSize[1]/2)));
            // new BoxGenerator(this.boxPrefab, this, new Vector3(50, 50, 50), new Vector3(0, -200, 0));

            //Fluid
            this.generators.Add(new SeaGenerator(this, this.floorSize[0], this.floorSize[1], this.wallSize, Config.numberOfLayersInSea));    
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

        public void buttonClicked()
        {
            new BucketGenerator(this, new Vector3(4, 4, 4), new Vector3(-150, 200, -100)).start();    
        }
    }
}