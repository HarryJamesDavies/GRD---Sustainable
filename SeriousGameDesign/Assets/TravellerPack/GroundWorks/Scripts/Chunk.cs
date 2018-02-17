using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public bool m_active = true;
    public GameObject m_surfacePrefab;
    public List<GameObject> m_baseWalls = new List<GameObject>();

    public Section m_section = null;
    public Surface m_surface = null;

    public void PrimaryInitialise(Section _section)
    {
        m_section = _section;

        GameObject surface = Instantiate(m_surfacePrefab);
        surface.transform.SetParent(transform);
        surface.transform.localPosition = new Vector3(2.75f, 5.51f, 2.75f); // Stop using magic numbers
        m_surface = surface.GetComponent<Surface>();
        m_surface.gameObject.SetActive(false);
    }

    public void SecondaryInitialise()
    {
        m_surface.gameObject.SetActive(true);
        m_surface.Initialise(this);
    }

    public void SetWall(Direction.Directions _direction, bool _state)
    {
        int index = (int)_direction;
        if(m_baseWalls[index])
        {
            m_baseWalls[index].SetActive(_state);
        }
    }
}
