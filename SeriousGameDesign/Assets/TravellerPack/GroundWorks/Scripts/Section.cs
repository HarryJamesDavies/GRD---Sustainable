using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public GameObject m_chunkPrefab;
    public Layer m_layer;
    public List<Chunk> m_chunks = new List<Chunk>();
    public List<bool> m_chunksActive;

    public void PrimaryInitialise(Layer _layer, List<bool> _chunksActive)
    {
        m_layer = _layer;
        m_chunksActive = _chunksActive;

        for (int sectionZ = 0; sectionZ < m_layer.m_world.ChunksPer; sectionZ++)
        {
            for (int sectionX = 0; sectionX < m_layer.m_world.ChunksPer; sectionX++)
            {
                GameObject temp = Instantiate(m_chunkPrefab);
                temp.name = "Chunk (" + ((sectionX + 1) + (sectionZ * m_layer.m_world.SectionsPer)) + ")";
                temp.transform.SetParent(transform);

                Vector3 parentPos = transform.position;
                temp.transform.position = new Vector3(parentPos.x + (sectionX * m_layer.m_sectionLength),
                    parentPos.y + (m_layer.m_layerIndex * m_layer.m_layerHeight), parentPos.z - (sectionZ * m_layer.m_sectionLength));

                m_chunks.Add(temp.GetComponent<Chunk>());
            }
        }

        for (int flagIter = 0; flagIter < m_chunksActive.Count; flagIter++)
        {
            m_chunks[flagIter].m_active = m_chunksActive[flagIter];
            m_chunks[flagIter].gameObject.SetActive(m_chunksActive[flagIter]);
        }
    }

    public void SecondaryInitialise()
    {
        for (int wallIter = 0; wallIter < m_chunks.Count; wallIter++)
        {
            m_chunks[wallIter].PrimaryInitialise(this);
            m_chunks[wallIter].transform.localScale = new Vector3(m_layer.m_baseScale, 1.0f, m_layer.m_baseScale);

            if (m_chunks[wallIter].m_active)
            {
                m_chunks[wallIter].SetWall(Direction.Directions.North, ChunkExist(wallIter, Direction.Directions.North));
                m_chunks[wallIter].SetWall(Direction.Directions.East, ChunkExist(wallIter, Direction.Directions.East));
                m_chunks[wallIter].SetWall(Direction.Directions.South, ChunkExist(wallIter, Direction.Directions.South));
                m_chunks[wallIter].SetWall(Direction.Directions.West, ChunkExist(wallIter, Direction.Directions.West));
            }
        }
    }

    public void TertiaryInitialise()
    {
        for (int chunkIter = 0; chunkIter < m_chunks.Count; chunkIter++)
        {
            m_chunks[chunkIter].SecondaryInitialise();
        }
    }

    public bool ChunkExist(int _index, Direction.Directions _direction)
    {
        bool result = false;
        switch(_direction)
        {
            case Direction.Directions.North:
                {
                    int nextIndex = _index + 3;
                    result = true;
                    if (nextIndex <= m_chunks.Count - 1)
                    {
                        if (m_chunks[nextIndex].m_active)
                        {
                            result = false;
                        }
                    }
                    break;
                }
            case Direction.Directions.East:
                {
                    int nextIndex = _index - 1;
                    result = true;
                    if (_index % 3 != 0)
                    {
                        if (_index >= 0)
                        {
                            if (m_chunks[nextIndex].m_active)
                            {
                                result = false;
                            }
                        }
                    }
                    break;
                }
            case Direction.Directions.South:
                {
                    int nextIndex = _index - 3;
                    result = true;
                    if (nextIndex >= 0)
                    {
                        if (m_chunks[nextIndex].m_active)
                        {
                            result = false;
                        }
                    }
                    break;
                }
            case Direction.Directions.West:
                {
                    int nextIndex = _index + 1;
                    result = true;
                    if (nextIndex % 3 != 0)
                    {
                        if (nextIndex <= m_chunks.Count - 1)
                        {
                            if (m_chunks[nextIndex].m_active)
                            {
                                result = false;
                            }
                        }
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
        return result;
    }

    public Chunk GetChunk(Vector2 _gridPosition)
    {
        return m_chunks[(int)_gridPosition.x + ((int)_gridPosition.y * m_layer.m_world.ChunksPer)];
    }
}
