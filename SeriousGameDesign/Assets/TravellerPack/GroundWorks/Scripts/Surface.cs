using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    public bool m_addBorder = true;
    private int m_dimension = 10;

    public float m_width = 10.0f;
    private float m_cellWidth = 1.0f;

    public float m_maxHeight = 10.0f;

    public Texture2D m_heightMap;

    private Mesh m_mesh;
    private List<Vector3> m_vertices = new List<Vector3>();
    private List<int> m_indicies = new List<int>();

    public Chunk m_chunk = null;

    public void Initialise(Chunk _chunk)
    {
        if (m_heightMap)
        {
            m_chunk = _chunk;
            m_maxHeight = m_chunk.m_section.m_layer.m_layerHeight;
            InitialiseMesh();
        }
    }

    public void Initialise()
    {
        if (m_heightMap)
        {
            InitialiseMesh();
        }
    }

    private void InitialiseMesh()
    {
        //Set Mesh
        m_mesh = new Mesh();
        m_mesh.name = "Surface";
        m_dimension = m_heightMap.width;

        //Set Vertex X,Z
        m_cellWidth = m_width / m_dimension;
        Vector3 origin = transform.position;
        Vector3 offset = new Vector3((m_dimension / 2.0f) * m_cellWidth, 0.0f, (m_dimension / 2.0f) * m_cellWidth);

        for (int z = 0; z < m_dimension; z++)
        {
            for (int x = 0; x < m_dimension; x++)
            {
                Vector3 pos = new Vector3(/*origin.x + */(x * m_cellWidth), /*origin.y*/0.0f, /*origin.z + */(z * m_cellWidth));
                pos -= offset;
                m_vertices.Add(pos);
            }
        }

        //Set Vertex Y
        Color[] pixels = new Color[m_heightMap.width * m_heightMap.height];
        pixels = m_heightMap.GetPixels();

        for (int vertex = 0; vertex < m_vertices.Count; vertex++)
        {
            m_vertices[vertex] = new Vector3(m_vertices[vertex].x, pixels[vertex].b * m_maxHeight, m_vertices[vertex].z);
        }

        //Set Indicies
        for (int z = 0; z < m_dimension - 1; z++)
        {
            for (int x = 0; x < m_dimension - 1; x++)
            {
                m_indicies.Add(x + (z * m_dimension));
                m_indicies.Add(x + ((z + 1) * m_dimension));
                m_indicies.Add((x + 1) + (z * m_dimension));

                m_indicies.Add((x + 1) + (z * m_dimension));
                m_indicies.Add(x + ((z + 1) * m_dimension));
                m_indicies.Add((x + 1) + ((z + 1) * m_dimension));
            }
        }

        if (m_addBorder)
        {
            AddBorder(origin);
        }

        //Initialise Mesh
        m_mesh.SetVertices(m_vertices);
        m_mesh.SetTriangles(m_indicies, 0);
        m_mesh.RecalculateNormals();
        m_mesh.RecalculateTangents();

        GetComponent<MeshFilter>().mesh = m_mesh;
        gameObject.AddComponent<MeshCollider>();
    }

    private void AddBorder(Vector3 _origin)
    {
        int coreVertexCount = m_vertices.Count;
        List<Vector3> borderVertices = new List<Vector3>();
        List<int> borderIndicies = new List<int>();

        //Add border Vertices
        for (int bottomIter = 0; bottomIter < m_dimension; bottomIter++)
        {
            borderVertices.Add(new Vector3(m_vertices[bottomIter].x, /*_origin.y*/0.0f, m_vertices[bottomIter].z));
        }

        for (int middleIter = m_dimension; middleIter < m_vertices.Count - m_dimension; middleIter++)
        {
            borderVertices.Add(new Vector3(m_vertices[middleIter].x, /*_origin.y*/0.0f, m_vertices[middleIter].z));
            middleIter += (m_dimension - 1);
            borderVertices.Add(new Vector3(m_vertices[middleIter].x, /*_origin.y*/0.0f, m_vertices[middleIter].z));
        }

        for (int topIter = m_vertices.Count - m_dimension; topIter < m_vertices.Count; topIter++)
        {
            borderVertices.Add(new Vector3(m_vertices[topIter].x, /*_origin.y*/0.0f, m_vertices[topIter].z));
        }

        m_vertices.AddRange(borderVertices);

        //Add bottom Indicies
        for (int bottomIter = coreVertexCount; bottomIter < (coreVertexCount + m_dimension) - 1; bottomIter++)
        {
            borderIndicies.Add(bottomIter);
            borderIndicies.Add(bottomIter - coreVertexCount);
            borderIndicies.Add(bottomIter + 1);

            borderIndicies.Add(bottomIter + 1);
            borderIndicies.Add(bottomIter - coreVertexCount);
            borderIndicies.Add((bottomIter - coreVertexCount) + 1);
        }

        //Add bottom Left Indicies
        borderIndicies.Add(coreVertexCount);
        borderIndicies.Add(coreVertexCount + m_dimension);
        borderIndicies.Add(0);

        borderIndicies.Add(coreVertexCount - coreVertexCount);
        borderIndicies.Add(coreVertexCount + m_dimension);
        borderIndicies.Add(m_dimension);

        //Add middle left indicies
        int innerIter = m_dimension;
        for (int leftIter = coreVertexCount + m_dimension; leftIter < m_vertices.Count - m_dimension; leftIter += 2)
        {
            borderIndicies.Add(leftIter);
            borderIndicies.Add(leftIter + 2);
            borderIndicies.Add(innerIter);

            borderIndicies.Add(innerIter);
            borderIndicies.Add(leftIter + 2);
            borderIndicies.Add(innerIter + m_dimension);

            innerIter += m_dimension;
        }

        //Add middle Right indicies
        int outerIter = (coreVertexCount + m_dimension) - 1;
        for (int leftIter = m_dimension - 1; leftIter < (coreVertexCount - m_dimension) - 1; leftIter += m_dimension)
        {
            borderIndicies.Add(leftIter);
            borderIndicies.Add(leftIter + m_dimension);
            borderIndicies.Add(outerIter);

            borderIndicies.Add(outerIter);
            borderIndicies.Add(leftIter + m_dimension);
            borderIndicies.Add(outerIter + 2);

            outerIter += 2;
        }

        //Add top Right Indicies
        borderIndicies.Add((coreVertexCount - m_dimension) - 1);
        borderIndicies.Add(coreVertexCount - 1);
        borderIndicies.Add((m_vertices.Count - m_dimension) - 1);

        borderIndicies.Add((m_vertices.Count - m_dimension) - 1);
        borderIndicies.Add(coreVertexCount - 1);
        borderIndicies.Add(m_vertices.Count - 1);


        //Add top indicies
        for (int topIter = m_vertices.Count - m_dimension; topIter < m_vertices.Count - 1; topIter++)
        {
            borderIndicies.Add(topIter - borderVertices.Count);
            borderIndicies.Add(topIter);
            borderIndicies.Add((topIter - borderVertices.Count) + 1);

            borderIndicies.Add((topIter - borderVertices.Count) + 1);
            borderIndicies.Add(topIter);
            borderIndicies.Add(topIter + 1);
        }

        m_indicies.AddRange(borderIndicies);

        borderVertices.Clear();
        borderIndicies.Clear();
    }
}
