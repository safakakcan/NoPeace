using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartographer : MonoBehaviour
{
    public string sceneName = "New Scene";
    public string fileName = "new_scene";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            Camera.main.GetComponent<PlayerController>().SaveScene(sceneName, fileName);
    }
}
