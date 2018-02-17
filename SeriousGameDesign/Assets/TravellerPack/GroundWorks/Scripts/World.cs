using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int SectionsPer = 3;
    public int ChunksPer = 3;

    public GameObject m_layerPrefab;
    public List<Layer> m_worldLayers = new List<Layer>();
    public List<Texture2D> m_activeLayers = new List<Texture2D>();

    private bool m_initialise = false;

    public Layer InitialiseLayer(Texture2D _tex, int _priority)
    {
        if (!m_initialise)
        {
            if (_priority < m_activeLayers.Count)
            {
                m_activeLayers.Insert(_priority, _tex);
            }
            else
            {
                m_activeLayers.Add(_tex);
            }

            Layer temp = Instantiate(m_layerPrefab).GetComponent<Layer>();
            temp.name = "Layer (" + (m_activeLayers.Count) + ")";
            temp.transform.SetParent(transform);
            temp.PrimaryInitialise(m_activeLayers.Count - 1, m_activeLayers[m_activeLayers.Count - 1], this);
            m_worldLayers.Add(temp);
            m_initialise = true;

            return temp;
        }

        return null;
    }

    private void Clear()
    {
        for(int layerIter = 0; layerIter < m_worldLayers.Count; layerIter++)
        {
            Destroy(m_worldLayers[layerIter].gameObject);
        }
    }
}