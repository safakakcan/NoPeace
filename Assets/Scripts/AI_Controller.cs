using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameState))]
[RequireComponent(typeof(PlayerController))]
public class AI_Controller : MonoBehaviour
{
    public List<Faction> factions = new List<Faction>();
    float timer = 1;
    public float timerSecond = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Camera.main.GetComponent<GameState>().play)
            return;

        if (timer <= 0)
        {
            timer = timerSecond;

            CalcBehaviours();
            CalcDiplomacy();
            MoveUnits();
            RaiseUnits();
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void DetectFactions()
    {
        List<Team> teams = GetComponent<GameState>().teams;
        foreach (Team t in teams)
        {
            Faction f = new Faction(t.name);
            f.opportunityDistance = Random.Range(25, 100);
            f.threadDistance = Random.Range(25, 100);
            f.attackDistance = Random.Range(3, 7);
            f.defenseDistance = Random.Range(3, 7);
            foreach (Team tt in teams)
            {
                f.behaviours.Add(new Behaviour(tt.name));
            }
            factions.Add(f);
        }
    }

    void RaiseUnits()
    {
        for (int f = 0; f < factions.Count; f++)
        {
            if (factions[f].flags.Count > 0 && GetComponent<PlayerController>().team != f)
            {
                float tax = 0;
                foreach (GameObject ff in factions[f].flags)
                {
                    tax += ff.GetComponent<FlagScript>().taxRate;
                }
                foreach (GameObject uu in factions[f].units)
                {
                    tax -= uu.GetComponent<UnitScript>().info.size;
                }

                if (tax > 1 || factions[f].units.Count < 4)
                {
                    GameObject flag = factions[f].flags[Random.Range(0, factions[f].flags.Count)];
                    flag.GetComponent<FlagScript>().RaiseTroop();
                }
                else
                {
                    if (factions[f].units.Count > 0 && factions[f].units.Count < factions[f].flags.Count * 1.5f)
                        factions[f].units[Random.Range(0, factions[f].units.Count)].GetComponent<UnitScript>().SplitUnit();

                    if (Random.Range(0, 1) > 0.25f)
                    {
                        factions[f].flags[Random.Range(0, factions[f].flags.Count)].GetComponent<FlagScript>().ImproveEconomy();
                    }
                    else
                    {
                        factions[f].flags[Random.Range(0, factions[f].flags.Count)].GetComponent<FlagScript>().ImproveEconomy();
                    }
                }
            }
        }
    }

    void CalcDiplomacy()
    {
        List<War> wars = GetComponent<GameState>().wars;
        
        for (int f = 0; f < factions.Count; f++)
        {
            List<War> atkWars = new List<War>();
            List<War> defWars = new List<War>();
            foreach (War war in wars)
            {
                if (war.attacker == f)
                {
                    atkWars.Add(war);
                }
                else if (war.defender == f)
                {
                    defWars.Add(war);
                }
            }

            if (defWars.Count + atkWars.Count < 2)
            {
                int target = -1;
                float targetRate = 0;

                for (int b = 0; b < factions[f].behaviours.Count; b++)
                {
                    float rate = factions[f].behaviours[b].opportunityRate;
                    if (rate > targetRate)
                    {
                        target = b;
                        targetRate = rate;
                    }
                }

                if (target > -1)
                {
                    bool exist = false;
                    foreach (War w in GetComponent<GameState>().wars)
                    {
                        if (w.attacker == f && w.defender == target)
                        {
                            exist = true;
                            break;
                        }
                    }

                    if (!exist)
                        GetComponent<GameState>().wars.Add(new War(f, target));
                }
            }
        }
    }

    void CalcBehaviours()
    {
        factions.Clear();
        List<Team> teams = GetComponent<GameState>().teams;
        foreach (Team t in teams)
        {
            Faction f = new Faction(t.name);
            foreach (Team tt in teams)
            {
                Behaviour behaviour = new Behaviour(tt.name);
                f.behaviours.Add(behaviour);
            }
            factions.Add(f);
        }

        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject flag in flags)
        {
            factions[flag.GetComponent<FlagScript>().team].flags.Add(flag);
        }

        foreach (GameObject unit in units)
        {
            factions[unit.GetComponent<UnitScript>().team].units.Add(unit);
        }

        /*Determine Behaviours*/

        for (int f = 0; f < factions.Count; f++)
        {
            for (int b = 0; b < factions[f].behaviours.Count; b++)
            {
                if (f == b)
                    continue;
                List<float> thread = new List<float>();
                List<float> opportunity = new List<float>();

                /*Determine Threads*/
                
                for (int flag = 0; flag < factions[f].flags.Count; flag++)
                {
                    thread.Add(0);
                    
                    foreach (GameObject u in factions[b].units)
                    {
                        if (Vector3.Distance(factions[f].flags[flag].transform.position, u.transform.position) < factions[f].threadDistance)
                        {
                            thread[flag] += factions[f].threadDistance - Vector3.Distance(factions[f].flags[flag].transform.position, u.transform.position);
                        }
                    }
                }

                factions[f].behaviours[b].thread = thread.IndexOf(Mathf.Max(thread.ToArray()));
                factions[f].behaviours[b].threadRate = Mathf.Max(thread.ToArray());
                
                /*Determine Opportunities*/
                
                for (int flag = 0; flag < factions[b].flags.Count; flag++)
                {
                    opportunity.Add(0);
                    
                    foreach (GameObject u in factions[f].units)
                    {
                        if (Vector3.Distance(factions[b].flags[flag].transform.position, u.transform.position) < factions[f].opportunityDistance)
                        {
                            opportunity[flag] += factions[f].opportunityDistance - Vector3.Distance(factions[b].flags[flag].transform.position, u.transform.position);
                        }
                    }
                    
                    foreach (GameObject u in factions[b].units)
                    {
                        if (Vector3.Distance(factions[b].flags[flag].transform.position, u.transform.position) < factions[f].threadDistance)
                        {
                            opportunity[flag] -= factions[f].threadDistance - Vector3.Distance(factions[b].flags[flag].transform.position, u.transform.position);
                        }
                    }
                    
                    opportunity[flag] += factions[f].units.Count - factions[b].units.Count;
                }

                factions[f].behaviours[b].opportunity = opportunity.IndexOf(Mathf.Max(opportunity.ToArray()));
                factions[f].behaviours[b].opportunityRate = Mathf.Max(opportunity.ToArray());
            }
        }
    }

    void MoveUnits()
    {
        for (int faction = 0; faction < factions.Count; faction++)
        {
            if (GetComponent<PlayerController>().team == faction)
                continue;

            List<GameObject> units = new List<GameObject>();
            foreach (GameObject u in factions[faction].units)
            {
                if (!u.GetComponent<UnitScript>().info.retreat)
                    units.Add(u);
            }

            List<GameObject> flags = factions[faction].flags;

            //if (flags.Count <= 0)
                //continue;

            List<float> threads = new List<float>();

            for (int f = 0; f < flags.Count; f++)
            {
                threads.Add(0);
                foreach (Behaviour behaviour in factions[faction].behaviours)
                {
                    if (behaviour.thread == f)
                        threads[f] += behaviour.threadRate;
                }
            }

            int thread = threads.IndexOf(Mathf.Max(threads.ToArray()));

            List<float> opportunities = new List<float>();

            foreach (Behaviour behaviour in factions[faction].behaviours)
            {
                opportunities.Add(behaviour.opportunityRate);
            }

            int opportunity = opportunities.IndexOf(Mathf.Max(opportunities.ToArray()));
            int threadUnitCount = 0;
            if (flags.Count > 0)
                threadUnitCount = (int)(units.Count * (threads[thread] / (threads[thread] + opportunities[opportunity])));
            int opportunityUnitCount = units.Count - threadUnitCount;

            for (int i = 0; i < units.Count; i++)
            {
                if (i < threadUnitCount)
                {
                    GameObject target = null;
                    GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
                    foreach (GameObject u in allUnits)
                    {
                        if (u.GetComponent<UnitScript>().team != faction)
                        {
                            if (target == null)
                            {
                                target = u;
                            }
                            else
                            {
                                if (Vector3.Distance(flags[thread].transform.position, u.transform.position) < Vector3.Distance(units[i].transform.position, target.transform.position))
                                {
                                    target = u;
                                }
                            }
                        }
                    }

                    if (target != null && Vector3.Distance(flags[thread].transform.position, target.transform.position) < factions[faction].defenseDistance)
                    {
                        units[i].GetComponent<UnitScript>().action.type = ActionType.Attack;
                        units[i].GetComponent<UnitScript>().action.location = target.transform.position;
                    }
                    else
                    {
                        units[i].GetComponent<UnitScript>().action.type = ActionType.Move;
                        units[i].GetComponent<UnitScript>().action.location = flags[thread].transform.position;
                    }
                }
                else
                {
                    GameObject target = null;
                    GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
                    foreach (GameObject u in allUnits)
                    {
                        if (u.GetComponent<UnitScript>().team != faction)
                        {
                            if (target == null)
                            {
                                target = u;
                            }
                            else
                            {
                                if (Vector3.Distance(units[i].transform.position, u.transform.position) < Vector3.Distance(units[i].transform.position, target.transform.position))
                                {
                                    target = u;
                                }
                            }
                        }
                    }

                    if (target != null && Vector3.Distance(units[i].transform.position, target.transform.position) < factions[faction].attackDistance)
                    {
                        units[i].GetComponent<UnitScript>().action.type = ActionType.Defend;
                        units[i].GetComponent<UnitScript>().action.location = target.transform.position;
                    }
                    else
                    {
                        units[i].GetComponent<UnitScript>().action.type = ActionType.Move;
                        int targetFlagIndex = factions[faction].behaviours[opportunity].opportunity;
                        units[i].GetComponent<UnitScript>().action.location = factions[opportunity].flags[targetFlagIndex].transform.position;
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class Faction
{
    public string name = "";
    public List<Behaviour> behaviours = new List<Behaviour>();
    public List<GameObject> flags = new List<GameObject>();
    public List<GameObject> units = new List<GameObject>();
    public float opportunityDistance = 100;
    public float threadDistance = 50;
    public float attackDistance = 10;
    public float defenseDistance = 10;

    public Faction(string Name)
    {
        name = Name;
    }
}

[System.Serializable]
public class Behaviour
{
    public string name = "";
    public int thread = 0;
    public float threadRate = 0;
    public int opportunity = 0;
    public float opportunityRate = 0;

    public Behaviour(string Name)
    {
        name = Name;
    }
}