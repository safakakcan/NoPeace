using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameState))]
[RequireComponent(typeof(AI_Controller))]
public class PlayerController : MonoBehaviour
{
    public bool loaded = false;
    public int team = 0;

    public GameObject callTroops;
    public GameObject alert;
    public GameObject gameMenu;
    public GameObject selectNewScene;
    public GameObject sceneList;
    public GameObject sceneEntry;
    public GameObject selectTeam;
    public GameObject teamList;
    public GameObject teamEntry;

    public GameObject pointer;
    public GameObject commands;
    public GameObject flagPanel;
    public GameObject teamText;
    public GameObject militaryText;
    public GameObject treasureText;
    public GameObject populationText;
    public GameObject wars;
    public GameObject warEntry;
    public GameObject selectedUnit = null;
    public Action selectedAction = null;
    public GameObject selectedFlag = null;

    public float minZoom = 5;
    public float maxZoom = 50;
    public float zoomSpeed = 4;
    public float mapXLength = 100;
    public float mapYLength = 100;

    Vector3 mouseOrigin;
    float saveTimer = 10;

    // Start is called before the first frame update
    void Start()
    {
        //SetLoaded(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            ScreenCapture.CaptureScreenshot((System.DateTime.Now.Ticks.ToString() + ".png"), 1);

        if (GetComponent<GameState>().play)
        {
            treasureText.GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate("treasure", ((int)GetComponent<GameState>().teams[team].treasure).ToString());
            populationText.GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate("population", ((int)GetComponent<GameState>().teams[team].population).ToString());
        }

        if (Input.touchCount == 2)
        {
            Pinch();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (selectedUnit != null)
            {
                if (selectedAction.type == ActionType.Move)
                {
                    selectedAction.location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    selectedAction.location.z = 0;
                    selectedUnit.GetComponent<UnitScript>().action = selectedAction;
                    ToggleCommands(false);
                }
                else if(selectedAction.type == ActionType.Attack)
                {
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pos.z = 0;
                    GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
                    foreach(GameObject unit in units)
                    {
                        if (Vector3.Distance(pos, unit.transform.position) <= 1)
                        {
                            selectedAction.location = pos;
                            selectedUnit.GetComponent<UnitScript>().action = selectedAction;
                            ToggleCommands(false);
                            break;
                        }
                    }
                }
            }
            else
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
                foreach (GameObject flag in flags)
                {
                    if (Vector3.Distance(pos, flag.transform.position) < 1)
                    {
                        ShowFlagPanel(flag);
                        break;
                    }
                }
            }
            mouseOrigin = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
            pos.x = pos.x / 2;
            pos.y = pos.y / 2;
            transform.Translate(pos * (-3.5f * GetComponent<Camera>().orthographicSize));
            mouseOrigin = Input.mousePosition;
        }

        GetComponent<Camera>().orthographicSize -= Input.mouseScrollDelta.y;

        if (GetComponent<Camera>().orthographicSize < minZoom) { GetComponent<Camera>().orthographicSize = minZoom; }
        else if (GetComponent<Camera>().orthographicSize > maxZoom) { GetComponent<Camera>().orthographicSize = maxZoom; }
        
