using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GroundWorks : MonoBehaviour
{
    public static GroundWorks Instance = null;

    [SerializeField]
    public List<World> m_worlds = new List<World>();

    void Awake()
    {
        if(CheckInstance())
        {
        }
    }

    private bool CheckInstance()
    {
        if (Instance)
        {
            DestroyImmediate(this);
            return false;
        }
        else
        {
            Instance = this;
            return true;
        }
    }

    public void AddWorld(World _world)
    {
        m_worlds.Add(_world);
    }
}
