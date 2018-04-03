using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActionInfo
{
    public string m_actionName;
    public string m_compatibleAction = "Any";
    public Action m_action;
    public bool m_toggle = false;
    public bool m_requiresOptionBar = false;
}

public class OptionsData : MonoBehaviour
{
    public string m_name;
    public List<ActionInfo> m_actions = new List<ActionInfo>();

    public List<ActionInfo> GetActions()
    {
        return m_actions;        
    }

    public List<ActionInfo> GetCompatibleActions(string _action)
    {
        List<ActionInfo> actions = new List<ActionInfo>();

        foreach (ActionInfo actionPair in m_actions)
        {
            if (actionPair.m_compatibleAction == _action)
            {
                actions.Add(actionPair);
            }
        }

        foreach (ActionInfo actionPair in m_actions)
        {
            if (actionPair.m_compatibleAction == "Any")
            {
                actions.Add(actionPair);
            }
        }

        return actions;
    }
}
