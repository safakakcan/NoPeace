using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ActionType
{
    Wait,
    Move,
    Defend,
    Attack
}

[System.Serializable]
public class Action
{
    public ActionType type = ActionType.Wait;
    public Vector3 location;

    public Action(ActionType Type, Vector3 Location)
    {
        type = Type;
        location = Location;
    }
}

[System.Serializable]
public class Info
{
    public string name = "Unit";
    public float size = 1;
    public float health = 100f;
    public float attack = 1f;
    public float defence = 1f;
    public float speed = 1f;
    public float moral = 100f;
    public float hitDistance = 1.25f;
    public bool retreat = false;
}

[System.Serializable]
public class Team
{
    public string name;
    public Color color;
    public int unit;
    public float treasure;
    public float population;
    public bool defeated;
    public bool isPlayer;

    public Team(string Name, Color Color, bool IsPlayer = false)
    {
        name = Name;
        color = Color;
        unit = 0;
        treasure = 100;
        population = 5000;
        defeated = false;
        isPlayer = IsPlayer;
    }
}

public class War
{
    public int attacker;
    public int defender;
    public bool atkFight;
    public bool defFight;

    public War (int Attacker, int Defender)
    {
        attacker = Attacker;
        defender = Defender;
        atkFight = true;
        defFight = false;
    }
}

[System.Serializable]
public class GameStateSaveObject
{
    public List<Team> teams;
    public List<Faction> factions;
    public int playerTeam;
    public float minZoom;
    public float maxZoom;
    public float zoomSpeed;
    public float mapXLength;
    public float mapYLength;
}

[System.Serializable]
public class MapSaveObject
{
    public string name;
    public Vector3 position;
}

[System.Serializable]
public class FlagSaveObject
{
    public string name;
    public int team;
    public float taxRate;
    public float populationGrowth;
    public Vector3 position;
}

[System.Serializable]
public class UnitSaveObject
{
    public Action action;
    public Info info;
    public int team;
    public Vector3 position;
}

[System.Serializable]
public class SaveObject
{
    public string name = "";
    public GameStateSaveObject gameStateSaveObject = new GameStateSaveObject();
    public MapSaveObject mapSaveObject = new MapSaveObject();
    public List<FlagSaveObject> flagSaveObject = new List<FlagSaveObject>();
    public List<UnitSaveObject> unitSaveObject = new List<UnitSaveObject>();
}

[System.Serializable]
public class SaveLog
{
    public List<SaveObject> saveObjects = new List<SaveObject>();
}