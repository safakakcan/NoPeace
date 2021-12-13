using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagScript : MonoBehaviour
{
    public int team = 0;
    public float taxRate = 1;
    public float populationGrowth = 1;
    public float military = 1;
    public bool raiseTroop = false;
    public float raiseTimer = 0;
    public GameObject text;
    public GameObject raiseBar;
    public GameObject select;

    // Start is called before the first frame update
    void Start()
    {
        //Init(team, gameObject.name);
    }

    public void Init(int Team, string Name)
    {
        text.GetComponent<TextMesh>().text = Name;
        ChangeTeam(Team);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Camera.main.GetComponent<GameState>().play)
            return;

        Camera.main.GetComponent<GameState>().teams[team].treasure += Time.deltaTime * taxRate;
        Camera.main.GetComponent<GameState>().teams[team].population += Time.deltaTime * populationGrowth;

        if (raiseTroop)
        {
            raiseTimer -= Time.deltaTime * military;
            raiseBar.transform.localScale = new Vector3((raiseTimer / (10 / military)) * 2.5f, 0.25f, 1);

            if (raiseTimer <= 0)
            {
                raiseTimer = 0;
                raiseTroop = false;
                raiseBar.SetActive(false);
                PlaceTroop();
            }
        }
    }

    public void ChangeTeam(int index)
    {
        team = index;
        GetComponent<SpriteRenderer>().material.color = Camera.main.GetComponent<GameState>().teams[index].color;
        raiseBar.GetComponent<SpriteRenderer>().material.color = Camera.main.GetComponent<GameState>().teams[index].color;
        select.GetComponent<SpriteRenderer>().material.color = Camera.main.GetComponent<GameState>().teams[index].color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Unit")
        {
            /*
            List<War> wars = Camera.main.GetComponent<GameState>().wars;
            bool isAttacker = false;
            foreach (War w in wars)
            {
                if (w.attacker == collision.GetComponent<UnitScript>().team && w.defender == team)
                {
                    isAttacker = true;
                    break;
                }
            }

            if (isAttacker)
            */
            ChangeTeam(collision.GetComponent<UnitScript>().team);
            StopRaising();
        }
    }

    public void RaiseTroop()
    {
        if (!raiseTroop && Camera.main.GetComponent<GameState>().teams[team].treasure >= 125 && Camera.main.GetComponent<GameState>().teams[team].population >= 250)
        {
            Camera.main.GetComponent<GameState>().teams[team].treasure -= 125;
            Camera.main.GetComponent<GameState>().teams[team].population -= 250;
            raiseTimer = 10 / military;
            raiseTroop = true;
            if (Camera.main.GetComponent<PlayerController>().team == team)
                raiseBar.SetActive(true);
        }
    }

    public void StopRaising()
    {
        raiseTroop = false;
        raiseBar.SetActive(false);
    }

    public void PlaceTroop()
    {
        GameObject unit = (GameObject)Instantiate(Camera.main.GetComponent<GameState>().unitPrefabs[Camera.main.GetComponent<GameState>().teams[team].unit]);
        unit.GetComponent<UnitScript>().Init(team);
        unit.transform.position = transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        military += 0.1f;
        if (gameObject == Camera.main.GetComponent<PlayerController>().selectedFlag)
        {
            Camera.main.GetComponent<PlayerController>().ShowFlagPanel(Camera.main.GetComponent<PlayerController>().selectedFlag);
        }
    }

    public void GrowPopulation()
    {
        if (Camera.main.GetComponent<GameState>().teams[team].treasure >= 125 && Camera.main.GetComponent<GameState>().teams[team].population >= 75)
        {
            Camera.main.GetComponent<GameState>().teams[team].treasure -= 125;
            Camera.main.GetComponent<GameState>().teams[team].population -= 75;
            populationGrowth += 0.2f;
        }
    }

    public void ImproveEconomy()
    {
        if (Camera.main.GetComponent<GameState>().teams[team].treasure >= 125 && Camera.main.GetComponent<GameState>().teams[team].population >= 250)
        {
            Camera.main.GetComponent<GameState>().teams[team].treasure -= 125;
            Camera.main.GetComponent<GameState>().teams[team].population -= 250;
            taxRate += 0.2f;
        }
    }
}
