using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;
using Models;

namespace Generators{
    public abstract class Generator
    {
        private System.Random random;
        protected GameObject prefab;
        
        public Generator(GameObject prefab){
            this.prefab = prefab;
            this.random = new System.Random();
        }
        private float getRand(){
            return (float)random.Next(-10, 10) / 100f;
        }
        protected float3 getNoiseVector(){
            return new float3(this.getRand(), 0, this.getRand());
        }
        protected float3 getEmptyVector(){
            return new float3(0, 0, 0);
        }
        protected GameObject createGameObject(Vector3 position){
            return UnityEngine.Object.Instantiate(this.prefab, position, Quaternion.identity);
        }
    }
}