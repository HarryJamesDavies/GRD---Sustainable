using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionData
{
    public Player m_player;
    public GameObject m_parent;
    public OptionsBar m_optionsBar;

    public ActionData(Player _player, GameObject _parent, OptionsBar _optionsBar)
    {
        m_player = _player;
        m_parent = _parent;
        m_optionsBar = _optionsBar;
    }
}