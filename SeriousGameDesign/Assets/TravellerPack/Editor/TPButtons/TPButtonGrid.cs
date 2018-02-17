using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TPButtonGrid : Editor
{
    public List<List<TPButton>> m_buttons = new List<List<TPButton>>();
    private Vector2 m_buttonSize;
    private Vector2 m_position = new Vector2(0.0f, 0.0f);
    private TPButtonGridResponse m_response;

    public TPButtonGrid Initialise(TPButtonType _type, Vector2 _buttonDimentions, Vector2 _buttonSize, Vector2 _gridPostion,
        bool _startState = false, Texture _positiveTexture = null, Texture _negativeTexture = null, bool _clearOnResponse = false)
    {
        m_buttonSize = _buttonSize;
        m_position = _gridPostion;
        m_response = new TPButtonGridResponse((int)_buttonDimentions.x, _clearOnResponse);

        for (int y = 0; y < (int)_buttonDimentions.y; y++)
        {
            m_buttons.Add(new List<TPButton>());
            for (int x = 0; x < (int)_buttonDimentions.x; x++)
            {
                m_buttons[y].Add(CreateInstance<TPButton>().Initialise(_type, m_buttonSize, _startState, _positiveTexture, _negativeTexture));
                m_response.m_allButtons.Add(new TPButtonResponse(x + (y * (int)_buttonDimentions.x), new Vector2(x, y), _startState));
            }
        }
        return this;
    }

    public TPButtonGrid Initialise(TPButtonType _type, Vector2 _buttonDimentions, float _buttonSize, Vector2 _gridPostion,
        bool _startState = false, Texture _positiveTexture = null, Texture _negativeTexture = null, bool _clearOnResponse = false)
    {
        m_buttonSize = new Vector2(_buttonSize, _buttonSize);
        m_position = _gridPostion;
        m_response = new TPButtonGridResponse((int)_buttonDimentions.x, _clearOnResponse);

        for (int y = 0; y < (int)_buttonDimentions.y; y++)
        {
            m_buttons.Add(new List<TPButton>());
            for (int x = 0; x < (int)_buttonDimentions.x; x++)
            {
                m_buttons[y].Add(CreateInstance<TPButton>().Initialise(_type, m_buttonSize, _startState, _positiveTexture, _negativeTexture));
                m_response.m_allButtons.Add(new TPButtonResponse(x + (y * (int)_buttonDimentions.x), new Vector2(x, y), _startState));
            }
        }
        return this;
    }

    public TPButtonGrid Initialise(TPButtonType _type, Vector2 _buttonDimentions, Vector2 _buttonSize, Vector2 _gridPostion,
        List<List<bool>> _startStates, Texture _positiveTexture = null, Texture _negativeTexture = null, bool _clearOnResponse = false)
    {
        m_buttonSize = _buttonSize;
        m_position = _gridPostion;
        m_response = new TPButtonGridResponse((int)_buttonDimentions.x, _clearOnResponse);

        for (int y = 0; y < (int)_buttonDimentions.y; y++)
        {
            m_buttons.Add(new List<TPButton>());
            for (int x = 0; x < (int)_buttonDimentions.x; x++)
            {
                m_buttons[y].Add(CreateInstance<TPButton>().Initialise(_type, m_buttonSize, _startStates[x][y], _positiveTexture, _negativeTexture));
                m_response.m_allButtons.Add(new TPButtonResponse(x + (y * (int)_buttonDimentions.x), new Vector2(x, y), _startStates[x][y]));
            }
        }
        return this;
    }

    public TPButtonGrid Initialise(TPButtonType _type, Vector2 _buttonDimentions, float _buttonSize, Vector2 _gridPostion, 
        List<List<bool>> _startStates, Texture _positiveTexture = null, Texture _negativeTexture = null, bool _clearOnResponse = false)
    {
        m_buttonSize = new Vector2(_buttonSize, _buttonSize);
        m_position = _gridPostion;
        m_response = new TPButtonGridResponse((int)_buttonDimentions.x, _clearOnResponse);

        for (int y = 0; y < (int)_buttonDimentions.y; y++)
        {
            m_buttons.Add(new List<TPButton>());
            for (int x = 0; x < (int)_buttonDimentions.x; x++)
            {
                m_buttons[y].Add(CreateInstance<TPButton>().Initialise(_type, m_buttonSize, _startStates[x][y], _positiveTexture, _negativeTexture));
                m_response.m_allButtons.Add(new TPButtonResponse(x + (y * (int)_buttonDimentions.x), new Vector2(x, y), _startStates[x][y]));
            }
        }
        return this;
    }

    public TPButtonGridResponse RenderGUI()
    {
        if (m_position.x >= 0.0f && m_position.y >= 0.0f)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(m_position.y);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(m_position.x);

                    GUILayout.BeginVertical("Box", GUILayout.Width(m_buttonSize.x * m_buttons.Count));
                    {
                        for (int y = 0; y < m_buttons.Count; y++)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                for (int x = 0; x < m_buttons[y].Count; x++)
                                {
                                    m_response.SetButtonState(new Vector2(x, y), m_buttons[y][x].RenderGUI());
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        return m_response;
    }

    public void SetButtonSize(Vector2 _buttonSize)
    {
        m_buttonSize = _buttonSize;

        foreach (List<TPButton> group in m_buttons)
        {
            foreach (TPButton button in group)
            {
                button.SetButtonSize(m_buttonSize);
            }
        }
    }

    public void SetButtonSize(float _buttonSize)
    {
        m_buttonSize = new Vector2(_buttonSize, _buttonSize);

        foreach (List<TPButton> group in m_buttons)
        {
            foreach (TPButton button in group)
            {
                button.SetButtonSize(m_buttonSize);
            }
        }
    }

    public void SetPosition(Vector2 _pos)
    {
        m_position = _pos;
    }
}
