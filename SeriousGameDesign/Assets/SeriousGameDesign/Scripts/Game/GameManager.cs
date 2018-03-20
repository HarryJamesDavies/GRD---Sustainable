using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Play,
    Result
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

    public List<GameObject> m_remainingRubbish = new List<GameObject>();
    public int m_landFilledRubbish = 0;
    public int m_landFilledRecycling = 0;
    public int m_recycledRubbish = 0;
    public int m_sortedRubbish = 0;

    public Text m_RuLF;
    public Text m_ReLF;
    public Text m_SR;
    public Text m_ReR;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChanger.TransitionScene("MainMenu");
        }

        m_RuLF.text = "Rubbish Land Filled: " + m_landFilledRubbish;
        m_ReLF.text = "Recycling Land Filled: " + m_landFilledRecycling;
        m_SR.text = "Sorted Rubbish: " + m_sortedRubbish;
        m_ReR.text = "Recycling Recycled: " + m_recycledRubbish;
    }

    public void ChangeState(GameState _state)
    {
        switch (_state)
        {
            case GameState.Play:
                {
                    if(m_remainingRubbish.Count == 0)
                    {
                        Debug.Log("You win!");
                    }

                    m_player.m_lockMovement = false;
                    m_player.transform.position = m_playerPlayPoint.position;
                    m_player.transform.rotation = m_playerPlayPoint.rotation;
                    m_resultArea.SetActive(false);
                    m_playArea.SetActive(true);
                    break;
                }
            case GameState.Result:
                {
                    m_player.m_lockMovement = true;
                    m_player.transform.position = m_playerResultPoint.position;
                    m_player.transform.rotation = m_playerResultPoint.rotation;
                    m_playArea.SetActive(false);
                    m_resultArea.SetActive(true);
                    ResultManager.Instance.Begin();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}