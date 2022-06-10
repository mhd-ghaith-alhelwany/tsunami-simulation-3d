using UnityEngine;
namespace Models
{
    public abstract class Model
    {
        private GameObject gameObject;
        public Model(GameObject gameObject){this.gameObject = gameObject;}
        public void setObjectPosition(Vector3 value){
            this.gameObject.transform.localPosition = value;
        }
    }
}