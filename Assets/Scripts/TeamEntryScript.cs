using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamEntryScript : MonoBehaviour
{
    public int team = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectTeam()
    {
        Camera.main.GetComponent<PlayerController>().StartPlayer(team);
        Camera.main.GetComponent<PlayerController>().selectTeam.SetActive(false);
        Camera.main.GetComponent<PlayerController>().gameMenu.SetActive(false);
    }
}
