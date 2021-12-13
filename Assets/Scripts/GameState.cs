using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(AI_Controller))]
public class GameState : MonoBehaviour
{
    public bool play = false;
    public GameObject gameSpeedSlider;
    public GameObject[] unitPrefabs;
    public GameObject flagPrefab;
    public List<Team> teams = new List<Team>()
    {
        new Team("Team", Color.white)
    };
    
    public List<War> wars = new List<War>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!play)
            return;

        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i].defeated)
                continue;

            bool f = false;
            GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
            foreach (GameObject flag in flags)
            {
                if (flag.GetComponent<FlagScript>().team == i)
                {
                    f = true;
                    break;
                }
            }

            bool u = false;
            GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
            foreach (GameObject unit in units)
            {
                if (unit.GetComponent<UnitScript>().team == i)
                {
                    u = true;
                    break;
                }
            }

            if (!f && !u)
            {
                teams[i].defeated = true;
            }

            for (int w = 0; w < wars.Count; w++)
            {
                if ((!wars[w].atkFight && !wars[w].defFight) || teams[wars[w].attacker].defeated || teams[wars[w].defender].defeated)
                {
                    wars.RemoveAt(w);
                }
            }
        }


        List<int> factions = new List<int>();
        for (int t = 0; t < teams.Count; t++)
        {
            if (!teams[t].defeated)
                factions.Add(t);
        }
        if (factions.Count == 1)
        {
            GetComponent<PlayerController>().ShowAlert(GetComponent<LanguageController>().Translate("won", teams[factions[0]].name));
            Pause();
        }
    }

    public void InitGame()
    {
        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        foreach (GameObject flag in flags)
        {
            flag.GetComponent<FlagScript>().Init(flag.GetComponent<FlagScript>().team, flag.name);
        }

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in units)
        {
            unit.GetComponent<UnitScript>().Init(unit.GetComponent<UnitScript>().team);
        }

        GetComponent<AI_Controller>().DetectFactions();
    }

    public void Play()
    {
        play = true;
    }

    public void Pause()
    {
        play = false;
    }

    public void SetGameSpeed()
    {
        Time.timeScale = gameSpeedSlider.GetComponent<UnityEngine.UI.Slider>().value;
    }
}
