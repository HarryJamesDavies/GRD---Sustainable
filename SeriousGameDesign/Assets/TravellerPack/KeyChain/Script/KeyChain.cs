using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class KeyChain : MonoBehaviour
{
    [SerializeField]
    public static KeyChain Instance;

    [SerializeField]
    public InputMapManager m_inputMapManager = null;
    [SerializeField]
    public ControllerManager m_controllerManager = null;

    void Awake()
    {
        CheckInstance();
    }

    public void CheckInstance()
    {
        if (Instance != this)
        {
            if (!Instance)
            {
                Instance = this;
                Initialise();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void Initialise()
    {
        m_inputMapManager = GetComponent<InputMapManager>();
        if (m_inputMapManager == null)
        {
            m_inputMapManager = gameObject.AddComponent<InputMapManager>();
        }
        m_inputMapManager.Initialise();

        m_controllerManager = GetComponent<ControllerManager>();
        if (m_controllerManager == null)
        {
            m_controllerManager = gameObject.AddComponent<ControllerManager>();
        }
        m_controllerManager.Initialise();

        //Get preset data
        InputSettingsData data = Resources.Load("InputSettings/KeyChainSettings") as InputSettingsData;

        if (data != null)
        {
            ControllerManager.m_maxNumInputs = data.m_maxNumInputs;
            ControllerManager.m_enableKeyboard = data.m_enableKeyboard;
            ControllerManager.m_enableNES = data.m_enableNES;
            ControllerManager.m_enablePS4 = data.m_enablePS4;
        }
        else
        {
            Debug.Log("No Saved Settings");
        }
    }

    void Start()
    {
        m_controllerManager.Begin();
    }

    void Update()
    {
        m_controllerManager.Tick();
    }

    void LateUpdate()
    {
        m_controllerManager.LateTick();
    }    
}
