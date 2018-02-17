using UnityEngine;
using System.Collections.Generic;

public class Pawn : MonoBehaviour
{
    public InputMap m_inputMap = null;
    public InputMapManager.InputType m_inputType = InputMapManager.InputType.NULL;
    public LocalNoticeBoard m_noticeBoard = null;

    public Pawn()
    {

    }

    public Pawn(InputMap _map, int _controllerID)
    {
        SetInputMap(_map, _controllerID);
    }

    public void Start()
    {
        GlobalNoticeBoard.Instance.SubscribeToEvent(GlobalEvent.CO_CONTROLLERADDED, EvFunc_AssignMap);
    }

    void OnDestroy()
    {
        GlobalNoticeBoard.Instance.UnsubscribeToEvent(GlobalEvent.CO_CONTROLLERADDED, EvFunc_AssignMap);
    }

    void EvFunc_AssignMap(object _data = null)
    {
        if (_data != null)
        {
            ControllerAddedEventData data = _data as ControllerAddedEventData;
            m_inputType = data.ControllerType;
            SetInputMap(KeyChain.Instance.m_inputMapManager.GetPresetMap(data.ControllerType, data.ControllerName), data.ControllerIndex);
        }
        else
        {
            Debug.Log("Can't set pawn InputMap with null data");
        }
    }

    public void SetInputMap(InputMap _map, int _controllerID)
    {
        if (_map != null)
        {
            KeyChain.Instance.m_inputMapManager.ResetMap(m_inputMap);
            m_inputMap = ScriptableObject.CreateInstance<InputMap>();
            m_inputMap.Initialise(_map, _controllerID);
            SetNoticeBoard();
        }
        else
        {
            Debug.Log("InputMap null");
        }
    }

    void SetNoticeBoard()
    {
        if(m_noticeBoard)
        {
            Destroy(m_noticeBoard);
        }

        List<string> names = new List<string>();

        m_noticeBoard = gameObject.AddComponent<LocalNoticeBoard>();
        m_inputMap.m_defaultAction.ForEach(delegate (InputMap.Action action)
        {
            names.Add(action.m_name);
        });

        m_inputMap.m_customAction.ForEach(delegate (InputMap.Action action)
        {
            names.Add(action.m_name);
        });

        m_noticeBoard.Initialise(names, false);
    }

    public void ResetInputMap()
    {
        KeyChain.Instance.m_inputMapManager.ResetMap(m_inputMap);
    }

    public void AssignInput(InputMapManager.InputType _type, string _name, int _controllerID)
    {
        m_inputType = _type;
        SetInputMap(KeyChain.Instance.m_inputMapManager.GetPresetMap(_name), _controllerID);

        return;
    }
}
