using UnityEngine;
namespace Models{
    public class Particle : Model
    {
        private float density, pressure;
        private Vector3 force, velocity, position;

        public Particle(GameObject gameObject): base(gameObject)
        {
            this.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            this.force = new Vector3(0.0f, 0.0f, 0.0f);
            this.density = 0.0f;
            this.pressure = 0.0f;
            this.setPosition(gameObject.transform.localPosition);
        }

        public void setPosition(Vector3 value){this.position = value; base.setObjectPosition(value);}
        public void setVelocity(Vector3 value){this.velocity = value;}
        public void setForce(Vector3 value){this.force = value;}
        public void setDensity(float value){this.density = value;}
        public void setPressure(float value){this.pressure = value;}
        public Vector3 getPosition(){return position;}
        public Vector3 getVelocity(){return velocity;}
        public Vector3 getForce(){return force;}
        public float getDensity(){return density;}
        public float getPressure(){return pressure;}
        
    }
}