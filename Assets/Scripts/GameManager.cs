using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStatus
{
    DEFAULT,
    LOBBY,
    SETTINGS,
    INGAME,
    DIALOGUE,
    TRANSITION, //jika lg dalam animasi anything (ga cuma transition)
    SELECTION,
    PAUSE,
    DEATH
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private float gravityScale;
    [SerializeField] private float defaultPlayerHealth = 100f;
    [SerializeField] private float defaultPlayerSpeed = 100f;
    [SerializeField] private float defaultPlayerStamina = 100f;

    [SerializeField] private GameStatus status;
    [SerializeField] private CinemachineVirtualCamera CinemachineCamera;

    private float currentPlayerHealth;
    private float currentPlayerSpeed;
    private float currentPlayerStamina;

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
        _instance = this;
        DontDestroyOnLoad(gameObject);
        Physics2D.gravity = new Vector2(0, -gravityScale);
    }

    //private void Start()
    //{
    //    DialogueManager.Instance.StartDialogue("D3_1");
    //}

    public GameStatus GetStatus()
    {
        return status;
    }

    public void ChangeStatus(GameStatus newStatus)
    {
        status = newStatus;
    }

    public bool CompareStatus(GameStatus status)
    {
        return this.status == status;
    }

    public void TestFunction()
    {
        Debug.Log("Received called in GameManager");
    }

    public void StartGame()
    {

    }

    public void PauseGame()
    {

    }

    public void ExitToMainMenu()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayerSpawn(float x, float y)
    {   
        GameObject p = Instantiate(player, new Vector3(x, y, 0), gameObject.transform.rotation);
        p.GetComponent<PlayerHealth>().SetDefaultHP(defaultPlayerHealth);
        p.GetComponent<PlayerStamina>().SetStamina(defaultPlayerStamina);
        p.GetComponent<PlayerSpeed>().SetSpeed(defaultPlayerSpeed);
    }

    public void PlayerDeath()
    {
        print("Player dead");
    }

    public void SetInteractableArea(string name)
    {
        InteractableAreaName = name;
    }

    public void InteractItem()
    {
        if (InteractableAreaName == null) return;
    }
}
