using UnityEngine;
using Main;

public class ECSEngine : MonoBehaviour
{
    public GameObject particlePrefab;
    public GameObject boxPrefab;
    public ECSGame game;
    
    void Start()
    {        
        this.game = new ECSGame(particlePrefab, boxPrefab, transform);
        this.game.start();
    }

    void Update()
    {
        this.game.update();
    }

    public void buttonClicked()
    {
        Debug.Log(123);
    }
}
