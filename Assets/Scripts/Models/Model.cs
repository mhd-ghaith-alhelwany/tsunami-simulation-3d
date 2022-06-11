using UnityEngine;
namespace Models
{
    public abstract class Model
    {
        private GameObject gameObject;
        private int id;
        private static int count = 0;
        public Model(GameObject gameObject){this.gameObject = gameObject; this.id = count++;}
        public int getId(){return this.id;}

        public void setObjectPosition(Vector3 value){
            this.gameObject.transform.localPosition = value;
        }
    }
}