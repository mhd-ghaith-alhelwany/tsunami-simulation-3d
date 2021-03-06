using UnityEngine;
using Unity.Mathematics;

namespace Models{
    public class Particle : Model
    {
        private float density, pressure;
        private float3 force, velocity, position;
        private int3 positionInGrid;

        public Particle(GameObject gameObject): base(gameObject)
        {
            this.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            this.force = new Vector3(0.0f, 0.0f, 0.0f);
            this.density = 0.0f;
            this.pressure = 0.0f;
            this.setPosition(gameObject.transform.localPosition);
        }

        public void setPosition(float3 value){
            base.setObjectPosition(value); 
            this.position = value; 
        }
        public void setVelocity(float3 value){this.velocity = value;}
        public void setForce(float3 value){this.force = value;}
        public void setDensity(float value){this.density = value;}
        public void setPressure(float value){this.pressure = value;}
        public void setPositionInGrid(int3 value){this.positionInGrid = value;}
        
        public float3 getPosition(){return position;}
        public float3 getVelocity(){return velocity;}
        public float3 getForce(){return force;}
        public float getDensity(){return density;}
        public float getPressure(){return pressure;}
        public int3 getPositionInGrid(){return positionInGrid;}
        
    }
}