using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using TMPro;


#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(GameManager), true), CanEditMultipleObjects]
public class GameManagerEditor : Editor
{
    // this are serialized variables in YourClass
    SerializedProperty skipIntro;
    SerializedProperty useCustomSpawnLocation;
    SerializedProperty customSpawnLocation;


    private void OnEnable()
    {
        skipIntro = serializedObject.FindProperty("skipIntro");
        useCustomSpawnLocation = serializedObject.FindProperty("useCustomSpawnLocation");
        customSpawnLocation = serializedObject.FindProperty("customSpawnLocation");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(skipIntro);

        if (skipIntro.boolValue)
        {
            EditorGUILayout.PropertyField(useCustomSpawnLocation);
        }

        if (useCustomSpawnLocation.boolValue)
        {
            EditorGUILayout.PropertyField(customSpawnLocation);
        }

        // must be on the end.
        serializedObject.ApplyModifiedProperties();

        // add this to render base
        base.OnInspectorGUI();

    }
}

#endif

public enum GameStatus
{
    DEFAULT,
    IMMORTAL,
    LOBBY,
    STARTING,
    RETRYING,
    SETTINGS,
    INTRO,
    INGAME,
    DIALOGUE,
    TRANSITION, //jika lg dalam animasi anything (ga cuma transition)
    SELECTION,
    PAUSE,
    DEATH,
    ENDING
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private float gravityScale;
    [SerializeField] private float defaultPlayerHealth = 100f;
    [SerializeField] private float defaultPlayerSpeed = 100f;
    [SerializeField] private float defaultPlayerStamina = 100f;
    [SerializeField] private int defaultPlayerLives = 3;
    [SerializeField] private static GameStatus status;
    [SerializeField] private Vector2 startSpawnLocation = new Vector2(0, 0);
    [SerializeField] private Vector2 respawnLocation = new Vector2(0, 0);
    [SerializeField] private bool testMode = false;
    [SerializeField] private string gameVersion;
    [SerializeField, HideInInspector] private bool skipIntro;
    [SerializeField, HideInInspector] private bool useCustomSpawnLocation;
    [SerializeField, HideInInspector] private Vector2 customSpawnLocation = new Vector2(0, 0);

    private static CinemachineVirtualCamera CinemachineCamera;
    private static CinemachineConfiner2D CinemachineConfiner;
    private static GameObject spawnedPlayer;
    private static Vector2 spawnLocation = new Vector2(0, 0);
    private static bool IsPaused = false;
    private static int currentLives;
    private Vector2 cameraReposition = new Vector2(0, 0);
    private static bool actualGameplayStarted;

    private static float currentPlayerHealth;
    private static float currentPlayerSpeed;
    private static float currentPlayerStamina;
    private static float currentStaminaDepletionRate = 1f;

