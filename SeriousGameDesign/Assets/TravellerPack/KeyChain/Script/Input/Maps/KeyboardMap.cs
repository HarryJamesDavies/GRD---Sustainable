using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class KeyboardMap : InputMap
{
    [Serializable]
    public class KeyboardButton : Button
    {
        public KeyCode m_keyboardKey;

        public KeyboardButton(string _input, string _key, KeyCode _keyboardKey) : base(_input, _key)
        {
            m_keyboardKey = _keyboardKey;
        }
    }

    private static string[] m_KeyboardActions = { "Horizontal", KeyCode.LeftArrow.ToString(), "Vertical", KeyCode.UpArrow.ToString(), "Jump", KeyCode.Return.ToString(),
        "Pause", KeyCode.LeftShift.ToString(), "Menu", KeyCode.Tab.ToString() };

    public KeyboardMap()
    {

    }

    public void Initialise()
    {
        m_mapType = InputMapManager.InputType.KEYBOARD;

        //m_buttonCount = Enum.GetNames(typeof(KeyCode)).Length;
        m_buttonCount = 0;

        m_buttons.Clear();
        for (int iter = 8; iter < 330; iter++)
        {
            if (Enum.IsDefined(typeof(KeyCode), iter))
            {
                m_buttonCount++;
                KeyCode key = (KeyCode)iter;
                m_buttons.Add(new KeyboardButton(key.ToString(), key.ToString(), key));
            }
        }

        m_defaultActionCount = m_KeyboardActions.Length / 2;

        m_defaultAction.Clear();
        for (int iter = 0; iter <= (m_defaultActionCount * 2) - 1; iter++)
        {
            m_defaultAction.Add(new Action(m_KeyboardActions[iter], GetButton(m_KeyboardActions[iter + 1])));
            iter++;
        }

        m_customActionCount = 0;
        m_customAction.Clear();

        UpdateKeyNames();
    }

    public void Initialise(MapData _data)
    {
        m_mapType = InputMapManager.InputType.KEYBOARD;

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
    public KeyboardButton GetButton(string _input)
    {
        foreach (KeyboardButton button in m_buttons)
        {
            if (button.m_input == _input)
            {
                return button;
            }
        }

        return null;
    }

    public KeyboardButton GetButton(KeyCode _PS4Key)
    {
        foreach (KeyboardButton button in m_buttons)
        {
            if (button.m_keyboardKey == _PS4Key)
            {
                return button;
            }
        }

        return null;
    }

    public string GetInput(KeyCode _button)
    {
        foreach (KeyboardButton button in m_buttons)
        {
            if (button.m_keyboardKey == _button)
            {
                return button.m_input;
            }
        }

        return null;
    }

    public KeyCode GetKeyboardKey(string _action)
    {
        foreach (Action action in m_defaultAction)
        {
            if (action.m_name == _action)
            {
                KeyboardButton button = (KeyboardButton)action.m_button;
                return button.m_keyboardKey;
            }
        }

        foreach (Action action in m_customAction)
        {
            if (action.m_name == _action)
            {
                KeyboardButton button = (KeyboardButton)action.m_button;
                return button.m_keyboardKey;
            }
        }

        return KeyCode.None;
    }

    public void AddDefaultAction(string _action, string _input, KeyCode _key)
    {
        m_defaultAction.Add(new Action(_action, new KeyboardButton(_input, _key.ToString(), _key)));
    }

    public void AddCustomAction(string _action, string _input, KeyCode _key)
    {
        m_customAction.Add(new Action(_action, new KeyboardButton(_input, _key.ToString(), _key)));
    }
}
