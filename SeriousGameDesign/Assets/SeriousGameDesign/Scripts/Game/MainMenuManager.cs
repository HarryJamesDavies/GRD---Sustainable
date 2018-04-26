using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuState
{
    Title,
    Info,
    Play
}

public class MainMenuManager : MonoBehaviour
{
    public MenuState m_state = MenuState.Info;

    public string m_playerName = "";
    public string m_townName = "";

    public GameObject m_titleUI;
    public GameObject m_infoUI;
    public GameObject m_playUI;

    public InputField m_playerNameField;
    public InputField m_townNameField;
    public GameObject m_nextButton;

    public void Awake()
    {
        if(FindObjectOfType<GameData>())
        {
            ChangeState(MenuState.Play);
        }
        else
        {
            ChangeState(MenuState.Title);
        }
    }

    public void LoadNextScene(string _scene)
    {
        SceneChanger.TransitionScene(_scene);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }

        if(m_state == MenuState.Info)
        {
            m_playerName = m_playerNameField.text;
            m_townName = m_townNameField.text;

            if(m_playerName != "" && m_townName != "")
            {
                m_nextButton.SetActive(true);
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }    

    public void ChangeState(MenuState _state)
    {
        m_state = _state;
        m_titleUI.SetActive(false);
        m_infoUI.SetActive(false);
        m_playUI.SetActive(false);
        m_nextButton.SetActive(false);

        switch (m_state)
        {
            case MenuState.Title:
                {
                    m_titleUI.SetActive(true);
                    GameData gameData = FindObjectOfType<GameData>();
                    if(gameData)
                    {
                        Destroy(gameData.gameObject);
                    }
                    break;
                }
            case MenuState.Info:
                {
                    m_infoUI.SetActive(true);
                    break;
                }
            case MenuState.Play:
                {
                    m_playUI.SetActive(true);
                    GameData gameData = new GameObject("GameData").AddComponent<GameData>();
                    gameData.Initialise(m_playerName, m_townName);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void ChangeState(int _state)
    {
        m_state = (MenuState)_state;
        m_titleUI.SetActive(false);
        m_infoUI.SetActive(false);
        m_playUI.SetActive(false);
        m_nextButton.SetActive(false);

        switch (m_state)
        {
            case MenuState.Title:
                {
                    m_titleUI.SetActive(true);
                    GameData gameData = FindObjectOfType<GameData>();
                    if (gameData)
                    {
                        Destroy(gameData.gameObject);
                    }
                    break;
                }
            case MenuState.Info:
                {
                    m_infoUI.SetActive(true);
                    break;
                }
            case MenuState.Play:
                {
                    m_playUI.SetActive(true);
                    GameData gameData = new GameObject("GameData").AddComponent<GameData>();
                    gameData.Initialise(m_playerName, m_townName);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
