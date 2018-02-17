using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class RetensionTracker : MonoBehaviour
{
    public static RetensionTracker Instance = null;

    [SerializeField]
    public List<RetensionData> m_data = new List<RetensionData>();
    public List<float> m_retensionPecentages = new List<float>();
    public float m_totalRetension = 0.0f;
    
    void Awake()
    {
        CreateInstance();
    }

    public void CreateInstance()
    {
        if(Instance)
        {
            Debug.LogWarning("Multiple RentensionTracker scritps detected." + 
                "\nRemoving youngest RetensionTracker Instance");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public int CreateData(string _dataName)
    {
        int ID = m_data.Count;
        m_data.Add(new RetensionData(_dataName, ID));
        m_retensionPecentages.Add(0.0f);
        return ID;
    }

    public void StopAllData()
    {
        foreach(RetensionData data in m_data)
        {
            data.StopCurrentTimeChunk();
        }
    }

    public void CalculateAllLengths()
    {
        m_totalRetension = 0.0f;
        for (int dataIter = 0; dataIter < m_data.Count; dataIter++)
        {
            m_totalRetension += m_data[dataIter].GetTotalLength();
        }

        for (int dataIter = 0; dataIter < m_data.Count; dataIter++)
        {
            if (m_totalRetension != 0.0f)
            {
                m_retensionPecentages[dataIter] = m_data[dataIter].GetTotalLength() / m_totalRetension;
            }
            else
            {
                m_retensionPecentages[dataIter] = 0.0f;
            }
        }
    }

    public void SaveData()
    {
        if (!Directory.Exists(Application.dataPath + "/Data"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Data");
        }

        string outData = "";

        outData += "RetensionDataCount: " + m_data.Count + "\n" + "\n";

        foreach(RetensionData data in m_data)
        {
            outData += data.ToString();
            outData += "==========================" + "\n" + "\n";
        }

        File.WriteAllText(Application.dataPath + "/Data/SaveData.txt", outData);
        
    }

    public void LoadData(string _fileName)
    {
        if (!Directory.Exists(Application.dataPath + "/Data/SaveData.txt"))
        {
            string[] inData = File.ReadAllLines(Application.dataPath + "/Data/SaveData.txt");

            //Get Data count
            int lineIter = 0;
            int dataCount = 0;
            int.TryParse(inData[lineIter].Replace("RetensionDataCount: ", ""), out dataCount);
            lineIter++;

            //Skip Space
            lineIter++;

            m_data.Clear();
            for (int dataIter = 0; dataIter < dataCount; dataIter++)
            {
                //Get Data name
                CreateData(inData[lineIter]);
                lineIter++;

                //Get Data length
                float.TryParse(inData[lineIter].Replace("TotalLenght: ", ""), out m_data[dataIter].m_totalLength);
                lineIter++;

                //Get Skip Space
                lineIter++;

                //Get Chunk count
                int chunkCount = 0;
                int.TryParse(inData[lineIter].Replace("TimeChunkCount: ", ""), out chunkCount);
                lineIter++;
                
                //Skip Space
                lineIter++;

                for(int chunkIter = 0; chunkIter < chunkCount; chunkIter++)
                {
                    //Create Chunk and Get Chunk ID
                    int chunkID = -1;
                    int.TryParse(inData[lineIter].Replace("ID: ", ""), out chunkID);
                    m_data[dataIter].m_timeChunks.Add(new TimeChunk(chunkID));
                    lineIter++;

                    //Get Start Time
                    float.TryParse(inData[lineIter].Replace("StartTime: ", ""), out m_data[dataIter].m_timeChunks[chunkIter].m_startTime);
                    lineIter++;

                    //Get End Time
                    float.TryParse(inData[lineIter].Replace("EndTime: ", ""), out m_data[dataIter].m_timeChunks[chunkIter].m_endTime);
                    lineIter++;

                    //Get Chunk Length
                    float.TryParse(inData[lineIter].Replace("ChunkLength: ", ""), out m_data[dataIter].m_timeChunks[chunkIter].m_chunkLength);
                    lineIter++;

                    //Skip Space
                    lineIter++;
                }

                //Skip 2 Spaces
                lineIter++;
                lineIter++;
            }
        }
    }
}
