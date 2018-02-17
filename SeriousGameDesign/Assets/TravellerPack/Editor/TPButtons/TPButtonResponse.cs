using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPButtonResponse
{
    public int m_buttonIndex = -1;
    public Vector2 m_gridPosition = new Vector2(-1, -1);
    public bool m_state = false;

    public TPButtonResponse(int _buttonIndex, Vector2 _gridPosition, bool _state)
    {
        m_buttonIndex = _buttonIndex;
        m_gridPosition = _gridPosition;
        m_state = _state;
    }

    public TPButtonResponse(TPButtonResponse _orignal)
    {
        m_buttonIndex = _orignal.m_buttonIndex;
        m_gridPosition = _orignal.m_gridPosition;
        m_state = _orignal.m_state;
    }
}
