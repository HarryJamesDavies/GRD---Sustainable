using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RetensionData
{
    public string m_dataName;
    public int m_ID = -1;

    public float m_totalLength = 0.0f;

    [SerializeField]
    public List<TimeChunk> m_timeChunks = new List<TimeChunk>();

    public RetensionData(string _dataName, int _ID)
    {
        m_dataName = _dataName;
        m_ID = _ID;
    }

    /// <summary>
    /// Generates and starts new TimeChunk. 
    /// Returns Chunk ID, which is needed to access the Chunk later.
    /// </summary>
    /// <returns></returns>
    public int StartTimeChunk()
    {
        int ID = m_timeChunks.Count;
        m_timeChunks.Add(new TimeChunk(ID));
        return ID;
    }
    
    /// <summary>
    /// Stops specfic Chunk.
    /// Returns the Chunk Length.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public float StopTimeChunk(int _ID)
    {
        m_timeChunks[_ID].StopChunk();
        return m_timeChunks[_ID].m_chunkLength;
    }

    public float StopCurrentTimeChunk()
    {
        m_timeChunks[m_timeChunks.Count - 1].StopChunk();
        return m_timeChunks[m_timeChunks.Count - 1].m_chunkLength;
    }

    public void ClearChunks()
    {
        m_timeChunks.Clear();
    }

    public float GetTotalLength()
    {
        return CalculateTotalLength();
    }

    private float CalculateTotalLength()
    {
        m_totalLength = 0.0f;
        foreach(TimeChunk chunk in m_timeChunks)
        {
            m_totalLength += chunk.m_chunkLength;
        }
        return m_totalLength;
    }

    public override string ToString()
    {
        string content = "";

        content += m_dataName + "\n";
        content += "TotalLenght: " + CalculateTotalLength() + "\n" + "\n";
        content += "TimeChunkCount: " + m_timeChunks.Count + "\n" + "\n";

        foreach (TimeChunk chunk in m_timeChunks)
        {
            content += chunk.ToString() + "\n";
        }

        return content;
    }
}

[Serializable]
public class TimeChunk
{
    public int m_ID = -1;

    public float m_startTime;
    public float m_endTime;
    public float m_chunkLength = 0.0f;

    public TimeChunk(int _ID)
    {
        m_ID = _ID;
        m_startTime = Time.time;
    }

    public void StopChunk()
    {
        m_endTime = Time.time;
        m_chunkLength = m_endTime - m_startTime;
    }

    public override string ToString()
    {
        if (m_endTime == 0.0f)
        {
            StopChunk();
        }

        string content = "";

        content += "ID: " + m_ID + "\n";

        content += "StartTime: " + m_startTime + "\n";
        content += "EndTime: " + m_endTime + "\n";
        content += "ChunkLength: " + m_chunkLength + "\n";

        return content;
    }
}