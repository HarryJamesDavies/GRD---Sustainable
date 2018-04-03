using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public Vector3 m_speed;

    void Update()
    {
        transform.Rotate(m_speed * Time.deltaTime);
    }
}