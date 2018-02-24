using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockObjectToMouse : MonoBehaviour
{
    private Camera m_camera;
    private Rigidbody m_rigidBody;
    private float m_distanceFromCamera = 20.0f;    
    private bool m_frozen = false;
    
	public LockObjectToMouse Initialise (Camera _camera)
    {
        m_camera = _camera;
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.useGravity = false;
        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
        transform.position = ray.origin + (ray.direction * 20.0f);
        return this;
    }

    void OnDestroy()
    {
        m_rigidBody.useGravity = true;
    }

    public void UpdatePosition(Vector3 _position)
    {
        if (m_camera)
        {
            transform.position = _position;
        }
    }
}
