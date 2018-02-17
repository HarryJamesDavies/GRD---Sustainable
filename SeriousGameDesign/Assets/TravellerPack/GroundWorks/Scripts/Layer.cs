using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public int m_layerIndex = 0;
    public GameObject m_sectionPrefab;
    public List<Section> m_sections = new List<Section>();
    private Texture2D m_layout;

    public float m_layerHeight = 50.0f;
    public float m_sectionLength = 200.0f;
    private const float DefaultBaseScale = 50.0f;
    public float m_baseScale = 1.0f;

    public World m_world = null;

    public void PrimaryInitialise(int _layerIndex, Texture2D _layout, World _world)
    {
        m_layerIndex = _layerIndex;
        m_layout = _layout;
        m_world = _world;
    }

    public void SecondaryInitialise()
    {
        Color[] pixels = new Color[m_layout.width * m_layout.height];
        pixels = m_layout.GetPixels();

        float worldLength = m_sectionLength * m_world.SectionsPer;
        m_baseScale = m_sectionLength / DefaultBaseScale;

        m_sections.Clear();
        for (int z = 0; z < m_world.SectionsPer; z++)
        {
            for (int x = 0; x < m_world.SectionsPer; x++)
            {
                GameObject temp = Instantiate(m_sectionPrefab);
                temp.name = "Section (" + ((x + 1) + (z * m_world.SectionsPer)) + ")";
                temp.transform.SetParent(transform);

                Vector3 parentPos = transform.position;
                temp.transform.position = new Vector3((parentPos.x + (x * m_sectionLength * m_world.ChunksPer)) - (worldLength),
                    parentPos.y + (m_layerIndex * m_layerHeight), (parentPos.z - (z * m_sectionLength * m_world.ChunksPer)) + (worldLength));

                List<bool> flags = new List<bool>();
                for (int sectionZ = 0; sectionZ < m_world.ChunksPer; sectionZ++)
                {
                    for (int sectionX = 0; sectionX < m_world.ChunksPer; sectionX++)
                    {
                        int resultX = sectionX + (x * m_world.ChunksPer);
                        int resultZ = (sectionZ + (z * m_world.ChunksPer)) * (m_world.ChunksPer * m_world.SectionsPer);

                        if (pixels[resultX + resultZ] == new Color(1.0f, 1.0f, 1.0f, 1.0f))
                        {
                            flags.Add(true);
                        }
                        else
                        {
                            flags.Add(false);
                        }
                    }
                }

                m_sections.Add(temp.GetComponent<Section>());
                m_sections[m_sections.Count - 1].PrimaryInitialise(this, flags);
            }
        }


        for (int secondary = 0; secondary < m_sections.Count; secondary++)
        {
            m_sections[secondary].SecondaryInitialise();
        }
    }

    public void TeritaryInitialise()
    {
        for (int secondary = 0; secondary < m_sections.Count; secondary++)
        {
            m_sections[secondary].TertiaryInitialise();
        }
    }

    public List<List<bool>> GetActiveChunks()
    {
        List<List<bool>> results = new List<List<bool>>();

        for(int row = 0; row < (m_world.ChunksPer * m_world.SectionsPer); row++)
        {
            results.Add(new List<bool>());
            for(int column = 0; column < (m_world.ChunksPer * m_world.SectionsPer); column++)
            {
                results[row].Add(true);
            }
        }

        for (int sectionY = 0; sectionY < m_world.SectionsPer; sectionY++)
        {
            for (int sectionX = 0; sectionX < m_world.SectionsPer; sectionX++)
            {
                for (int y = 0; y < m_world.ChunksPer; y++)
                {
                    for (int x = 0; x < m_world.ChunksPer; x++)
                    {
                        results[x + (sectionX * m_world.ChunksPer)][y + (sectionY * m_world.ChunksPer)]
                            = m_sections[sectionX + (sectionY * m_world.SectionsPer)].m_chunksActive[x + (y * m_world.ChunksPer)];
                    }
                }

            }
        }

        return results;
    }

    public Section GetSection(Vector2 _gridPosition)
    {
        int sectionIter = ((int)_gridPosition.x / m_world.ChunksPer) + (((int)_gridPosition.y / m_world.ChunksPer) * m_world.SectionsPer);
        return m_sections[sectionIter];
    }
}
