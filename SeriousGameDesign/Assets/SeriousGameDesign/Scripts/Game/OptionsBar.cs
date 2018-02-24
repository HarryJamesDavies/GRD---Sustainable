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

    private float m_offset = 6.0f;

    private bool m_initialised = false;

    public Transform m_buttonHolder;
    public GameObject m_buttonPrefab;

    public OptionsBar Initialise(Player _player, Camera _camera, Transform _parent, List<string> _actions)
    {
        m_player = _player;
        m_player.m_currentAction = "selecting";
        m_camera = _camera;
        m_canvas.worldCamera = m_camera;
        m_parent = _parent;

        foreach(string action in _actions)
        {
            CreateButton(action);
        }

        m_transform.SetParent(_parent);
        Vector3 offsetDirection = Vector3.Cross(Vector3.up, (transform.position - m_camera.transform.position).normalized).normalized;
        m_transform.localPosition = offsetDirection * m_offset;
        m_transform.LookAt(m_camera.transform);

        m_initialised = true;

        return this;
    }

    private void CreateButton(string _action)
    {
        GameObject newButton = Instantiate(m_buttonPrefab, m_buttonHolder);
        newButton.name = _action + " Button";
        newButton.GetComponentInChildren<Text>().text = _action;
        Button button = newButton.GetComponent<Button>();
        button.onClick.AddListener(delegate { HandleButtonPress(_action); });
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
        switch(_option)
        {
            case "carry":
                {
                    m_player.CarryObject(m_parent.gameObject);
                    Destroy(gameObject);
                    break;
                }
            case "bin":
                {
                    if (m_parent)
                    {
                        m_parent.GetComponent<RubbishBin>().BinObject(m_player.m_currentCargo);
                        m_player.m_currentCargo = null;
                        m_player.m_disableCargo = false;
                        Destroy(gameObject);
                    }
                    break;
                }
            case "recycle":
                {
                    if (m_parent)
                    {
                        m_parent.GetComponent<RecyclingBin>().BinObject(m_player.m_currentCargo);
                        m_player.m_currentCargo = null;
                        m_player.m_disableCargo = false;
                        Destroy(gameObject);
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
