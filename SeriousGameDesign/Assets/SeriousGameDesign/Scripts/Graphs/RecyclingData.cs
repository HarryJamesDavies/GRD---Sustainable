using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecyclingData : MonoBehaviour
{
    public string m_folderName = "";
    public List<string> m_dayFiles = new List<string>();

    public CategoricData m_dailyRecycledRecycling = new CategoricData();
    public CategoricData m_dailyLandfilledRubbish = new CategoricData();
    public CategoricData m_dailyLandfilledRecycling = new CategoricData();
    public CategoricData m_dailySortedRubbish = new CategoricData();
    public CategoricData m_dailySortedRecycling = new CategoricData();

    public string m_weekFile = "";
    public CategoricData m_weekData = new CategoricData();

    public void LoadFolder(string _folderName)
    {
        m_folderName = _folderName;
        FindExternalData();

        for(int dayIter = 0; dayIter < m_dayFiles.Count; dayIter++)
        {
            ImportDayData(dayIter + 1, m_dayFiles[dayIter]);
        }
        ImportWeekData();
    }

    private void FindExternalData()
    {
        m_dayFiles.Clear();

        DirectoryInfo info = new DirectoryInfo(m_folderName);
        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo)
        {
            if (!file.FullName.Contains("meta") && file.FullName.Contains("txt"))
            {
                string name = file.FullName.Remove(0, m_folderName.Length);
                name = name.Remove(name.Length - 4, 4);

                if (file.FullName.Contains("Week"))
                {
                    m_weekFile = name;
                }
                else
                {
                    m_dayFiles.Add(name);
                }
            }
        }
    }

    private void ImportWeekData()
    {
        m_weekData = (CategoricData)DataHandler.LoadData(m_weekFile, m_folderName.Remove(m_folderName.Length - 1));       
    }

    private void ImportDayData(int _day, string _fileName)
    {
        CategoricData data = (CategoricData)DataHandler.LoadData(_fileName, m_folderName.Remove(m_folderName.Length - 1));
        for(int dataIter = 0; dataIter < data.m_data.Count; dataIter++)
        {
            HandleDayPair(_day, data.m_data[dataIter]);
        }
    }

    private void HandleDayPair(int _day, CategoricData.CategoricPair _pair)
    {
        switch(_pair.Key)
        {
            case "RecycledRecyclingDay":
                {
                    m_dailyRecycledRecycling.m_data.Add(new CategoricData.CategoricPair("Day " + _day, _pair.Value));
                    break;
                }
            case "LandfilledRubbishDay":
                {
                    m_dailyLandfilledRubbish.m_data.Add(new CategoricData.CategoricPair("Day " + _day, _pair.Value));
                    break;
                }
            case "LandfilledRecyclingDay":
                {
                    m_dailyLandfilledRecycling.m_data.Add(new CategoricData.CategoricPair("Day " + _day, _pair.Value));
                    break;
                }
            case "SortedRubbishDay":
                {
                    m_dailySortedRubbish.m_data.Add(new CategoricData.CategoricPair("Day " + _day, _pair.Value));
                    break;
                }
            case "SortedRecyclingDay":
                {
                    m_dailySortedRecycling.m_data.Add(new CategoricData.CategoricPair("Day " + _day, _pair.Value));
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
