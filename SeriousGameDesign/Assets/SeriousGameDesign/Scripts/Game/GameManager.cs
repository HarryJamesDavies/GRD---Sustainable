using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Play,
    Result,
    Outcome
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState m_state = GameState.Play;

    public Player m_player;
    public GameObject m_playArea;
    public Transform m_playerPlayPoint;

    public GameObject m_resultArea;
    public Transform m_playerResultPoint;

    public GameObject m_outcomeArea;
    public Transform m_playerOutcomePoint;

    public List<GameObject> m_remainingRubbish = new List<GameObject>();

    public List<GameObject> m_landFilledRubbish = new List<GameObject>();
    public List<GameObject> m_landFilledRecycling = new List<GameObject>();
    public List<GameObject> m_sortedRubbish = new List<GameObject>();
    public List<GameObject> m_sortedRecycling = new List<GameObject>();
    public List<GameObject> m_recycledRubbish = new List<GameObject>();

    public Text m_RuLF;
    public Text m_ReLF;
    public Text m_SR;
    public Text m_ReR;

    public float m_startTime = 0.0f;
    public static string CurrentOutFolderPath = "";
    public static string CurrentInFolderPath = "";

    void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if(Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        GameData gameData = FindObjectOfType<GameData>();
        string folderName = "";
        if(!gameData)
        {
            folderName = "DefaultyFace from DefaultyTown";
        }
        else
        {
            folderName = gameData.m_playerName + " from " + gameData.m_townName;
        }
        CurrentOutFolderPath = DataHandler.CreateFolder(folderName, DataHandler.OutDataPath);
        CurrentInFolderPath = DataHandler.CreateFolder(folderName, DataHandler.InDataPath);

        TutorialManager.Instance.BeginPlayTutorial();

        m_startTime = Time.time;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChanger.TransitionScene("MainMenu");
        }

        m_RuLF.text = "Rubbish Land Filled: " + m_landFilledRubbish.Count;
        m_ReLF.text = "Recycling Land Filled: " + m_landFilledRecycling.Count;
        m_SR.text = "Sorted Rubbish: " + m_sortedRubbish.Count;
        m_ReR.text = "Recycling Recycled: " + m_recycledRubbish.Count;
    }

    public void ChangeState(GameState _state)
    {
        switch (_state)
        {
            case GameState.Play:
                {
                    if(m_remainingRubbish.Count == 0)
                    {
                        ChangeState(GameState.Outcome);
                        return;
                    }

                    m_player.m_lockMovement = false;
                    m_player.transform.position = m_playerPlayPoint.position;
                    m_player.transform.rotation = m_playerPlayPoint.rotation;
                    m_resultArea.SetActive(false);
                    m_outcomeArea.SetActive(false);
                    m_playArea.SetActive(true);
                    break;
                }
            case GameState.Result:
                {
                    m_player.m_lockMovement = true;
                    m_player.transform.position = m_playerResultPoint.position;
                    m_player.transform.rotation = m_playerResultPoint.rotation;
                    m_playArea.SetActive(false);
                    m_outcomeArea.SetActive(false);
                    m_resultArea.SetActive(true);
                    TutorialManager.Instance.BeginResultTutorial();
                    ResultManager.Instance.Begin();
                    break;
                }
            case GameState.Outcome:
                {
                    m_player.m_lockMovement = true;
                    m_player.transform.position = m_playerOutcomePoint.position;
                    m_player.transform.rotation = m_playerOutcomePoint.rotation;
                    m_playArea.SetActive(false);
                    m_resultArea.SetActive(false);
                    m_outcomeArea.SetActive(true);
                    TutorialManager.Instance.BeginOutcomeTutorial();
                    OutcomeManager.Instance.Begin();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}