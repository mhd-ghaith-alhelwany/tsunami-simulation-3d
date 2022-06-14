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
        public bool isStarted;

        public ECSGame(GameObject particlePrefab, GameObject boxPrefab)
        {
            this.isStarted = false;
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
            for(int i = 0; i < this.generators.Count; i++){
                List<Particle> particles = generators[i].update(this.particles);
                if(particles == null)
                    this.generators.RemoveAt(i);
                else{
                    this.particles = particles;
                    this.restartControllers();
                }
            }
        }

        private void updateControllers()
        {
            foreach(Controller controller in this.controllers)
                controller.update();
        }

        private void initFluidGenerators()
        {
            this.generators.Add(new SeaGenerator(this.particlePrefab));
        }

        private void initRoom()
        {
            new BoxGenerator(this.boxPrefab, new Vector3(Simulation.RoomSizeX, Simulation.wallsThickness, Simulation.RoomSizeZ), new Vector3(0, -Simulation.RoomSizeY/2, 0), false).start();
            new BoxGenerator(this.boxPrefab, new Vector3(Simulation.wallsThickness, Simulation.RoomSizeY, Simulation.RoomSizeZ), new Vector3(+Simulation.RoomSizeX/2, 0, 0), false).start();
            new BoxGenerator(this.boxPrefab, new Vector3(Simulation.wallsThickness, Simulation.RoomSizeY, Simulation.RoomSizeZ), new Vector3(-Simulation.RoomSizeX/2, 0, 0), false).start();
            new BoxGenerator(this.boxPrefab, new Vector3(Simulation.RoomSizeX, Simulation.RoomSizeY, Simulation.wallsThickness), new Vector3(0, 0, -Simulation.RoomSizeZ/2), false).start();
            new BoxGenerator(this.boxPrefab, new Vector3(Simulation.RoomSizeX, Simulation.RoomSizeY, Simulation.wallsThickness), new Vector3(0, 0, +Simulation.RoomSizeZ/2), false).start();
        }

        public void initControllers()
        {
            SPHController sphController = new SPHController();
            sphController.setParticles(particles);
            this.controllers.Add(sphController);
        }

        public void restartControllers()
        {
            foreach(Controller controller in controllers)
                controller.Dispose();

            this.controllers = new List<Controller>();
            this.initControllers();
        }
        
        public void addFluidGenerator(FluidGenerator generator)
        {
            this.particles = generator.start(this.particles);
            this.generators.Add(generator);
            this.restartControllers();
        }

        public void dropBucketButtonPressed()
        {
            this.addFluidGenerator(new BucketGenerator(this.particlePrefab, new Vector3(8, 8, 8), new Vector3(-64 , 0, 0)));
        }
        public void startWaveButtonPressed()
        {
            this.addFluidGenerator(new WaveGenerator(this.particlePrefab));
        }

        public void start()
        {
            this.isStarted = true;
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