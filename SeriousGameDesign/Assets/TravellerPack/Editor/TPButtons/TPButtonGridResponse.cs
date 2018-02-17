using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPButtonGridResponse
{
    public List<TPButtonResponse> m_allButtons = new List<TPButtonResponse>();
    public List<TPButtonResponse> m_dirtyButtons = new List<TPButtonResponse>();
    private bool m_clearOnResponse = false;

    public int m_length = 0;

    public TPButtonGridResponse(int _length, bool _clearOnResponse = false)
    {
        m_length = _length;
        m_clearOnResponse = _clearOnResponse;
    }

    public TPButtonResponse GetAllButton(Vector2 _gridPosition)
    {
        return m_allButtons[(int)_gridPosition.x + ((int)_gridPosition.y * m_length)];
    }

    public void SetButtonState(Vector2 _gridPosition, bool _newState)
    {
        if (m_allButtons[(int)_gridPosition.x + ((int)_gridPosition.y * m_length)].m_state != _newState)
        {
            m_allButtons[(int)_gridPosition.x + ((int)_gridPosition.y * m_length)].m_state = _newState;
            m_dirtyButtons.Add(m_allButtons[(int)_gridPosition.x + ((int)_gridPosition.y * m_length)]);
        }
    }

    public TPButtonResponse GetNextButton()
    {
        if (m_dirtyButtons.Count > 0)
        {
            TPButtonResponse result = new TPButtonResponse(m_dirtyButtons[0]);
            if (m_clearOnResponse)
            {
                m_dirtyButtons.Clear();
            }
            else
            {
                m_dirtyButtons.RemoveAt(0);
            }
            return result;
        }
        return null;
    }
}
