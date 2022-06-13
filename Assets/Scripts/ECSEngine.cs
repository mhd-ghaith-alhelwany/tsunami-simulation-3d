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
    }

    void Update()
    {
        Debug.Log(this.game.isStarted);
        if(this.game.isStarted)
            this.game.update();
    }

    private GameObject[] UIObjects;
    public void startButton()
    {
        this.game.start();
        UIObjects = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject uiObject in UIObjects) Destroy(uiObject);
    }
}
