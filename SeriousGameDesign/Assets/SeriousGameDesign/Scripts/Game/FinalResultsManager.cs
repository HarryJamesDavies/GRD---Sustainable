using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalResultsManager : MonoBehaviour
{
    public static FinalResultsManager Instance = null;
    public ResultData m_data = null;

    public Text m_weightPerPersonText;
    public Text m_landfileldRubbishText;
    public Text m_landfilledRecyclingText;
    public Text m_sortedRubbishText;
    public List<Image> m_weightObjectResultsIcons = new List<Image>();
    public List<Text> m_weightObjectResultsCounts = new List<Text>();
    public Text m_moneySpentText;

    private void Awake()
    {
        CheckInstance();
    }

    private void CheckInstance()
    {
        if(Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    void Start ()
    {
        m_data = FindObjectOfType<ResultData>();

        if (m_data)
        {
            m_weightPerPersonText.text = m_data.m_weightPerPerson.ToString("F2");
            m_landfileldRubbishText.text = "= " + m_data.m_landfileldRubbish.ToString();
            m_landfilledRecyclingText.text = "= " + m_data.m_landfilledRecycling.ToString();
            m_sortedRubbishText.text = "= " + m_data.m_sortedRubbish.ToString();

            for (int objectIter = 0; objectIter < m_data.m_weightObjectResults.Count; objectIter++)
            {
                m_weightObjectResultsIcons[objectIter].transform.parent.gameObject.SetActive(true);
                m_weightObjectResultsIcons[objectIter].sprite = m_data.m_weightObjectResults[objectIter].m_icon;
                m_weightObjectResultsCounts[objectIter].text = m_data.m_weightObjectResults[objectIter].m_count.ToString();
            }

            m_moneySpentText.text = m_data.m_moneySpent.ToString("F2");

            Destroy(m_data.gameObject);
            m_data = null;
        }
    }
	
	public void ReturnToMenu()
    {
        SceneChanger.TransitionScene("MainMenu");
    }
}
