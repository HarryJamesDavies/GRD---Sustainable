using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsBar : MonoBehaviour
{
    public enum Options
    {
        PickUp
    }

    public RectTransform m_transform;
    public Canvas m_canvas;
    private Camera m_camera;
    private Transform m_parent;
    private Player m_player;
    public Text m_nameText;

    private float m_offset = 6.0f;

    private bool m_initialised = false;

    public Transform m_buttonHolder;
    public GameObject m_triggerButtonPrefab;
    public GameObject m_toggleButtonPrefab;

    private Dictionary<string, Action> m_triggerActions = new Dictionary<string, Action>();
    private Dictionary<string, Action> m_toggleActions = new Dictionary<string, Action>();

    public OptionsBar Initialise(Player _player, Camera _camera, Transform _parent, string _name, List<ActionInfo> _actions)
    {
        m_player = _player;
        m_player.m_currentAction = "Selecting";
        m_camera = _camera;
        m_canvas.worldCamera = m_camera;
        m_parent = _parent;
        m_nameText.text = _name;

        foreach (ActionInfo action in _actions)
        {
            CreateButton(action.m_actionName, action.m_toggle);

            if (action.m_toggle)
            {
                m_toggleActions.Add(action.m_actionName, action.m_action);
            }
            else
            {
                m_triggerActions.Add(action.m_actionName, action.m_action);
            }
        }

        m_transform.SetParent(_parent);
        Vector3 offsetDirection = Vector3.Cross(Vector3.up, (transform.position - m_camera.transform.position).normalized).normalized;
        m_transform.localPosition = offsetDirection * m_offset;
        m_transform.LookAt(m_camera.transform);

        m_initialised = true;

        return this;
    }

    private void CreateButton(string _action, bool _toggle)
    {
        if (_toggle)
        {
            GameObject newButton = Instantiate(m_toggleButtonPrefab, m_buttonHolder);
            newButton.name = _action + " Toogle";
            newButton.GetComponentInChildren<Text>().text = _action;
            Toggle button = newButton.GetComponent<Toggle>();
            button.onValueChanged.AddListener(delegate { HandleTooglePress(_action, button.isOn); });
        }
        else
        {
            GameObject newButton = Instantiate(m_triggerButtonPrefab, m_buttonHolder);
            newButton.name = _action + " Button";
            newButton.GetComponentInChildren<Text>().text = _action;
            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(delegate { HandleButtonPress(_action); });
        }
    }

    void Update()
    {
        if(m_initialised)
        {
            //Vector3 offsetDirection = Vector3.Cross(Vector3.up, (transform.position - m_camera.transform.position).normalized).normalized;    
            Vector3 offsetDirection = Vector3.Cross(Vector3.up, m_camera.transform.forward * 30.0f).normalized;
            m_transform.localPosition = offsetDirection * m_offset;
            Debug.DrawRay(m_parent.transform.position, offsetDirection * 30.0f, Color.blue);
            Debug.DrawRay(m_camera.transform.position, m_camera.transform.forward * 30.0f, Color.red);
            m_transform.LookAt(m_camera.transform);
        }
    }

    public void HandleButtonPress(string _option)
    {
        m_triggerActions[_option].DoAction(new ActionData(m_player, m_parent.gameObject, this));
        Destroy(gameObject);
    }

    public void HandleTooglePress(string _option, bool _state)
    {
        if (!_state)
        {
            m_toggleActions[_option].DoAction(new ActionData(m_player, m_parent.gameObject, this));
        }
        else
        {
            m_toggleActions[_option].Destroy();
        }
    }
}
