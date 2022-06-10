using UnityEngine;
using Generators;
using System.Collections.Generic;
using Config;
using Models;
using Controllers;

namespace Main {

    public class ECSGame
    {
        private GameObject particlePrefab;
        private GameObject boxPrefab;

        private System.Random random;

        private List<FluidGenerator> generators;
        private List<Particle> particles;
        private List<Controller> controllers;
        
        public ECSGame(GameObject particlePrefab, GameObject boxPrefab)
        {

            this.particlePrefab = particlePrefab;
            this.boxPrefab = boxPrefab;
            this.random = new System.Random();

            this.generators = new List<FluidGenerator>();
            this.particles = new List<Particle>();
            this.controllers = new List<Controller>();
        }

        private void startFluidGenerators()
        {
            foreach(FluidGenerator generator in this.generators)
                this.particles = generator.start(this.particles);
        }

        private void updateGenerators()
        {
            foreach(FluidGenerator generator in this.generators)
                this.particles = generator.update(this.particles);
        }

        private void updateControllers()
        {
            foreach(Controller controller in this.controllers)
                controller.update();
        }

        private void initFluidGenerators()
        {
            this.generators.Add(new SeaGenerator(this.particlePrefab, Simulation.numberOfLayersInSea));    
        }

        private void initRoom()
        {
            new BoxGenerator(this.boxPrefab, new Vector3(Simulation.floorX, Simulation.wallsThickness, Simulation.floorY), new Vector3(0, -Simulation.wallSize/2, 0), false).start();
            new BoxGenerator(this.boxPrefab, new Vector3(Simulation.wallsThickness, Simulation.wallSize, Simulation.floorY), new Vector3(+Simulation.floorX/2, 0, 0), false).start();
            new BoxGenerator(this.boxPrefab, new Vector3(Simulation.wallsThickness, Simulation.wallSize, Simulation.floorY), new Vector3(-Simulation.floorX/2, 0, 0), false).start();
            new BoxGenerator(this.boxPrefab, new Vector3(Simulation.floorX, Simulation.wallSize, Simulation.wallsThickness), new Vector3(0, 0, -Simulation.floorY/2), false).start();
            new BoxGenerator(this.boxPrefab, new Vector3(Simulation.floorX, Simulation.wallSize, Simulation.wallsThickness), new Vector3(0, 0, +Simulation.floorY/2), false).start();
        }

        public void initControllers()
        {
            SPHController sphController = new SPHController();
            sphController.setParticles(particles);
            this.controllers.Add(sphController);
        }
        
        public void addFluidGenerator(FluidGenerator generator)
        {
            this.particles = generator.start(this.particles);
            this.generators.Add(generator);
        }

        public void buttonClicked()
        {
            BucketGenerator generator = new BucketGenerator(this.particlePrefab, new Vector3(4, 4, 4), new Vector3(-150, 200, -100));
            this.addFluidGenerator(generator);
        }

        public void start()
        {
            this.initRoom();
            this.initFluidGenerators();
            this.startFluidGenerators();
            this.initControllers();
        }

        public void update()
        {
            this.updateGenerators();
            this.updateControllers();
        }
    }
}