using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExternalViewer;

[Serializable]
public class RetensionTracker : MonoBehaviour
{
    public static RetensionTracker Instance = null;
    
    [SerializeField] public List<RetensionData> m_data = new List<RetensionData>();

    [SerializeField] public BarChart m_barChart;
    [SerializeField] public PieChart m_pieChart;
    [SerializeField] public LineGraph m_lineGraph;
    public Dropdown m_graphDropdown;

    public Dropdown m_fileDropdown;
    public Button m_loadButton;

    private bool m_dataLoaded;
    public GraphType m_graphSelected = GraphType.Bar;

    public string m_dataPath;

    void Awake()
    {
        CreateInstance();

        m_dataPath = Application.dataPath + "/GraphData";
        if (!Directory.Exists(m_dataPath))
        {
            Directory.CreateDirectory(m_dataPath);
        }

        List<Dropdown.OptionData> graphData = new List<Dropdown.OptionData>();
        for (int graphIter = 0; graphIter < (int)GraphType.Count; graphIter++)
        {
            GraphType type = (GraphType)graphIter;
            graphData.Add(new Dropdown.OptionData(type.ToString()));
        }
        m_graphDropdown.AddOptions(graphData);

        List<string> files = FindExternalData();
        List<Dropdown.OptionData> fileData = new List<Dropdown.OptionData>();
        for (int fileIter = 0; fileIter < files.Count; fileIter++)
        {
            fileData.Add(new Dropdown.OptionData(files[fileIter]));
        }
        m_fileDropdown.AddOptions(fileData);

        m_loadButton.onClick.AddListener(LoadExternalData);
    }

    public void CreateInstance()
    {
        if(Instance)
        {
            UnityEngine.Debug.LogWarning("Multiple RentensionTracker scritps detected." + 
                "\nRemoving youngest RetensionTracker Instance");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if(!m_dataLoaded)
        {
            m_barChart.transform.parent.gameObject.SetActive(false);
            m_pieChart.gameObject.SetActive(false);
            m_lineGraph.gameObject.SetActive(false);
        }
        else
        {
            m_graphSelected = (GraphType)m_graphDropdown.value;

            switch(m_graphSelected)
            {
                case GraphType.Bar:
                    {
                        m_barChart.transform.parent.gameObject.SetActive(true);
                        m_pieChart.gameObject.SetActive(false);
                        m_lineGraph.gameObject.SetActive(false);
                        m_barChart.Draw(CalculateAllLengths(), GetDataNames());
                        break;
                    }
                case GraphType.Pie:
                    {
                        m_barChart.transform.parent.gameObject.SetActive(false);
                        m_pieChart.gameObject.SetActive(true);
                        m_lineGraph.gameObject.SetActive(false);
                        m_pieChart.Draw(CalculateAllLengths(true));
                        break;
                    }
                case GraphType.Line:
                    {
                        m_barChart.transform.parent.gameObject.SetActive(false);
                        m_pieChart.gameObject.SetActive(false);
                        m_lineGraph.gameObject.SetActive(true);
                        m_lineGraph.Draw(GetLinePoints());
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    private List<string> FindExternalData()
    {
        List<string> files = new List<string>();
        files.Add("");

        DirectoryInfo info = new DirectoryInfo(m_dataPath);
        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo)
        {
            if (!file.FullName.Contains("meta") && file.FullName.Contains("txt"))
            {
                files.Add(file.FullName.Replace(m_dataPath, ""));
            }
        }
        return files;
    }

    public void LoadExternalData()
    {
        string path = m_fileDropdown.options[m_fileDropdown.value].text;
        if (path != "")
        {
            if(File.Exists(path))
            {
                LoadData(path);
                m_dataLoaded = true;
            }
        }
    }

    public int CreateData(string _dataName)
    {
        int ID = m_data.Count;
        m_data.Add(new RetensionData(_dataName, ID));
        return ID;
    }

    public void StopAllData()
    {
        foreach(RetensionData data in m_data)
        {
            data.StopCurrentTimeChunk();
        }
    }

    public List<float> CalculateAllLengths(bool _percentage = false)
    {
        List<float> retension = new List<float>();

        if (_percentage)
        {
            float totalRetension = 0.0f;
            for (int dataIter = 0; dataIter < m_data.Count; dataIter++)
            {
                totalRetension += m_data[dataIter].GetTotalLength();
            }


            for (int dataIter = 0; dataIter < m_data.Count; dataIter++)
            {
                if (totalRetension != 0.0f)
                {
                    retension.Add(360.0f * (m_data[dataIter].GetTotalLength() / totalRetension));
                }
                else
                {
                    retension.Add(0.0f);
                }
            }
        }
        else
        {
            for (int dataIter = 0; dataIter < m_data.Count; dataIter++)
            {
                retension.Add(m_data[dataIter].GetTotalLength());
            }
        }

        return retension;
    }

    public List<string> GetDataNames()
    {
        List<string> names = new List<string>();
        for (int dataIter = 0; dataIter < m_data.Count; dataIter++)
        {
            names.Add(m_data[dataIter].m_dataName);
        }

        return names;
    } 

    public List<LinePoints> GetLinePoints()
    {
        List<LinePoints> linePoints = new List<LinePoints>();
        for (int dataIter = 0; dataIter < m_data.Count; dataIter++)
        {
            LinePoints newLinePoint = new LinePoints();
            newLinePoint.m_points = new List<Vector3>();

            for (int chunkIter = 0; chunkIter < m_data[dataIter].m_timeChunks.Count; chunkIter++)
            {
                newLinePoint.m_points.Add(new Vector3(m_data[dataIter].m_timeChunks[chunkIter].m_startTime * 3.0f, 
                    m_data[dataIter].m_timeChunks[chunkIter].m_chunkLength * 3.0f, dataIter));
            }

            linePoints.Add(newLinePoint);
        }

        return linePoints;
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
        if (!Directory.Exists(_fileName))
        {
            string[] inData = File.ReadAllLines(_fileName);

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
