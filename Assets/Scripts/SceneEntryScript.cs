using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntryScript : MonoBehaviour
{
    public int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectScene()
    {
        string sceneName = Resources.LoadAll<TextAsset>("Scenes/")[index].name;

        if (Camera.main.GetComponent<PlayerController>().selectNewScene.activeSelf)
        {
            Camera.main.GetComponent<PlayerController>().LoadScene(sceneName);
            Camera.main.GetComponent<PlayerController>().ViewTeams();
            Camera.main.GetComponent<PlayerController>().selectNewScene.SetActive(false);
            Camera.main.GetComponent<PlayerController>().selectTeam.SetActive(true);
        }
        else
        {
            Camera.main.GetComponent<PlayerController>().LoadGame(sceneName);
        }
    }
}
