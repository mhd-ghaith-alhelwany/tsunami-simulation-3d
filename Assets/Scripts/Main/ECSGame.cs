using UnityEngine;
using Generators;
using Controllers;
using System.Collections.Generic;
using Unity.Entities;
using Config;

namespace Main {

    public class ECSGame
    {
        private GameObject particlePrefab;
        private GameObject boxPrefab;

        private List<Generator> generators;
        private SPHController controller;

        private GameObjectConversionSettings settings;
        private EntityManager entityManager;

        private Entity particlePrefabEntity;
        private Entity boxPrefabEntity;
        
        private System.Random random;

        public ECSGame(GameObject particlePrefab, GameObject boxPrefab)
        {

            this.particlePrefab = particlePrefab;
            this.boxPrefab = boxPrefab;

            this.generators = new List<Generator>();
            
            this.settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            this.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            this.particlePrefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(this.particlePrefab, settings);
            this.boxPrefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(this.boxPrefab, settings);

            this.random = new System.Random();
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
            this.initRoom();
        }

        private void initRoom()
        {
            //Solids
            this.generators.Add(new BoxGenerator(this.boxPrefabEntity, this.entityManager, new Vector3(Simulation.floorX, Simulation.wallsThickness, Simulation.floorY), new Vector3(0, -Simulation.wallSize/2, 0), false));
            this.generators.Add(new BoxGenerator(this.boxPrefabEntity, this.entityManager, new Vector3(Simulation.wallsThickness, Simulation.wallSize, Simulation.floorY), new Vector3(+Simulation.floorX/2, 0, 0), false));
            this.generators.Add(new BoxGenerator(this.boxPrefabEntity, this.entityManager, new Vector3(Simulation.wallsThickness, Simulation.wallSize, Simulation.floorY), new Vector3(-Simulation.floorX/2, 0, 0), false));
            this.generators.Add(new BoxGenerator(this.boxPrefabEntity, this.entityManager, new Vector3(Simulation.floorX, Simulation.wallSize, Simulation.wallsThickness), new Vector3(0, 0, -Simulation.floorY/2), false));
            this.generators.Add(new BoxGenerator(this.boxPrefabEntity, this.entityManager, new Vector3(Simulation.floorX, Simulation.wallSize, Simulation.wallsThickness), new Vector3(0, 0, +Simulation.floorY/2), false));

            //Fluid
            this.generators.Add(new SeaGenerator(this.particlePrefabEntity, this.entityManager, Simulation.numberOfLayersInSea));    
        }

        public void startController()
        {
            this.controller.start();
        }

        public void updateController()
        {
            this.controller.update();
        }


        public void start()
        {
            this.initEnvironment();
            this.startGenerators();
            this.startController();
        }

        public void update()
        {
            this.updateGenerators();
            this.updateController();
        }

        public void buttonClicked()
        {
            new BucketGenerator(this.particlePrefabEntity, this.entityManager, new Vector3(4, 4, 4), new Vector3(-150, 200, -100)).start();
        }
    }
}