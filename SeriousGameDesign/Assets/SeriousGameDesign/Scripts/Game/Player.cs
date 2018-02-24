using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera m_camera;

    private float m_heightOffset = 2.0f;
    public GameObject m_currentCargo = null;
    private LockObjectToMouse m_lock = null;
    private int m_previousLayer = -1;
    private int m_cargoLayer = 8;

    public float m_maxRayDistance = 200.0f;
    public float m_castDownHeight = 200.0f;
    private bool m_mouseHitSomething = false;
    private RaycastHit m_mouseHit = new RaycastHit();
    private bool m_downHitSomething = false;
    private RaycastHit m_downHit = new RaycastHit();
    public LayerMask m_mask;

    private GameObject m_currentlyHighlightedObject = null;
    private GameObject m_currentlySelectedObject = null;

    public OptionsBar m_optionsBarPrefab;
    private OptionsBar m_currentOptionBar = null;

    public float m_rotationForce = 3.0f;
    public string m_currentAction = "";

    public bool m_disableCargo = false;
    public bool m_disableCargoDrop = false;

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
            }
            else
            {
                m_currentlyHighlightedObject = null;
            }            
        }

        OptionsBarHandle();
        CargoHandle();
    }

    private void OptionsBarHandle()
    {
        if (m_currentlyHighlightedObject && m_currentCargo == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_currentOptionBar)
                {
                    //CloseOptionsBar();
                }

                OpenOptionsBar();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_mouseHitSomething)
                {
                    if (m_currentlyHighlightedObject != m_currentlySelectedObject)
                    {
                        //CloseOptionsBar();
                    }
                }
            }
        }
    }

    private void OpenOptionsBar()
    {
        m_currentlySelectedObject = m_currentlyHighlightedObject;
        OptionsData data = m_currentlySelectedObject.GetComponent<OptionsData>();
        if (data)
        {
            m_currentOptionBar = Instantiate(m_optionsBarPrefab).GetComponent<OptionsBar>();
            m_currentOptionBar.Initialise(this, m_camera, m_currentlySelectedObject.transform, data.GetActions());
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
        if (!m_disableCargo)
        {
            MoveCargo();
            DropCargo();
        }
    }

    public void CarryObject(GameObject _highlightedObject)
    {
        m_currentCargo = _highlightedObject;
        m_lock = m_currentCargo.AddComponent<LockObjectToMouse>().Initialise(m_camera);
        m_previousLayer = m_currentCargo.layer;
        m_currentCargo.layer = m_cargoLayer;
        m_currentAction = "carry";
    }

    private void DropCargo()
    {
        if (m_currentCargo)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_disableCargo = CheckForCompatibleAction();
                if (!m_disableCargo)
                {
                    m_currentCargo.layer = m_previousLayer;
                    m_previousLayer = -1;
                    Destroy(m_currentCargo.GetComponent<LockObjectToMouse>());
                    m_currentCargo = null;
                    m_lock = null;
                }
            }
        }
    }

    private void MoveCargo()
    {
        if (!m_disableCargo)
        {
            if (m_currentCargo && m_downHitSomething)
            {
                Vector3 cargoPos = new Vector3(m_downHit.point.x,
                    m_downHit.point.y + m_heightOffset, m_downHit.point.z);
                m_lock.UpdatePosition(cargoPos);
            }
        }
    }

    private bool CheckForCompatibleAction()
    {
        if(m_mouseHitSomething)
        {
            m_currentlySelectedObject = m_currentlyHighlightedObject;
            OptionsData data = m_currentlySelectedObject.GetComponent<OptionsData>();
            if (data)
            {
                List<string> actions = data.GetCompatibleActions(m_currentAction);
                if (actions.Count > 0)
                {
                    m_currentOptionBar = Instantiate(m_optionsBarPrefab).GetComponent<OptionsBar>();
                    m_currentOptionBar.Initialise(this, m_camera, m_currentlySelectedObject.transform, actions);
                    return true;
                }
            }
            else
            {
                m_currentOptionBar = null;
            }
        }
        return false;
    }
}
