using UnityEngine;
using UnityEngine.UI;

public class Input : MonoBehaviour
{
    void Start(){}
    void Update(){}

    public void setX(string str){ Config.Simulation.numberOfParticlesX = int.Parse(str); }
    public void setY(string str){ Config.Simulation.numberOfParticlesY = int.Parse(str); }
    public void setZ(string str){ Config.Simulation.numberOfParticlesZ = int.Parse(str); }
}
