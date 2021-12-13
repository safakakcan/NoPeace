using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public Action action;
    public Info info = new Info();
    public int team = 0;
    public Sprite unitSprite;
    public Sprite shipSprite;

    // Start is called before the first frame update
    void Start()
    {
        //Init(team);
        action = new Action(ActionType.Wait, transform.position);
    }

    public void Init(int Team)
    {
        team = Team;
        info.health *= info.size;
        transform.localScale = new Vector3(0.2f * info.size, 0.2f * info.size, 1);
        GetComponent<SpriteRenderer>().material.color = Camera.main.GetComponent<GameState>().teams[team].color;
        Color startColor = Camera.main.GetComponent<GameState>().teams[team].color;
        startColor.a = 0.5f;
        Color endColor = Camera.main.GetComponent<GameState>().teams[team].color;
        endColor.a = 0.0f;
        transform.GetChild(3).GetComponent<LineRenderer>().startColor = startColor;
        transform.GetChild(3).GetComponent<LineRenderer>().endColor = endColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Camera.main.GetComponent<GameState>().play)
            return;

        if (Camera.main.GetComponent<GameState>().teams[team].treasure >= (Time.deltaTime / 4) * info.size)
        {
            Camera.main.GetComponent<GameState>().teams[team].treasure -= (Time.deltaTime / 4) * info.size;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (action != null && Vector3.Distance(transform.position, action.location) > 0.1f)
        {
            float sprint = action.type == ActionType.Attack ? 1.5f : 1f;
            transform.position = Vector3.MoveTowards(transform.position, action.location, Time.deltaTime * info.speed * sprint);
            transform.GetChild(3).GetComponent<LineRenderer>().SetPosition(0, transform.position);
            transform.GetChild(3).GetComponent<LineRenderer>().SetPosition(1, action.location);
        }

        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        GameObject closestFlag = null;
        foreach (GameObject flag in flags)
        {
            if (flag.GetComponent<FlagScript>().team == team)
            {
                if (closestFlag == null)
                {
                    closestFlag = flag;
                }
                else
                {
                    if (Vector3.Distance(transform.position, flag.transform.position) < Vector3.Distance(transform.position, closestFlag.transform.position))
                    {
                        closestFlag = flag;
                    }
                }
            }
        }

        GameObject[] targets = GetTargets();
        Attack(targets);
        transform.GetChild(0).gameObject.SetActive(targets.Length > 0);
        CalcMoral(targets);

        if (info.moral <= 0)
        {
            info.retreat = true;
            if (closestFlag != null)
                action = new Action(ActionType.Move, closestFlag.transform.position);
        }
        else if (info.moral >= 25 && info.retreat){
            info.retreat = false;
            action = new Action(ActionType.Wait, transform.position);
        }

        if (closestFlag != null)
        {
            if (Vector3.Distance(transform.position, closestFlag.transform.position) < 1 && targets.Length == 0)
            {
                info.health += Time.deltaTime / 2;
                info.moral += Time.deltaTime;

                if (info.health > 100 * info.size)
                    info.health = 100 * info.size;
                if (info.moral > 100)
                    info.moral = 100;
            }
        }

        transform.GetChild(1).localScale = new Vector3((info.health / (100 * info.size)) * 5, 0.5f, 1);
        transform.GetChild(2).localScale = new Vector3((info.moral / 100) * 5, 0.5f, 1);
    }

    public GameObject[] GetTargets()
    {
        List<GameObject> targets = new List<GameObject>();
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unit in allUnits)
        {
            if (Vector3.Distance(transform.position, unit.transform.position) < info.hitDistance && unit.GetComponent<UnitScript>().team != team)
            {
                targets.Add(unit);
            }
        }

        return targets.ToArray();
    }

    public void Attack(GameObject[] targets)
    {
        foreach (GameObject target in targets)
        {
            float act = action.type == ActionType.Attack ? 2 : 1;
            target.GetComponent<UnitScript>().Hit((info.attack * act) / targets.Length);
        }
    }


    public void Hit(float damage)
    {
        float act = action.type == ActionType.Defend ? 1.5f : 1;
        info.health -= (damage / (info.defence * act)) * Time.deltaTime;

        if (info.health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void CalcMoral(GameObject[] targets)
    {
        float def = action.type == ActionType.Defend ? 2 : 1;
        info.moral -= (targets.Length * Time.deltaTime * 6) / def;
        info.moral += Time.deltaTime;
        if (info.moral > 100)
        {
            info.moral = 100;
        }
        else if (info.moral < 0)
        {
            info.moral = 0;
        }
    }

    public void SplitUnit()
    {
        if (info.size < 1)
            return;

        transform.position = transform.position - new Vector3(0.25f, 0, 0);
        action = new Action(ActionType.Wait, transform.position);
        info.size = info.size / 2;
        info.hitDistance = 1;
        Init(team);

        GameObject other = Instantiate(Camera.main.GetComponent<GameState>().unitPrefabs[Camera.main.GetComponent<GameState>().teams[team].unit]);
        other.transform.position = transform.position + new Vector3(0.25f, 0, 0);
        other.GetComponent<UnitScript>().info = info;
        other.GetComponent<UnitScript>().Init(team);
    }

    private void OnMouseDown()
    {
        if (Camera.main.GetComponent<PlayerController>().team == team && Camera.main.GetComponent<PlayerController>().selectedUnit == null)
        {
            Camera.main.GetComponent<PlayerController>().selectedUnit = this.gameObject;
            Camera.main.GetComponent<PlayerController>().selectedAction = new Action(ActionType.Wait, transform.position);
            Camera.main.GetComponent<PlayerController>().ToggleCommands(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Obstacle")
        {
            action = new Action(ActionType.Wait, transform.position);
        }
        else if (collision.collider.tag == "Unit")
        {
            if (!info.retreat && collision.collider.GetComponent<UnitScript>().team != team)
                action = new Action(ActionType.Wait, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            info.speed = info.speed / 2;
            GetComponent<SpriteRenderer>().sprite = shipSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            info.speed = info.speed * 2;
            GetComponent<SpriteRenderer>().sprite = unitSprite;
        }
    }
}
