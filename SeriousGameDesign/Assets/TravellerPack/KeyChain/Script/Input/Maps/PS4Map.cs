using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PS4Map : InputMap
{
    public enum PS4
    {
        X, //Button 1
        Square, //Button 0
        Circle, //Button 2
        Triangle, //Button 3
        L1, //Button 4
        L2A, //Axis 4
        L2B, //Button 6
        L3, //Button 10
        R1, //Button 5
        R2A, //Axis 5
        R2B, //Button 7
        R3, //Button 11
        Share, //Button 8
        Options, //Button 9
        PS, //Button 12
        PadPress, //Button 13
        LeftStickX, //Axis 1
        LeftStickY, //Axis 2
        RightStickX, //Axis 3
        RightStickY, //Axis 6
        DPadX, //Axis 7
        DPadY, //Axis 8
        NULL
    }

    [Serializable]
    public class PS4Button : Button
    {
        public PS4 m_PS4Key;

        public PS4Button(string _input, string _key, PS4 _PS4Key) : base(_input, _key)
        {
            m_PS4Key = _PS4Key;
        }
    }

    private static string[] m_inputStrings = { "Button 1", "Button 0", "Button 2", "Button 3", "Button 4", "Axis 4", "Button 6", "Button 10", "Button 5",
        "Axis 5", "Button 7", "Button 11", "Button 8", "Button 9", "Button 12", "Button 13", "Axis 1", "Axis 2", "Axis 3",
        "Axis 6", "Axis 7", "Axis 8" };

    private static string[] m_PS4Actions = { "Horizontal", "Axis 1", "Vertical", "Axis 2", "Jump", "Button 1", "Pause", "Button 9", "Menu", "Button 13" };

    public PS4Map()
    {
        
    }

    public void Initialise()
    {
        m_mapType = InputMapManager.InputType.PS4;

        m_buttonCount = m_inputStrings.Length;

        m_buttons.Clear();
        for (int iter = 0; iter <= m_buttonCount - 1; iter++)
        {
            PS4 key = (PS4)iter;
            m_buttons.Add(new PS4Button(m_inputStrings[iter], key.ToString(), key));
        }

        m_defaultActionCount = m_PS4Actions.Length / 2;

        m_defaultAction.Clear();
        for (int iter = 0; iter <= (m_defaultActionCount * 2) - 1; iter++)
        {
            m_defaultAction.Add(new Action(m_PS4Actions[iter], GetButton(m_PS4Actions[iter + 1])));
            iter++;
        }

        m_customActionCount = 0;
        m_customAction.Clear();

        UpdateKeyNames();
    }

    public void Initialise(MapData _data)
    {
        m_mapType = InputMapManager.InputType.PS4;

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
    public PS4Button GetButton(string _input)
    {
        foreach (PS4Button button in m_buttons)
        {
            if (button.m_input == _input)
            {
                return button;
            }
        }

        return null;
    }

    public PS4Button GetButton(PS4 _PS4Key)
    {
        foreach (PS4Button button in m_buttons)
        {
            if (button.m_PS4Key == _PS4Key)
            {
                return button;
            }
        }

        return null;
    }
    
    public string GetInput(PS4 _button)
    {
        foreach (PS4Button button in m_buttons)
        {
            if (button.m_PS4Key == _button)
            {
                return button.m_input;
            }
        }

        return null;
    }

    public PS4 GetPS4Key(string _action)
    {
        foreach (Action action in m_defaultAction)
        {
            if (action.m_name == _action)
            {
                PS4Button button = (PS4Button)action.m_button;
                return button.m_PS4Key;
            }
        }

        foreach (Action action in m_customAction)
        {
            if (action.m_name == _action)
            {
                PS4Button button = (PS4Button)action.m_button;
                return button.m_PS4Key;
            }
        }

        return PS4.NULL;
    }

    public void AddDefaultAction(string _action, string _input, PS4 _key)
    {
        m_defaultAction.Add(new Action(_action, new PS4Button(_input, _key.ToString(), _key)));
    }

    public void AddCustomAction(string _action, string _input, PS4 _key)
    {
        m_customAction.Add(new Action(_action, new PS4Button(_input, _key.ToString(), _key)));
    }
}
