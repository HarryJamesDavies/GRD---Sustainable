using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public Camera m_camera;

    public float m_groundClearance = 2.0f;
    private float m_heightOffset = 0.0f;
    private Vector3 m_prevGroundPosition = new Vector3(0.0f, 0.0f, 0.0f);
    public GameObject m_currentCargo = null;
    public LockObjectToMouse m_lock = null;
    private int m_previousLayer = -1;
    private int m_cargoLayer = 8;

    public float m_maxRayDistance = 200.0f;
    public float m_castDownHeight = 100.0f;
    public bool m_mouseHitSomething = false;
    private RaycastHit m_mouseHit = new RaycastHit();
    public bool m_downHitSomething = false;
    private RaycastHit m_downHit = new RaycastHit();
    public LayerMask m_mask;

    public GameObject m_currentlyHighlightedObject = null;
    public GameObject m_currentlySelectedObject = null;

    public OptionsBar m_optionsBarPrefab;
    private OptionsBar m_currentOptionBar = null;

    public float m_rotationForce = 3.0f;
    public string m_currentAction = "";

    public bool m_disableCargo = false;

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 forwardMovement = (transform.forward * Input.GetAxis("Vertical"));
        Vector3 rightMovement = (transform.right * Input.GetAxis("Horizontal"));
        Vector3 upMovement = (transform.up * Input.GetAxis("Up"));
        transform.position += forwardMovement + rightMovement + upMovement;

        if (Input.GetMouseButton(1))
        {
            float horizontalRotation = Input.GetAxis("Mouse X");
            float verticalRotation = -Input.GetAxis("Mouse Y");
            Vector3 finalRotation = transform.rotation.eulerAngles + (new Vector3(verticalRotation, horizontalRotation, 0.0f) * m_rotationForce);
            transform.rotation = Quaternion.Euler(finalRotation);
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    void LateUpdate()
    {
        CheckCasts();

        if(m_disableCargo)
        {
            OptionsBarHandle();
        }
        else
        {
            OptionsBarHandle();
            CargoHandle();
        }
    }

    private void CheckCasts()
    {
        if (m_camera)
        {
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
            m_mouseHitSomething = Physics.Raycast(ray, out m_mouseHit, m_maxRayDistance, m_mask);

            if (m_mouseHitSomething)
            {
                m_currentlyHighlightedObject = m_mouseHit.collider.gameObject;
                Debug.DrawRay(ray.origin, ray.direction * m_mouseHit.distance, Color.yellow);

                Vector3 origin = new Vector3(m_mouseHit.point.x, m_castDownHeight, m_mouseHit.point.z);
                m_downHitSomething = Physics.Raycast(origin, Vector3.down, out m_downHit, m_maxRayDistance, m_mask);

                if(m_downHitSomething)
                {
                    Debug.DrawRay(origin, m_downHit.point - origin, Color.green);
                }
            }
            else
            {
                m_currentlyHighlightedObject = null;
            }
        }
    }

    private void OptionsBarHandle()
    {
        if (m_currentlyHighlightedObject && m_currentCargo == null && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_currentOptionBar)
                {
                    CloseOptionsBar();
                }

                OpenOptionsBar();
            }
        }
        else if (m_currentlyHighlightedObject && m_currentCargo && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_currentOptionBar)
                {
                    CloseOptionsBar();
                    m_disableCargo = false;
                }

                OptionsData data = m_currentlyHighlightedObject.GetComponent<OptionsData>();
                List<ActionInfo> actions = CheckForCompatibleAction(data);
                if (actions.Count != 0)
                {
                    OpenOptionsBar(data.m_name, actions);
                    m_disableCargo = true;
                }
            }
        }
    }

    private void OpenOptionsBar()
    {
        OptionsData data = m_currentlyHighlightedObject.GetComponent<OptionsData>();
        if (data)
        {
            m_currentlySelectedObject = m_currentlyHighlightedObject;
            m_currentOptionBar = Instantiate(m_optionsBarPrefab).GetComponent<OptionsBar>();
            m_currentOptionBar.Initialise(this, m_camera, m_currentlySelectedObject.transform, data.m_name, data.GetCompatibleActions(""));
        }
        else
        {
            m_currentOptionBar = null;
        }
    }

    private void OpenOptionsBar(string _name, List<ActionInfo> _actions)
    {
        if (_actions.Count != 0)
        {
            m_currentOptionBar = Instantiate(m_optionsBarPrefab).GetComponent<OptionsBar>();
            m_currentOptionBar.Initialise(this, m_camera, m_currentlySelectedObject.transform, _name, _actions);
        }
        else
        {
            m_currentOptionBar = null;
        }
    }

    private void CloseOptionsBar()
    {
        Destroy(m_currentOptionBar.gameObject);
        m_currentOptionBar = null;
    }

    private void CargoHandle()
    {
        if (m_currentCargo && !m_disableCargo)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DropCargo();
            }
            else
            {
                MoveCargo();
            }
        }
    }

    public void CarryObject(GameObject _highlightedObject)
    {
        m_currentCargo = _highlightedObject;
        m_lock = m_currentCargo.AddComponent<LockObjectToMouse>().Initialise(m_camera);
        m_previousLayer = m_currentCargo.layer;
        m_currentCargo.layer = m_cargoLayer;
        m_currentAction = "Carry";
        m_prevGroundPosition = _highlightedObject.transform.position;
        m_heightOffset = m_groundClearance + (m_currentCargo.GetComponent<Collider>().bounds.size.y / 2.0f);
    }

    private void DropCargo()
    {
        m_currentCargo.layer = m_previousLayer;
        m_previousLayer = -1;
        Destroy(m_currentCargo.GetComponent<LockObjectToMouse>());
        m_currentCargo = null;
        m_lock = null;
    }

    private void MoveCargo()
    {
        if (!m_disableCargo)
        {
            if (m_currentCargo && m_downHitSomething)
            {
                Vector3 cargoPos = new Vector3(m_downHit.point.x,
                    m_downHit.point.y + m_heightOffset, m_downHit.point.z);
                m_prevGroundPosition = m_downHit.point;
                m_lock.UpdatePosition(cargoPos);
            }
            else if(m_currentCargo)
            {
                Vector3 cargoPos = new Vector3(m_prevGroundPosition.x,
                    m_prevGroundPosition.y + m_heightOffset, m_prevGroundPosition.z);
                m_lock.UpdatePosition(cargoPos);
            }
        }
    }

    private List<ActionInfo> CheckForCompatibleAction(OptionsData _data)
    {
        List<ActionInfo> actions = new List<ActionInfo>();
        if (_data)
        {
            m_currentlySelectedObject = m_currentlyHighlightedObject;
            actions = _data.GetCompatibleActions(m_currentAction);
            return actions;
        }
        m_currentlySelectedObject = null;

        return actions;
    }
}
