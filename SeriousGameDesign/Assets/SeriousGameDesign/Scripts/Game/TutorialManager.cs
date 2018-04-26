using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TutorialElement
{
    public GameObject m_element;
    public bool m_isStopper = false;
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance = null;

    public delegate void UIUpdate();
    public UIUpdate m_updateOnInputEvent;

    private int m_currentPlayTutorialIndex = 0;
    public List<TutorialElement> m_playTutorialUI;
    private bool m_playTutorialShown;

    private int m_currentResultTutorialIndex = 0;
    public List<TutorialElement> m_resultTutorialUI;
    private bool m_resultsTutorialShown;

    private int m_currentOutcomesTutorialIndex = 0;
    public List<TutorialElement> m_outcomesTutorialUI;
    private bool m_outcomesTutorialShown;

    private void Awake()
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

    private void Update()
    {
        if (Input.anyKeyDown && m_updateOnInputEvent != null)
        {
            m_updateOnInputEvent();
        }
    }

    public void BeginPlayTutorial()
    {
        if (!m_playTutorialShown)
        {
            m_playTutorialShown = true;
            GameManager.Instance.m_player.m_lockMovement = true;
            GameManager.Instance.m_player.m_lockMenus = true;
            m_updateOnInputEvent += UpdatePlayTutorial;
            m_currentPlayTutorialIndex = 0;
            m_currentPlayTutorialIndex = HandleElements(m_playTutorialUI, m_currentPlayTutorialIndex);
        }
    }

    private void UpdatePlayTutorial()
    {
        if (m_currentPlayTutorialIndex < m_playTutorialUI.Count)
        {
            m_currentPlayTutorialIndex = HandleElements(m_playTutorialUI, m_currentPlayTutorialIndex);
        }
        else
        {
            EndPlayTutorial();
        }
    }

    private void EndPlayTutorial()
    {
        m_currentPlayTutorialIndex = 0;
        m_updateOnInputEvent -= UpdatePlayTutorial;
        GameManager.Instance.m_player.m_lockMovement = false;
        GameManager.Instance.m_player.m_lockMenus = false;
    }

    public void BeginResultTutorial()
    {
        if (!m_resultsTutorialShown)
        {
            m_resultsTutorialShown = true;
            GameManager.Instance.m_player.m_lockMovement = true;
            GameManager.Instance.m_player.m_lockMenus = true;
            m_updateOnInputEvent += UpdateResultTutorial;
            m_currentResultTutorialIndex = 0;
            m_currentResultTutorialIndex = HandleElements(m_resultTutorialUI, m_currentResultTutorialIndex);
        }
    }

    private void UpdateResultTutorial()
    {
        if (m_currentResultTutorialIndex < m_resultTutorialUI.Count)
        {
            m_currentResultTutorialIndex = HandleElements(m_resultTutorialUI, m_currentResultTutorialIndex);
        }
        else
        {
            EndResultTutorial();
        }
    }

    private void EndResultTutorial()
    {
        m_currentResultTutorialIndex = 0;
        m_updateOnInputEvent -= UpdateResultTutorial;
        GameManager.Instance.m_player.m_lockMovement = false;
        GameManager.Instance.m_player.m_lockMenus = false;
    }

    public void BeginOutcomeTutorial()
    {
        if (!m_outcomesTutorialShown)
        {
            m_outcomesTutorialShown = true;
            GameManager.Instance.m_player.m_lockMovement = true;
            GameManager.Instance.m_player.m_lockMenus = true;
            m_updateOnInputEvent += UpdateOutcomeTutorial;
            m_currentOutcomesTutorialIndex = 0;
            m_currentOutcomesTutorialIndex = HandleElements(m_outcomesTutorialUI, m_currentOutcomesTutorialIndex);
        }
    }

    private void UpdateOutcomeTutorial()
    {
        if (m_currentOutcomesTutorialIndex < m_outcomesTutorialUI.Count)
        {
            m_currentOutcomesTutorialIndex = HandleElements(m_outcomesTutorialUI, m_currentOutcomesTutorialIndex);
        }
        else
        {
            EndOutcomeTutorial();
        }
    }

    private void EndOutcomeTutorial()
    {
        m_currentOutcomesTutorialIndex = 0;
        m_updateOnInputEvent -= UpdateOutcomeTutorial;
        GameManager.Instance.m_player.m_lockMovement = false;
        GameManager.Instance.m_player.m_lockMenus = false;
    }

    private int HandleElements(List<TutorialElement> _elements, int _startingIndex)
    {
        int index = 0;
        for(index = _startingIndex; _startingIndex < _elements.Count; index++)
        {
            _elements[index].m_element.SetActive(!_elements[index].m_element.activeSelf);
            if(_elements[index].m_isStopper)
            {
                break;
            }
        }
        return index + 1;
    }
}
