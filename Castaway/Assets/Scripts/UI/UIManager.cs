using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button btn_Work;
    public GameObject workPanel;
    public Button btn_Cut;
    public Button btn_Haul;

    public Button btn_Zones;
    public GameObject zonesPanel;
    public Button btn_Stockpile;

    public Button btn_Resources;
    public GameObject resourcesPanel;
    public Text txt_Logs;

    public Button btn_Build;
    public GameObject buildPanel;
    public Button btn_WoodenWall;

    public GameObject characterPanel;
    public Button characterThumbnail;

    public Button currentButton {get; private set;}

    ClickController clickController;

    List<GameObject> panels = new List<GameObject>();
    List<Button> buttons = new List<Button>();
    GameManager manager;

    Coroutine focusCamera;

    void Awake()
    {
        currentButton = null;
        focusCamera = null;
        
        clickController = GetComponent<ClickController>();
        manager = GetComponent<GameManager>();
        workPanel.SetActive(false);
        zonesPanel.SetActive(false);
        resourcesPanel.SetActive(false);
        buildPanel.SetActive(false);

        AddPanelsToList();
        AddButtonsToList();
        AddCharacterThumbnails();

        btn_Work.onClick.AddListener( () => {TogglePanel(workPanel);} );
        btn_Cut.onClick.AddListener( () => {ToggleCut(btn_Cut);} );

        btn_Zones.onClick.AddListener( () => {TogglePanel(zonesPanel);} );
        btn_Stockpile.onClick.AddListener( () => {ToggleStockpilePlacement(btn_Stockpile);} );

        btn_Resources.onClick.AddListener( () => {TogglePanel(resourcesPanel);} );

        btn_Build.onClick.AddListener( () => {TogglePanel(buildPanel);} );
        btn_WoodenWall.onClick.AddListener( () => {ToggleBuild(btn_WoodenWall);} );
    }
    void Update()
    {
        int numberOfLogs = GetComponent<ResourceManager>().GetResourceQuantity(Resource.logs);
        txt_Logs.text = "Logs = " + numberOfLogs;
    }

    void AddPanelsToList()
    {
        panels.Add(workPanel);
        panels.Add(zonesPanel);
        panels.Add(resourcesPanel);
        panels.Add(buildPanel);
    }

    void AddButtonsToList()
    {
        buttons.Add(btn_Cut);
        buttons.Add(btn_Stockpile);
        buttons.Add(btn_Haul);
        buttons.Add(btn_WoodenWall);
    }

    void TogglePanel(GameObject panel)
    {
        foreach(GameObject p in panels)
        {
            if(p != panel)
                HideUIElement(p);
        }
        ToggleUIElement(panel);
    }

    void ResetButtons(Button button)
    {
        foreach(Button b in buttons)
        {
            if(b != button)
                b.GetComponent<Image>().color = Color.white;
        }
    }

    private void ToggleUIElement(GameObject g)
    {
        g.SetActive(!g.activeSelf);
    }

    private void HideUIElement(GameObject g)
    {
        g.SetActive(false);
    }

    private void ToggleCut(Button btn_Cut)
    {
        ResetButtons(btn_Cut);
        if(clickController.currentClickAction != ClickAction.Cut)
        {
            currentButton = btn_Cut;
            Debug.Log("Cutting");
            clickController.SetClickAction(ClickAction.Cut);
            btn_Cut.GetComponent<Image>().color = Color.green;
        }
        else
        {
            currentButton = null;
            Debug.Log("Disabling Cutting");
            clickController.SetClickAction(ClickAction.None);
            btn_Cut.GetComponent<Image>().color = Color.white;
        }
    }

    private void ToggleStockpilePlacement(Button btn_Stockpile)
    {
        ResetButtons(btn_Stockpile);
        if(clickController.currentClickAction != ClickAction.Zone_Stockpile)
        {
            currentButton = btn_Stockpile;
            Debug.Log("Zoning stockpile");
            clickController.SetClickAction(ClickAction.Zone_Stockpile);
            btn_Stockpile.GetComponent<Image>().color = Color.green;
        }
        else
        {
            currentButton = null;
            Debug.Log("Disabling zoning stockpile");
            clickController.SetClickAction(ClickAction.None);
            btn_Stockpile.GetComponent<Image>().color = Color.white;
        }
    }

    private void ToggleBuild(Button btn)
    {
        BuildManager buildManager = GetComponent<BuildManager>();
        ResetButtons(btn);
        if(clickController.currentClickAction != ClickAction.Build)
        {
            currentButton = btn;
            Debug.Log("Building");
            clickController.SetClickAction(ClickAction.Build);
            btn.GetComponent<Image>().color = Color.green;
        }
        else
        {
            currentButton = null;
            Debug.Log("Disabling Building");
            clickController.SetClickAction(ClickAction.None);
            btn.GetComponent<Image>().color = Color.white;
        }
    }

    private void AddCharacterThumbnails()
    {
        foreach(GameObject character in manager.Characters)
        {
            Button thumbnail = Instantiate(characterThumbnail);
            Image image = thumbnail.transform.GetChild(0).gameObject.GetComponent<Image>();
            image.sprite = GameObject.Find(character.name).GetComponent<SpriteRenderer>().sprite;
            thumbnail.transform.SetParent(characterPanel.transform, false);
            thumbnail.onClick.AddListener( () => {FocusCamera(character.name);} );
        }
    }

    private void FocusCamera(string characterName)
    {
        Debug.Log("Focusing camera on: " + characterName);
        GameObject targetCharacter = GameObject.Find(characterName);
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        if(focusCamera != null)
            StopCoroutine(focusCamera);
        focusCamera = StartCoroutine(cameraController.SmoothMoveToTarget(targetCharacter));
    }
}