        if (Camera.main.transform.position.x < 0) { Camera.main.transform.position = new Vector3(0, Camera.main.transform.position.y, -10); }
        else if (Camera.main.transform.position.x > mapXLength) { Camera.main.transform.position = new Vector3(mapXLength, Camera.main.transform.position.y, -10); }
        if (Camera.main.transform.position.y < 0) { Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 0, -10); }
        else if (Camera.main.transform.position.y > mapYLength) { Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, mapYLength, -10); }
        
        if (selectedUnit != null)
        {
            pointer.transform.position = selectedUnit.transform.position;
            if (commands.activeSelf)
            {
                commands.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = selectedUnit.GetComponent<UnitScript>().info.retreat ? GetComponent<LanguageController>().Translate("status", GetComponent<LanguageController>().Translate("retreat")) : GetComponent<LanguageController>().Translate("status", GetComponent<LanguageController>().Translate(selectedUnit.GetComponent<UnitScript>().action.type.ToString()));
                commands.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color = selectedUnit.GetComponent<UnitScript>().info.retreat ? new Color(1, 0.5f, 0.5f) : Color.white;
                commands.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().interactable = !selectedUnit.GetComponent<UnitScript>().info.retreat;
                commands.transform.GetChild(2).GetComponent<UnityEngine.UI.Button>().interactable = !selectedUnit.GetComponent<UnitScript>().info.retreat;
                commands.transform.GetChild(3).GetComponent<UnityEngine.UI.Button>().interactable = !selectedUnit.GetComponent<UnitScript>().info.retreat;
                commands.transform.GetChild(4).GetComponent<UnityEngine.UI.Button>().interactable = !selectedUnit.GetComponent<UnitScript>().info.retreat;
                commands.transform.GetChild(5).GetComponent<UnityEngine.UI.Button>().interactable = !selectedUnit.GetComponent<UnitScript>().info.retreat && selectedUnit.GetComponent<UnitScript>().info.size >= 1;
            }
        }
    }

    void Pinch()
    {
        Touch t1 = Input.GetTouch(0);
        Touch t2 = Input.GetTouch(1);

        float dist1 = Vector2.Distance(t1.position - t1.deltaPosition, t2.position - t2.deltaPosition);
        float dist2 = Vector2.Distance(t1.position, t2.position);
        float zoom = (dist1 - dist2) / (100 / zoomSpeed);

        if (Camera.main.orthographicSize + zoom > maxZoom)
        {
            Camera.main.orthographicSize = maxZoom;
        }
        else if (Camera.main.orthographicSize + zoom < minZoom)
        {
            Camera.main.orthographicSize = minZoom;
        }
        else
        {
            Camera.main.orthographicSize += zoom;
        }

        /*
        Vector2 screenPos1 = (t1.position + t2.position) / 2;
        Vector2 screenPos2 = ((t1.position - t1.deltaPosition) + (t2.position - t2.deltaPosition)) / 2;
        Vector2 screenPos = screenPos1 - screenPos2;
        Vector2 value = -screenPos / (200 / zoomSpeed) * (Camera.main.orthographicSize / 10);
        Camera.main.transform.Translate(value);
        */
    }

    public void SelectAction(int index = 0)
    {
        if (index == 0)
        {
            selectedAction.type = ActionType.Wait;
            selectedAction.location = selectedUnit.transform.position;
            selectedUnit.GetComponent<UnitScript>().action = selectedAction;
            ToggleCommands(false);
        }
        else if (index == 1)
        {
            selectedAction.type = ActionType.Move;
        }
        else if (index == 2)
        {
            selectedAction.type = ActionType.Attack;
        }
        else if (index == 3)
        {
            selectedAction.type = ActionType.Defend;
            selectedAction.location = selectedUnit.transform.position;
            selectedUnit.GetComponent<UnitScript>().action = selectedAction;
            ToggleCommands(false);
        }
    }

    public void SplitUnit()
    {
        if (selectedUnit != null)
        {
            selectedUnit.GetComponent<UnitScript>().SplitUnit();
            ToggleCommands(false);
        }
    }

    public void ToggleCommands(bool show = true)
    {
        if (!show)
            selectedUnit = null;
        pointer.SetActive(show);
        commands.SetActive(show);
    }

    public void SelectTeam(int index = 0)
    {
        team = 0;
        foreach (Team t in GetComponent<GameState>().teams)
        {
            t.isPlayer = false;
        }
        GetComponent<GameState>().teams[index].isPlayer = true;
    }

    public void ViewScenes()
    {
        for (int i = 0; i < sceneList.GetComponent<UnityEngine.UI.ScrollRect>().content.childCount; i++)
        {
            Transform c = sceneList.GetComponent<UnityEngine.UI.ScrollRect>().content.GetChild(i);
            Destroy(c.gameObject);
        }

        TextAsset[] scenes = Resources.LoadAll<TextAsset>("Scenes/");

        for (int f = 0; f < scenes.Length; f++)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(scenes[f].text);
            GameObject entry = (GameObject)Instantiate(sceneEntry);
            entry.GetComponent<SceneEntryScript>().index = f;
            entry.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate(saveObject.name);
            entry.transform.SetParent(sceneList.GetComponent<UnityEngine.UI.ScrollRect>().content, false);
        }
    }

    public void ViewTeams()
    {
        for (int i = 0; i < teamList.GetComponent<UnityEngine.UI.ScrollRect>().content.childCount; i++)
        {
            Transform c = teamList.GetComponent<UnityEngine.UI.ScrollRect>().content.GetChild(i);
            Destroy(c.gameObject);
        }

        List<Team> teams = GetComponent<GameState>().teams;

        for (int f = 0; f < teams.Count; f++)
        {
            GameObject entry = (GameObject)Instantiate(teamEntry);
            entry.GetComponent<TeamEntryScript>().team = f;
            entry.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate(teams[f].name);
            entry.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color = teams[f].color;
            entry.transform.SetParent(teamList.GetComponent<UnityEngine.UI.ScrollRect>().content, false);
        }
    }

    public void ShowFlagPanel(GameObject flag)
    {
        if (selectedFlag != null)
            selectedFlag.GetComponent<FlagScript>().select.SetActive(false);
        selectedFlag = flag;
        selectedFlag.GetComponent<FlagScript>().select.SetActive(true);
        flagPanel.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = flag.GetComponent<FlagScript>().name;
        flagPanel.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate(GetComponent<GameState>().teams[flag.GetComponent<FlagScript>().team].name);
        flagPanel.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().color = GetComponent<GameState>().teams[flag.GetComponent<FlagScript>().team].color;
        flagPanel.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate("militarydegree", ((int)(float)(flag.GetComponent<FlagScript>().military * 100)).ToString());
        flagPanel.transform.GetChild(4).GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate("populationgrowth", ((int)(float)(flag.GetComponent<FlagScript>().populationGrowth * 100)).ToString());
        flagPanel.transform.GetChild(5).GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate("taxrate", ((int)(float)(flag.GetComponent<FlagScript>().taxRate * 100)).ToString());
        flagPanel.transform.GetChild(10).GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate("cost", "125", "250");
        flagPanel.transform.GetChild(11).GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate("cost", "125", "75");
        flagPanel.transform.GetChild(12).GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate("cost", "125", "250");

        if (flag.GetComponent<FlagScript>().team == team)
        {
            flagPanel.transform.GetChild(6).gameObject.SetActive(true);
            flagPanel.transform.GetChild(7).gameObject.SetActive(true);
            flagPanel.transform.GetChild(8).gameObject.SetActive(true);
            flagPanel.transform.GetChild(10).gameObject.SetActive(true);
            flagPanel.transform.GetChild(11).gameObject.SetActive(true);
            flagPanel.transform.GetChild(12).gameObject.SetActive(true);
        }
        else
        {
            flagPanel.transform.GetChild(6).gameObject.SetActive(false);
            flagPanel.transform.GetChild(7).gameObject.SetActive(false);
            flagPanel.transform.GetChild(8).gameObject.SetActive(false);
            flagPanel.transform.GetChild(10).gameObject.SetActive(false);
            flagPanel.transform.GetChild(11).gameObject.SetActive(false);
            flagPanel.transform.GetChild(12).gameObject.SetActive(false);
        }

        flagPanel.SetActive(true);
    }

    public void HideFlagPanel()
    {
        selectedFlag.GetComponent<FlagScript>().select.SetActive(false);
        flagPanel.SetActive(false);
        selectedFlag = null;
    }

    public void RaiseTroop()
    {
        selectedFlag.GetComponent<FlagScript>().RaiseTroop();
        ShowFlagPanel(selectedFlag);
    }

    public void GrowPopulation()
    {
        selectedFlag.GetComponent<FlagScript>().GrowPopulation();
        ShowFlagPanel(selectedFlag);
    }

    public void ImproveEconomy()
    {
        selectedFlag.GetComponent<FlagScript>().ImproveEconomy();
        ShowFlagPanel(selectedFlag);
    }

    public void SaveGame(string gameName = "autosave")
    {
        SaveObject saveObj = new SaveObject();
        saveObj.name = gameName;

        saveObj.gameStateSaveObject.teams = GetComponent<GameState>().teams;
        saveObj.gameStateSaveObject.factions = GetComponent<AI_Controller>().factions;
        saveObj.gameStateSaveObject.playerTeam = team;
        saveObj.gameStateSaveObject.minZoom = minZoom;
        saveObj.gameStateSaveObject.maxZoom = maxZoom;
        saveObj.gameStateSaveObject.zoomSpeed = zoomSpeed;
        saveObj.gameStateSaveObject.mapXLength = mapXLength;
        saveObj.gameStateSaveObject.mapYLength = mapYLength;

        saveObj.mapSaveObject.name = GameObject.FindGameObjectWithTag("Map").name;
        saveObj.mapSaveObject.position = GameObject.FindGameObjectWithTag("Map").transform.position;

        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        List<FlagSaveObject> flagObjects = new List<FlagSaveObject>();
        foreach (GameObject flag in flags)
        {
            FlagSaveObject obj = new FlagSaveObject();
            obj.name = flag.name;
            obj.team = flag.GetComponent<FlagScript>().team;
            obj.taxRate = flag.GetComponent<FlagScript>().taxRate;
            obj.populationGrowth = flag.GetComponent<FlagScript>().populationGrowth;
            obj.position = flag.transform.position;
            flagObjects.Add(obj);
        }
        saveObj.flagSaveObject = flagObjects;

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        List<UnitSaveObject> unitObjects = new List<UnitSaveObject>();
        foreach (GameObject unit in units)
        {
            UnitSaveObject obj = new UnitSaveObject();
            obj.action = unit.GetComponent<UnitScript>().action;
            obj.info = unit.GetComponent<UnitScript>().info;
            obj.team = unit.GetComponent<UnitScript>().team;
            obj.position = unit.transform.position;
            unitObjects.Add(obj);
        }
        saveObj.unitSaveObject = unitObjects;

        SaveLog saveLog = new SaveLog();

        if (PlayerPrefs.HasKey("SAVE"))
        {
            string savedJson = PlayerPrefs.GetString("SAVE");
            saveLog = JsonUtility.FromJson<SaveLog>(savedJson);
            for (int i = 0; i < saveLog.saveObjects.Count; i++)
            {
                if (saveLog.saveObjects[i].name == saveObj.name)
                {
                    saveLog.saveObjects.RemoveAt(i);
                    break;
                }
            }
        }

        saveLog.saveObjects.Add(saveObj);
        string json = JsonUtility.ToJson(saveLog);
        PlayerPrefs.SetString("SAVE", json);
        PlayerPrefs.Save();
        ShowAlert(GetComponent<LanguageController>().Translate("gamesaved"));
    }

    public void LoadGame(string gameName = "autosave")
    {
        if (!PlayerPrefs.HasKey("SAVE"))
        {
            ShowAlert(GetComponent<LanguageController>().Translate("nosave"));
            return;
        }
        
        SaveObject saveObject = null;
        string savedJson = PlayerPrefs.GetString("SAVE");
        SaveLog saveLog = JsonUtility.FromJson<SaveLog>(savedJson);

        foreach (SaveObject obj in saveLog.saveObjects)
        {
            if (obj.name == gameName)
            {
                saveObject = obj;
                break;
            }
        }

        if (saveObject != null)
        {
            ClearScene();
            GetComponent<GameState>().teams = saveObject.gameStateSaveObject.teams;
            GetComponent<AI_Controller>().factions = saveObject.gameStateSaveObject.factions;
            team = saveObject.gameStateSaveObject.playerTeam;
            minZoom = saveObject.gameStateSaveObject.minZoom;
            maxZoom = saveObject.gameStateSaveObject.maxZoom;
            zoomSpeed = saveObject.gameStateSaveObject.zoomSpeed;
            mapXLength = saveObject.gameStateSaveObject.mapXLength;
            mapYLength = saveObject.gameStateSaveObject.mapYLength;
            GameObject map = Instantiate(Resources.Load("Maps/" + saveObject.mapSaveObject.name)) as GameObject;
            map.name = saveObject.mapSaveObject.name;
            map.transform.position = saveObject.mapSaveObject.position;
            foreach (FlagSaveObject obj in saveObject.flagSaveObject)
            {
                GameObject flag = Instantiate(GetComponent<GameState>().flagPrefab);
                flag.name = obj.name;
                flag.GetComponent<FlagScript>().team = obj.team;
                flag.GetComponent<FlagScript>().taxRate = obj.taxRate;
                flag.GetComponent<FlagScript>().populationGrowth = obj.populationGrowth;
                flag.transform.position = obj.position;
            }
            foreach (UnitSaveObject obj in saveObject.unitSaveObject)
            {
                GameObject unit = Instantiate(Camera.main.GetComponent<GameState>().unitPrefabs[Camera.main.GetComponent<GameState>().teams[obj.team].unit]);
                unit.GetComponent<UnitScript>().action = obj.action;
                unit.GetComponent<UnitScript>().info = obj.info;
                unit.GetComponent<UnitScript>().team = obj.team;
                unit.transform.position = obj.position;
            }

            SetLoaded(true);
            selectNewScene.SetActive(false);
            gameMenu.SetActive(false);
            StartPlayer();
            Debug.Log("Game loaded successfully.");
        }
    }

    public void ClearScene()
    {
        GetComponent<GameState>().Pause();
        GameObject map = GameObject.FindGameObjectWithTag("Map");
        Destroy(map);
        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        for (int i = 0; i < flags.Length; i++)
        {
            Destroy(flags[i]);
        }
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        for (int i = 0; i < units.Length; i++)
        {
            Destroy(units[i]);
        }
        GetComponent<GameState>().teams.Clear();
        GetComponent<AI_Controller>().factions.Clear();
        pointer.SetActive(false);
        commands.SetActive(false);
        flagPanel.SetActive(false);
        SetLoaded(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void SaveScene(string sceneName, string fileName)
    {
        SaveObject saveObj = new SaveObject();
        saveObj.name = sceneName;

        saveObj.gameStateSaveObject.teams = GetComponent<GameState>().teams;
        saveObj.gameStateSaveObject.factions = GetComponent<AI_Controller>().factions;
        saveObj.gameStateSaveObject.playerTeam = team;
        saveObj.gameStateSaveObject.minZoom = minZoom;
        saveObj.gameStateSaveObject.maxZoom = maxZoom;
        saveObj.gameStateSaveObject.zoomSpeed = zoomSpeed;
        saveObj.gameStateSaveObject.mapXLength = mapXLength;
        saveObj.gameStateSaveObject.mapYLength = mapYLength;

        saveObj.mapSaveObject.name = GameObject.FindGameObjectWithTag("Map").name;
        saveObj.mapSaveObject.position = GameObject.FindGameObjectWithTag("Map").transform.position;

        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        List<FlagSaveObject> flagObjects = new List<FlagSaveObject>();
        foreach (GameObject flag in flags)
        {
            FlagSaveObject obj = new FlagSaveObject();
            obj.name = flag.name;
            obj.team = flag.GetComponent<FlagScript>().team;
            obj.taxRate = flag.GetComponent<FlagScript>().taxRate;
            obj.populationGrowth = flag.GetComponent<FlagScript>().populationGrowth;
            obj.position = flag.transform.position;
            flagObjects.Add(obj);
        }
        saveObj.flagSaveObject = flagObjects;

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        List<UnitSaveObject> unitObjects = new List<UnitSaveObject>();
        foreach (GameObject unit in units)
        {
            UnitSaveObject obj = new UnitSaveObject();
            obj.action = unit.GetComponent<UnitScript>().action;
            obj.info = unit.GetComponent<UnitScript>().info;
            obj.team = unit.GetComponent<UnitScript>().team;
            obj.position = unit.transform.position;
            unitObjects.Add(obj);
        }
        saveObj.unitSaveObject = unitObjects;
        string json = JsonUtility.ToJson(saveObj);

        System.IO.File.WriteAllText("Assets/Resources/Scenes/" + fileName + ".json", json);

        Debug.Log("Scene saved successfully.");
    }

    public void LoadScene(string sceneName)
    {
        SaveObject saveObject = null;
        saveObject = JsonUtility.FromJson<SaveObject>(Resources.Load<TextAsset>("Scenes/" + sceneName).text);

        if (saveObject != null)
        {
            ClearScene();
            GetComponent<GameState>().teams = saveObject.gameStateSaveObject.teams;
            GetComponent<AI_Controller>().factions = saveObject.gameStateSaveObject.factions;
            team = saveObject.gameStateSaveObject.playerTeam;
            minZoom = saveObject.gameStateSaveObject.minZoom;
            maxZoom = saveObject.gameStateSaveObject.maxZoom;
            zoomSpeed = saveObject.gameStateSaveObject.zoomSpeed;
            mapXLength = saveObject.gameStateSaveObject.mapXLength;
            mapYLength = saveObject.gameStateSaveObject.mapYLength;
            GameObject map = Instantiate(Resources.Load("Maps/" + saveObject.mapSaveObject.name)) as GameObject;
            map.name = saveObject.mapSaveObject.name;
            map.transform.position = saveObject.mapSaveObject.position;
            foreach (FlagSaveObject obj in saveObject.flagSaveObject)
            {
                GameObject flag = Instantiate(GetComponent<GameState>().flagPrefab);
                flag.name = obj.name;
                flag.GetComponent<FlagScript>().team = obj.team;
                flag.GetComponent<FlagScript>().taxRate = obj.taxRate;
                flag.GetComponent<FlagScript>().populationGrowth = obj.populationGrowth;
                flag.transform.position = obj.position;
            }
            foreach (UnitSaveObject obj in saveObject.unitSaveObject)
            {
                GameObject unit = Instantiate(Camera.main.GetComponent<GameState>().unitPrefabs[Camera.main.GetComponent<GameState>().teams[obj.team].unit]);
                unit.GetComponent<UnitScript>().action = obj.action;
                unit.GetComponent<UnitScript>().info = obj.info;
                unit.GetComponent<UnitScript>().team = obj.team;
                unit.transform.position = obj.position;
            }
            SetLoaded(true);
            
        }
    }

    public void SetLoaded(bool sceneLoaded)
    {
        if (sceneLoaded)
            GetComponent<GameState>().InitGame();

        loaded = sceneLoaded;
        gameMenu.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().interactable = sceneLoaded;
        gameMenu.transform.GetChild(2).GetComponent<UnityEngine.UI.Button>().interactable = sceneLoaded;
    }

    public void StartPlayer(int playerTeam = -1)
    {
        selectTeam.SetActive(false);
        if (playerTeam >= 0)
            team = playerTeam;

        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        foreach (GameObject flag in flags)
        {
            if (flag.GetComponent<FlagScript>().team == team)
            {
                transform.position = new Vector3(flag.transform.position.x, flag.transform.position.y, -10);
                break;
            }
        }

        teamText.GetComponent<UnityEngine.UI.Text>().text = GetComponent<LanguageController>().Translate(GetComponent<GameState>().teams[team].name);
        teamText.GetComponent<UnityEngine.UI.Text>().color = GetComponent<GameState>().teams[team].color;

        GetComponent<GameState>().Play();
    }

    public void ShowAlert(string text)
    {
        alert.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = text;
        alert.SetActive(true);
    }

    public void CallUnits()
    {
        float value = callTroops.transform.GetChild(2).GetComponent<UnityEngine.UI.Slider>().value;
        bool excludefighting = callTroops.transform.GetChild(3).GetComponent<UnityEngine.UI.Toggle>().isOn;
        bool onlyhighmorale = callTroops.transform.GetChild(4).GetComponent<UnityEngine.UI.Toggle>().isOn;
        List<GameObject> units = new List<GameObject>();

        GameObject[] allunits = GameObject.FindGameObjectsWithTag("Unit");
        
        foreach (GameObject u in allunits)
        {
            if (u.GetComponent<UnitScript>().team == team && !u.GetComponent<UnitScript>().info.retreat)
            {
                if (u.transform.GetChild(0).gameObject.activeSelf && excludefighting)
                    return;

                if (u.GetComponent<UnitScript>().info.moral < 75 && onlyhighmorale)
                    return;

                units.Add(u);
            }
        }

        List<GameObject> activeUnits = new List<GameObject>();
        for (int i = 0; i < Mathf.RoundToInt(units.Count * value); i++)
        {
            activeUnits.Add(units[i]);
        }

        foreach (GameObject u in activeUnits)
        {
            u.GetComponent<UnitScript>().action.location = selectedFlag.transform.position;
            if (selectedFlag.GetComponent<FlagScript>().team == team)
            {
                u.GetComponent<UnitScript>().action.type = ActionType.Defend;
            }
            else
            {
                u.GetComponent<UnitScript>().action.type = ActionType.Attack;
            }
        }

        ShowAlert(GetComponent<LanguageController>().Translate("directedunits", activeUnits.Count.ToString(), selectedFlag.name));
    }

    public void OpenStorePage()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.Safak.NoPeace");
    }

    public void OpenInstagramProfile()
    {
        Application.OpenURL("https://www.instagram.com/safak.akcan/");
    }
}