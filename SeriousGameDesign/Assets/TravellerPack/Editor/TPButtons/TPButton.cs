using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TPButton : Editor
{
    public bool m_value = false;
    public Vector2 m_buttonSize;

    private Texture m_texCurr;
    private Texture m_texPosi;
    private Texture m_texNega;

    private TPButtonType m_type = TPButtonType.Click;

    public TPButton Initialise(TPButtonType _type, Vector2 _buttonSize, bool _startState = false, Texture _positiveTexture = null, Texture _negativeTexture = null)
    {
        m_type = _type;
        m_value = _startState;
        m_buttonSize = _buttonSize;

        m_texPosi = _positiveTexture;
        m_texNega = _negativeTexture;
        ChangeTextures();

        return this;
    }

    public bool RenderGUI()
    {
        switch (m_type)
        {
            case TPButtonType.Click:
                {
                    m_value = GUILayout.Button(m_texCurr, GUILayout.Width(m_buttonSize.x), GUILayout.Height(m_buttonSize.y));
           
                    break;
                }
            case TPButtonType.Hold:
                {
                    m_value = false;
                    if (GUILayout.RepeatButton(m_texCurr, GUILayout.Width(m_buttonSize.x), GUILayout.Height(m_buttonSize.y)))
                    {
                        m_value = true;
                    }

                    break;
                }
            case TPButtonType.Toggle:
                {
                    if (GUILayout.Button(m_texCurr, GUILayout.Width(m_buttonSize.x), GUILayout.Height(m_buttonSize.y)))
                    {
                        m_value = !m_value;
                        ChangeTextures();
                    }

                    break;
                }
        }
        return m_value;
    }

    private void ChangeTextures()
    {
        if (m_value)
        {
            m_texCurr = m_texPosi;
        }
        else
        {
            m_texCurr = m_texNega;
        }
    }

    public void SetButtonSize(Vector2 _buttonSize)
    {
        m_buttonSize = _buttonSize;
    }
}
