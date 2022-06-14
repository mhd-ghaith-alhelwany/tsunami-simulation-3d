using UnityEngine;
using Main;

public class ECSEngine : MonoBehaviour
{
    public GameObject particlePrefab;
    public GameObject boxPrefab;
    public ECSGame game;
    void Start()
    {        
        this.game = new ECSGame(particlePrefab, boxPrefab);
        this.game.start();
    }

    void Update()
    {
        if(this.game.isStarted)
            this.game.update();
    }
    
    public void startWaveButton()
    {
        this.game.startWaveButtonPressed();
    }

    public void dropBucketButton()
    {
        this.game.dropBucketButtonPressed();
    }
}
