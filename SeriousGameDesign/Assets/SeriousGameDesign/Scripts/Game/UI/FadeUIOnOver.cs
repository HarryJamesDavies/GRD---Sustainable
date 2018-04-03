using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FadeUIOnOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool m_isOver = false;
    public bool m_fadeNow = false;
    public bool m_overrideFade = false;

    public float m_fadeLength = 2.0f;

    public float m_elapsedWait = 0.0f;
    public float m_waitLength = 10.0f;

    public GameObject m_infoPanel;
    public Vector2 m_alphaLevels = new Vector2();

    private void Start()
    {
        m_elapsedWait = m_waitLength;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_isOver = true;
        m_infoPanel.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_isOver = false;
        m_elapsedWait = 0.0f;
    }

    private void Update()
    {
        //if (m_fadeNow)
        //{
        //    if (m_overrideFade)
        //    {
        //        FadeUI(m_alphaLevels.y);
        //    }
        //    else
        //    {
        //        if (m_isOver)
        //        {
        //            FadeUI(m_alphaLevels.y);
        //        }
        //        else
        //        {
        //            FadeUI(m_alphaLevels.x);
        //        }
        //    }
        //}

        if (!m_isOver)
        {
            if(m_elapsedWait >= m_waitLength)
            {
                m_infoPanel.SetActive(true);
                //m_overrideFade = true;
            }
            else
            {
                m_elapsedWait += Time.deltaTime;
            }
        }
    }

    //private void FadeUI(float _targetAlpha)
    //{
    //    if (m_infoPanel.canvasRenderer.GetColor().a != _targetAlpha)
    //    {
    //        m_infoPanel.CrossFadeAlpha(_targetAlpha, m_fadeLength, false);
    //    }
    //    else
    //    {
    //        m_fadeNow = false;
    //        m_overrideFade = false;
    //    }
    //}
}