    private string InteractableAreaName;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is Null");
            }

            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += SceneChanged;
    }

    private void Start()
    {
        if (testMode)
        {
            currentLives = defaultPlayerLives;
            spawnLocation = new Vector2(0, 0);
            Physics2D.gravity = new Vector2(0, -gravityScale);
        }
        else
        {
            AudioController.Instance.PlayBGM("Main");
        }

        print(status);
    }

    public GameStatus GetStatus()
    {
        return status;
    }

    public void ChangeStatus(GameStatus newStatus)
    {
        status = newStatus;
    }

    public bool CompareStatus(GameStatus comparingStatus)
    {
        return status == comparingStatus;
    }

    public void TestFunction()
    {
        Debug.Log("Received called in GameManager");
    }

    public void StartGame()
    {
        if (testMode)
        {
            Debug.LogWarning("Game manager is set in test mode, disable test mode to start real game");
            return;
        }

        Physics2D.gravity = new Vector2(0, -gravityScale);

        if (skipIntro)
        {
            RetryGame();
            return;
        }

        status = GameStatus.STARTING;
        Transition.Instance.SwitchScene("Level");
    }

    public void RetryGame()
    {
        status = GameStatus.RETRYING;

        if (useCustomSpawnLocation) spawnLocation = customSpawnLocation;
        else spawnLocation = respawnLocation;

        SceneManager.LoadScene("Level");
    }

    public void PauseGame()
    {
        if (!IsPaused)
        {
            ChangeStatus(GameStatus.PAUSE);
        }
        else
        {
            ChangeStatus(GameStatus.DEFAULT);
        }
    }

    public bool GetGameplayStatus()
    {
        return actualGameplayStarted;
    }

    public void ExitToMainMenu()
    {
        status = GameStatus.LOBBY;
        actualGameplayStarted = false;

        Time.timeScale = 1f;
        IsPaused = false;
        AudioController.Instance.SetLowpass();

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartActualGameplay()
    {
        actualGameplayStarted = true;

        if (status == GameStatus.RETRYING)
        {
            SwitchLevelBackground(1);
            PlayerSpawn();
            NPCController.Instance.HideNPC("Serafin");
        }

        status = GameStatus.DEFAULT;
    }

    public void PlayerSpawn()
    {
        if (useCustomSpawnLocation) spawnLocation = customSpawnLocation;

        print("SpawnPlayer");
        spawnedPlayer = Instantiate(player, new Vector3(spawnLocation.x, spawnLocation.y, 0), gameObject.transform.rotation);
        spawnedPlayer.GetComponent<PlayerHealth>().SetDefaultHP(defaultPlayerHealth);
        spawnedPlayer.GetComponent<PlayerHealth>().ResetHealth();
        spawnedPlayer.GetComponent<PlayerStamina>().SetDefaultStamina(defaultPlayerStamina);
        spawnedPlayer.GetComponent<PlayerStamina>().SetStamina(currentPlayerStamina);
        spawnedPlayer.GetComponent<PlayerStamina>().SetDepletionRate(currentStaminaDepletionRate);
        spawnedPlayer.GetComponent<PlayerSpeed>().SetSpeed(defaultPlayerSpeed);
        CinemachineCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        CinemachineCamera.Follow = spawnedPlayer.transform;
    }

    public void PlayerDeath()
    {
        actualGameplayStarted = false;

        CinemachineCamera.Follow = null;
        print("Player dead");
        Destroy(spawnedPlayer);
        spawnedPlayer = null;
        InteractableManager.Instance.TriggerGameOver();
    }

    public void KillPlayer()
    {
        if (!actualGameplayStarted || status != GameStatus.DEFAULT) return;

        spawnedPlayer.GetComponent<PlayerHealth>().KillPlayer();
    }

    public void RespawnPlayer()
    {
        print(currentLives);
        currentPlayerStamina = spawnedPlayer.GetComponent<PlayerStamina>().GetStamina();
        var oldPlayer = spawnedPlayer;
        status = GameStatus.DEFAULT;
        PlayerSpawn();
        Destroy(oldPlayer);
    }

    public void MovePlayer()
    {
        spawnedPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10f);
    }

    public void TeleportPlayer(float x, float y)
    {
        spawnedPlayer.transform.position = new Vector2(x, y);
    }

    public void CameraStopFollow()
    {
        CinemachineCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        CinemachineCamera.Follow = null;
    }

    public void CameraStartFollow(GameObject target)
    {
        CinemachineCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        CinemachineCamera.Follow = target.transform;
    }

    public void CameraStartFollowPlayer()
    {
        CinemachineCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        CinemachineCamera.Follow = spawnedPlayer.transform;
    }

    public void SetInteractableArea(string name)
    {
        InteractableAreaName = name;
    }

    public void InteractItem()
    {
        if (InteractableAreaName == null) return;
    }

    public int GetLives()
    {
        return currentLives;
    }

    public float GetHealth()
    {
        return spawnedPlayer.GetComponent<PlayerHealth>().GetHealth();
    }

    public float GetMaxHealth()
    {
        return spawnedPlayer.GetComponent<PlayerHealth>().GetMaxHealth();
    }

    public float GetStamina()
    {
        return spawnedPlayer.GetComponent<PlayerStamina>().GetStamina();
    }

    public float GetMaxStamina()
    {
        return spawnedPlayer.GetComponent<PlayerStamina>().GetMaxStamina();
    }

    public void TakeLive(int lives = 1)
    {
        Debug.LogFormat("Lives taken: {0}", lives);
        currentLives -= lives;
    }

    internal void TakeDamage(int hp)
    {
        if (!actualGameplayStarted || status != GameStatus.DEFAULT) return;

        spawnedPlayer.GetComponent<PlayerHealth>().TakeDamage(hp);
    }

    public void SetSpawnPoint(float x, float y)
    {
        spawnLocation = new Vector2(x, y);
    }

    public void SwitchLevelBackground(int level)
    {
        CinemachineCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        CinemachineConfiner = GameObject.Find("Virtual Camera").GetComponent<CinemachineConfiner2D>();

        GameObject bounds;
        foreach (Transform child in GameObject.Find("Bounds").transform)
        {
            child.gameObject.SetActive(false);
        }

        switch (level)
        {
            case 0:
                BackgroundController.Instance.SwitchBackground("Default");
                bounds = CameraBoundsController.Instance.SwitchBoundaries("Bound0");
                bounds.gameObject.SetActive(true);
                CinemachineConfiner.m_BoundingShape2D = bounds.GetComponent<Collider2D>();
                AudioController.Instance.PlayBGM("Town");
                break;

            case 1:
                BackgroundController.Instance.SwitchBackground("Day");
                bounds = CameraBoundsController.Instance.SwitchBoundaries("Bound1");
                bounds.gameObject.SetActive(true);
                CinemachineConfiner.m_BoundingShape2D = bounds.GetComponent<Collider2D>();
                SetSpawnPoint(18.5f, -1f);
                AudioController.Instance.PlayBGM("Grassland");
                break;

            case 2:
                BackgroundController.Instance.SwitchBackground("Cave");
                bounds = CameraBoundsController.Instance.SwitchBoundaries("Bound2");
                bounds.gameObject.SetActive(true);
                CinemachineConfiner.m_BoundingShape2D = bounds.GetComponent<Collider2D>();

                SetSpawnPoint(336.5f, 85f);
                currentStaminaDepletionRate = 1.35f;
                AudioController.Instance.PlayBGM("Rockland");
                break;

            case 3:
                BackgroundController.Instance.SwitchBackground("Snow");
                bounds = CameraBoundsController.Instance.SwitchBoundaries("Bound3");
                bounds.gameObject.SetActive(true);
                CinemachineConfiner.m_BoundingShape2D = bounds.GetComponent<Collider2D>();
                SetSpawnPoint(314.5f, 227f);
                currentStaminaDepletionRate = 1.75f;
                AudioController.Instance.PlayBGM("Snow");
                break;

            default:
                Debug.LogWarning("Switch background option not valid");
                return;
        }
    }

    public void TeleportToStartingPoint()
    {
        TeleportPlayer(18.5f, -1f);
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            IsPaused = false;

            status = GameStatus.DEFAULT;
            Time.timeScale = 1f;
        }
        else
        {
            IsPaused = true;

            status = GameStatus.PAUSE;
            Time.timeScale = 0f;
        }

        PostProcessVolume ppvol = Camera.main.gameObject.GetComponent<PostProcessVolume>();

        AudioController.Instance.SetLowpass();
        Cursor.visible = IsPaused;
        ppvol.enabled = IsPaused;
    }

    public void TriggerPreCredits()
    {
        actualGameplayStarted = false;

        NPCController.Instance.MoveToCreditScene("Serafin");
        spawnedPlayer.GetComponent<SpriteRenderer>().flipX = true;
        DialogueManager.Instance.ContinueDialogue();
        CinemachineCamera.Follow = GameObject.Find("PreCreditTarget").transform;
    }

    public void TriggerCredits()
    {
        Color color = spawnedPlayer.GetComponent<SpriteRenderer>().color;
        Destroy(spawnedPlayer);
        NPCController.Instance.ToggleNPC("Serafin", false);

        BackgroundController.Instance.SwitchBackground("Evening");
        CinemachineCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        CinemachineCamera.Follow = GameObject.Find("CreditTarget").transform;
        ChangeStatus(GameStatus.ENDING);
        AudioController.Instance.PlayBGM("Credit");
    }

    private void SceneChanged(Scene current, Scene next)
    {
        switch (next.name)
        {
            case "Level":
                Cursor.visible = false;
                print(status);
                if (status != GameStatus.STARTING && status != GameStatus.RETRYING) return;

                SwitchLevelBackground(0);
                //AudioController.Instance.PlayBGM("Town");

                currentLives = defaultPlayerLives;
                currentPlayerStamina = defaultPlayerStamina;
                currentStaminaDepletionRate = 1f;

                if (status == GameStatus.STARTING)
                {
                    spawnLocation = startSpawnLocation;
                    ChangeStatus(GameStatus.INTRO);
                    DialogueManager.Instance.StartDialogue("D1");
                }
                else if (status == GameStatus.RETRYING)
                {
                    SetSpawnPoint(18.5f, -1f);
                    StartActualGameplay();
                    return;
                }

                PlayerSpawn();

                break;

            case "MainMenu":
                Cursor.visible = true;
                ChangeStatus(GameStatus.LOBBY);
                AudioController.Instance.PlayBGM("Main");
                if (GameObject.Find("Canvas/TxtVersion"))
                {
                    GameObject.Find("Canvas/TxtVersion").gameObject.GetComponent<TMP_Text>().text = "V" + gameVersion;
                }
                break;

            case "GameOver":
                Cursor.visible = true;
                break;
        }
    }
}
