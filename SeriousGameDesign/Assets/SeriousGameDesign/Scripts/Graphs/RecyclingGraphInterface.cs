using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ExternalViewer;

public enum Comparisons
{
    RecycledRecyclingDay,
    LandfilledRubbishDay,
    LandfilledRecyclingDay,
    SortedRubbishDay,
    SortedRecyclingDay,
    WeekData
}

public class RecyclingGraphInterface : MonoBehaviour
{
    public static RecyclingGraphInterface Instance = null;

    [SerializeField]
    public BarChart m_barChart;
    [SerializeField]
    public PieChart m_pieChart;
    [SerializeField]
    public LineGraph m_lineGraph;
    public Dropdown m_graphDropdown;
    public Dropdown m_comparisonDropdown;

    public Dropdown m_fileDropdown;
    public Button m_loadButton;

    private bool m_dataLoaded;
    public GraphType m_graphSelected = GraphType.Bar;
    public Comparisons m_comparisonType = Comparisons.RecycledRecyclingDay;

    private DataType m_type;
    private CategoricData m_categoricData;
    private NumericalData m_numericalData;

    private RecyclingData m_data;

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

        List<string> files = FindExternalFolders();
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
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private List<string> FindExternalFolders()
    {
        List<string> folders = new List<string>();
        folders.Add("");

        DirectoryInfo info = new DirectoryInfo(DataHandler.InDataPath + "/");
        DirectoryInfo[] folderInfo = info.GetDirectories();
        foreach (DirectoryInfo folder in folderInfo)
        {
            string name = folder.FullName.Remove(0, DataHandler.InDataPath.Length + 1);
            folders.Add(name);
        }
        return folders;
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
            if (m_data != null)
            {
                HandleComparisons();
            }
        }
    }

    private void HandleComparisons()
    {
        m_comparisonType = (Comparisons)m_comparisonDropdown.value;

        switch (m_comparisonType)
        {
            case Comparisons.RecycledRecyclingDay:
                {
                    HandleCategoricGraph(m_data.m_dailyRecycledRecycling);
                    break;
                }
            case Comparisons.LandfilledRubbishDay:
                {
                    HandleCategoricGraph(m_data.m_dailyLandfilledRubbish);
                    break;
                }
            case Comparisons.LandfilledRecyclingDay:
                {
                    HandleCategoricGraph(m_data.m_dailyLandfilledRecycling);
                    break;
                }
            case Comparisons.SortedRubbishDay:
                {
                    HandleCategoricGraph(m_data.m_dailySortedRubbish);
                    break;
                }
            case Comparisons.SortedRecyclingDay:
                {
                    HandleCategoricGraph(m_data.m_dailySortedRecycling);
                    break;
                }
            case Comparisons.WeekData:
                {
                    HandleCategoricGraph(m_data.m_weekData);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void HandleCategoricGraph(CategoricData _categoricData)
    {
        m_graphSelected = (GraphType)m_graphDropdown.value;

        switch (m_graphSelected)
        {
            case GraphType.Bar:
                {
                    DrawBarChart(_categoricData);
                    break;
                }
            case GraphType.Pie:
                {
                    DrawPiechart(_categoricData);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void HandleNumericalGraph(NumericalData _numericalData)
    {
        m_graphSelected = (GraphType)m_graphDropdown.value;

        switch (m_graphSelected)
        {
            case GraphType.Line:
                {
                    DrawLineGraph(_numericalData);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void DrawBarChart(CategoricData _data)
    {
        m_barChart.transform.parent.gameObject.SetActive(true);
        m_pieChart.gameObject.SetActive(false);
        m_lineGraph.gameObject.SetActive(false);
        m_barChart.Draw(_data.GetValues(), _data.GetKeys());
    }

    private void DrawPiechart(CategoricData _data)
    {
        m_barChart.transform.parent.gameObject.SetActive(false);
        m_pieChart.gameObject.SetActive(true);
        m_lineGraph.gameObject.SetActive(false);
        m_pieChart.Draw(MultiplyList(_data.GetPercentages(), 360.0f), Vector3.zero);
    }

    private void DrawLineGraph(NumericalData _data)
    {
        m_barChart.transform.parent.gameObject.SetActive(false);
        m_pieChart.gameObject.SetActive(false);
        m_lineGraph.gameObject.SetActive(true);
        List<LinePoints> linePoints = new List<LinePoints>();
        linePoints.Add(GetLinePoints(_data, 0.0f));
        m_lineGraph.Draw(linePoints);
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
        m_selectedFileName = DataHandler.InDataPath + "/" + m_fileDropdown.options[m_fileDropdown.value].text + "/";

        m_data = gameObject.GetComponent<RecyclingData>();
        if (m_data)
        {
            Destroy(m_data);
            m_data = null;
        }

        m_data = gameObject.AddComponent<RecyclingData>();
        m_data.LoadFolder(m_selectedFileName);

        m_graphDropdown.ClearOptions();
        List<Dropdown.OptionData> graphData = new List<Dropdown.OptionData>();
        graphData.Add(new Dropdown.OptionData(GraphType.Bar.ToString()));
        graphData.Add(new Dropdown.OptionData(GraphType.Pie.ToString()));
        m_graphDropdown.AddOptions(graphData);

        m_comparisonDropdown.ClearOptions();
        List<Dropdown.OptionData> comparisonData = new List<Dropdown.OptionData>();
        comparisonData.Add(new Dropdown.OptionData(Comparisons.RecycledRecyclingDay.ToString()));
        comparisonData.Add(new Dropdown.OptionData(Comparisons.LandfilledRubbishDay.ToString()));
        comparisonData.Add(new Dropdown.OptionData(Comparisons.LandfilledRecyclingDay.ToString()));
        comparisonData.Add(new Dropdown.OptionData(Comparisons.SortedRubbishDay.ToString()));
        comparisonData.Add(new Dropdown.OptionData(Comparisons.SortedRecyclingDay.ToString()));
        comparisonData.Add(new Dropdown.OptionData(Comparisons.WeekData.ToString()));
        m_comparisonDropdown.AddOptions(comparisonData);

        m_dataLoaded = true;
    }


}
