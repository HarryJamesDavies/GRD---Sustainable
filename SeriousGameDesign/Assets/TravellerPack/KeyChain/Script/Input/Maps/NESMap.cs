using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class NESMap : InputMap
{
    public enum NES
    {
        A, //Button 1
        B, //Button 0
        Start, //Button 9
        Select, //Button 8
        Horizontal, //Axis 1
        Vertical, //Axis 2
        NULL
    }

    [Serializable]
    public class NESButton : Button
    {
        public NES m_NESKey;

        public NESButton(string _input, string _key, NES _NESKey) : base(_input, _key)
        {
            m_NESKey = _NESKey;
        }
    }

    private static string[] m_inputStrings = { "Button 1", "Button 0", "Button 9", "Button 8", "Axis 1", "Axis 2" };

    private static string[] m_NESActions = { "Horizontal", "Axis 1", "Vertical", "Axis 2", "Jump", "Button 1", "Pause", "Button 8", "Menu", "Button 9" };

    public NESMap()
    {

    }

    public void Initialise()
    {
        m_mapType = InputMapManager.InputType.NES;

        m_buttonCount = m_inputStrings.Length;

        m_buttons.Clear();
        for (int iter = 0; iter <= m_buttonCount - 1; iter++)
        {
            NES key = (NES)iter;
            m_buttons.Add(new NESButton(m_inputStrings[iter], key.ToString(), key));
        }

        m_defaultActionCount = m_NESActions.Length / 2;

        m_defaultAction.Clear();
        for (int iter = 0; iter <= (m_defaultActionCount * 2) - 1; iter++)
        {
            m_defaultAction.Add(new Action(m_NESActions[iter], GetButton(m_NESActions[iter + 1])));
            iter++;
        }

        m_customActionCount = 0;
        m_customAction.Clear();

        UpdateKeyNames();
    }

    public void Initialise(MapData _data)
    {
        m_mapType = InputMapManager.InputType.NES;

        m_buttonCount = _data.m_buttonCount;
        m_buttons = _data.m_buttons;

        m_keyNames = _data.m_keyNames;

        m_defaultActionCount = _data.m_defaultActionCount;
        m_defaultAction = _data.m_defaultAction;

        m_customActionCount = _data.m_customActionCount;
        m_customAction = _data.m_customAction;

        UpdateKeyNames();
    }

    new
    public NESButton GetButton(string _input)
    {
        foreach (NESButton button in m_buttons)
        {
            if (button.m_input == _input)
            {
                return button;
            }
        }

        return null;
    }

    public NESButton GetButton(NES _NESKey)
    {
        foreach (NESButton button in m_buttons)
        {
            if (button.m_NESKey == _NESKey)
            {
                return button;
            }
        }

        return null;
    }

    public string GetInput(NES _button)
    {
        foreach (NESButton button in m_buttons)
        {
            if (button.m_NESKey == _button)
            {
                return button.m_input;
            }
        }

        return null;
    }

    public NES GetNESKey(string _action)
    {
        foreach (Action action in m_defaultAction)
        {
            if (action.m_name == _action)
            {
                NESButton button = (NESButton)action.m_button;
                return button.m_NESKey;
            }
        }

        foreach (Action action in m_customAction)
        {
            if (action.m_name == _action)
            {
                NESButton button = (NESButton)action.m_button;
                return button.m_NESKey;
            }
        }

        return NES.NULL;
    }

    public void AddDefaultAction(string _action, string _input, NES _key)
    {
        m_defaultAction.Add(new Action(_action, new NESButton(_input, _key.ToString(), _key)));
    }

    public void AddCustomAction(string _action, string _input, NES _key)
    {
        m_customAction.Add(new Action(_action, new NESButton(_input, _key.ToString(), _key)));
    }
}