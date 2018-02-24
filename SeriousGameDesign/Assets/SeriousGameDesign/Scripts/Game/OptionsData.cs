using System;
using System.Collections.Generic;
using UnityEngine;

public class OptionsData : MonoBehaviour
{
    [Serializable]
    public class Pair
    {
        public string m_action;
        public string m_compatibleAction;
    }
    public List<Pair> m_actions = new List<Pair>();
    private List<string> m_myActions = new List<string>();

    void Awake()
    {
        foreach (Pair actionPair in m_actions)
        {
            m_myActions.Add(actionPair.m_action);
        }
    }

    public List<string> GetActions()
    {
        return m_myActions;        
    }

    public List<string> GetCompatibleActions(string _action)
    {
        List<string> actions = new List<string>();

        foreach (Pair actionPair in m_actions)
        {
            if (actionPair.m_compatibleAction == _action)
            {
                actions.Add(actionPair.m_action);
            }
        }

        return actions;
    }
}
