using UnityEngine;
using UnityEngine.UI;

public class Inspect : Action
{
    [TextArea]
    public string m_description;
    public GameObject m_inspectPanelPrefab;

    private GameObject m_inspectorPanel;

    public override void DoAction(ActionData _data)
    {
        m_inspectorPanel = Instantiate(m_inspectPanelPrefab, _data.m_optionsBar.transform);
        m_inspectorPanel.GetComponentInChildren<Text>().text = m_description;
        m_inspectorPanel.GetComponent<RectTransform>().localPosition = new Vector2(-100.0f, 0.0f);

        base.DoAction(_data);
    }

    public override void Destroy()
    {
        Destroy(m_inspectorPanel);
        base.Destroy();
    }
}
