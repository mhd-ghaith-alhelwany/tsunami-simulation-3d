using UnityEngine;
using Generators.ECS.Fluid;
using Generators.ECS.Objects;
using Models;
using  Controllers;

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
        private Controller controller;

        private List<Particle> particles;
        private GameObject boxObject;
        private Transform transform;

        private GameObjectConversionSettings settings;
        private Entity particlePrefab;
        private EntityManager entityManager;
        
        private int particlesCount;

        public ECSGame(GameObject fluidPrefab, GameObject boxPrefab, Transform transform)
        {

            this.fluidPrefab = fluidPrefab;
            this.boxPrefab = boxPrefab;
            this.transform = transform;

            this.particleSize = 16f;
            this.wallSize = 500;
            this.floorSize = new Vector2(512, 512);
            this.particlesCount = 0;

            this.settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            this.particlePrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(this.getFluidPrefab(), settings);
            this.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            this.generators = new List<Generator>();

            this.generators.Add(new BucketGenerator(this, new Vector3(10, 10, 10), new Vector3(-100, 200, -200)));    
            this.generators.Add(new SeaGenerator(this, this.floorSize[0], this.floorSize[1], this.wallSize, 5));    
        }

        private int generateParticleId()
        {
            return this.particlesCount++;
        }
        
        public void createParticle(float3 position, float3 velocity)
        {            
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
            new BoxGenerator(this.boxPrefab, this, new Vector3(this.floorSize[0], 20, this.floorSize[1]), new Vector3(0, -this.wallSize/2, 0)).generate();

            new BoxGenerator(this.boxPrefab, this, new Vector3(20, this.wallSize, this.floorSize[1]), new Vector3(+this.floorSize[0]/2, 0, 0)).generate();
            new BoxGenerator(this.boxPrefab, this, new Vector3(20, this.wallSize, this.floorSize[1]), new Vector3(-this.floorSize[0]/2, 0, 0)).generate();
            new BoxGenerator(this.boxPrefab, this, new Vector3(this.floorSize[0], this.wallSize, 20), new Vector3(0, 0, -this.floorSize[1]/2)).generate();
            new BoxGenerator(this.boxPrefab, this, new Vector3(this.floorSize[0], this.wallSize, 20), new Vector3(0, 0, +this.floorSize[1]/2)).generate();

            // new BoxGenerator(this.boxPrefab, this, new Vector3(50, 50, 50), new Vector3(0, -200, 0)).generate();
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