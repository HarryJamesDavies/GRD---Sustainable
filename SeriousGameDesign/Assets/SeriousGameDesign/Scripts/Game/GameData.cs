using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public string m_playerName = "";
    public string m_townName = "";

    public void Initialise(string _playerName, string _townName)
    {
        DontDestroyOnLoad(gameObject);

        m_playerName = _playerName;
        m_townName = _townName;
    }
}
