using UnityEngine;
using UnityEngine.UI;

public class Input : MonoBehaviour
{
    void Start(){}
    void Update(){}

    public void setX(string str){ Config.Simulation.numberOfParticlesX = int.Parse(str); }
    public void setY(string str){ Config.Simulation.numberOfParticlesY = int.Parse(str); }
    public void setZ(string str){ Config.Simulation.numberOfParticlesZ = int.Parse(str); }
    public void setNumberOfWaves(string str){ Config.Simulation.numberOfWaves = int.Parse(str); }
    public void setWaveSize(string str){ Config.Simulation.waveSize = int.Parse(str); }
}
