using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ExternalViewer;

public enum GraphType
{
    Bar,
    Pie,
    Line,
    Count
}

public class GraphHandler : MonoBehaviour
{
    public static GraphHandler Instance = null;

    [SerializeField]
    public BarChart m_barChart;
    [SerializeField]
    public PieChart m_pieChart;
    [SerializeField]
    public LineGraph m_lineGraph;
    public Dropdown m_graphDropdown;

    public Dropdown m_fileDropdown;
    public Button m_loadButton;

    private bool m_dataLoaded;
    public GraphType m_graphSelected = GraphType.Bar;

    private DataType m_type;
    private CategoricData m_categoricData;
    private NumericalData m_numericalData;

    private string m_selectedFileName = "";

    void Start()
    {
        if (!Directory.Exists(DataHandler.InDataPath))
        {
            Directory.CreateDirectory(DataHandler.InDataPath);
        }

        if (!Directory.Exists(DataHandler.OutDataPath))
        {
            Directory.CreateDirectory(DataHandler.OutDataPath);
        }       

        List<string> files = FindExternalData();
        List<Dropdown.OptionData> fileData = new List<Dropdown.OptionData>();
        for (int fileIter = 0; fileIter < files.Count; fileIter++)
        {
            fileData.Add(new Dropdown.OptionData(files[fileIter]));
        }
        m_fileDropdown.AddOptions(fileData);

        m_loadButton.onClick.AddListener(LoadData);
    }

    private void CreateInstance()
    {
        if(Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddData(Data _data)
    {
        if (_data.m_type == DataType.Categoric)
        {
            m_type = DataType.Categoric;
            m_categoricData = (CategoricData)_data;
            m_numericalData = null;

            m_graphDropdown.ClearOptions();
            List<Dropdown.OptionData> graphData = new List<Dropdown.OptionData>();
            graphData.Add(new Dropdown.OptionData(GraphType.Bar.ToString()));
            graphData.Add(new Dropdown.OptionData(GraphType.Pie.ToString()));
            m_graphDropdown.AddOptions(graphData);
        }
        else
        {
            m_type = DataType.Numerical;
            m_numericalData = (NumericalData)_data;
            m_categoricData = null;

            m_graphDropdown.ClearOptions();
            List<Dropdown.OptionData> graphData = new List<Dropdown.OptionData>();
            graphData.Add(new Dropdown.OptionData(GraphType.Line.ToString()));
            m_graphDropdown.AddOptions(graphData);
        }
    }

    private List<string> FindExternalData()
    {
        List<string> files = new List<string>();
        files.Add("");

        DirectoryInfo info = new DirectoryInfo(DataHandler.InDataPath + "/");
        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo)
        {
            if (!file.FullName.Contains("meta") && file.FullName.Contains("txt"))
            {
                string name = file.FullName.Remove(0, DataHandler.InDataPath.Length + 1);
                name = name.Remove(name.Length - 4, 4);
                files.Add(name);
            }
        }
        return files;
    }

    void Update()
    {
        if (!m_dataLoaded)
        {
            m_barChart.transform.parent.gameObject.SetActive(false);
            m_pieChart.gameObject.SetActive(false);
            m_lineGraph.gameObject.SetActive(false);
        }
        else
        {
            m_graphSelected = (GraphType)m_graphDropdown.value;

            switch (m_graphSelected)
            {
                case GraphType.Bar:
                    {
                        if (m_categoricData != null)
                        {
                            m_barChart.transform.parent.gameObject.SetActive(true);
                            m_pieChart.gameObject.SetActive(false);
                            m_lineGraph.gameObject.SetActive(false);
                            m_barChart.Draw(m_categoricData.GetValues(), m_categoricData.GetKeys());
                        }
                        break;
                    }
                case GraphType.Pie:
                    {
                        if (m_categoricData != null)
                        {
                            m_barChart.transform.parent.gameObject.SetActive(false);
                            m_pieChart.gameObject.SetActive(true);
                            m_lineGraph.gameObject.SetActive(false);
                            m_pieChart.Draw(MultiplyList(m_categoricData.GetPercentages(), 360.0f), Vector3.zero);
                        }
                        break;
                    }
                case GraphType.Line:
                    {
                        if (m_numericalData != null)
                        {
                            m_barChart.transform.parent.gameObject.SetActive(false);
                            m_pieChart.gameObject.SetActive(false);
                            m_lineGraph.gameObject.SetActive(true);
                            List<LinePoints> linePoints = new List<LinePoints>();
                            linePoints.Add(GetLinePoints(m_numericalData, 0.0f));
                            m_lineGraph.Draw(linePoints);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    private List<float> MultiplyList(List<float> _values, float _multipiler)
    {
        for (int valueIter = 0; valueIter < _values.Count; valueIter++)
        {
            _values[valueIter] *= _multipiler;
        }
        return _values;
    }

    public static LinePoints GetLinePoints(NumericalData _Data, float _depth)
    {
        LinePoints linePoints = new LinePoints();
        linePoints.m_points = new List<Vector3>();

        for (int dataIter = 0; dataIter < _Data.m_data.Count; dataIter++)
        {
            linePoints.m_points.Add(new Vector3(_Data.m_data[dataIter].Key,
                _Data.m_data[dataIter].value, _depth));
        }

        return linePoints;
    }

    private void LoadData()
    {
        m_selectedFileName = m_fileDropdown.options[m_fileDropdown.value].text;
        AddData(DataHandler.LoadData(m_selectedFileName, DataHandler.InDataPath));
        m_dataLoaded = true;
    }
}
