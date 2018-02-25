using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataHandler : MonoBehaviour
{
    public static string InDataPath;
    public static string OutDataPath;

    private void Awake()
    {
        InDataPath = Application.dataPath + "/InData";
        OutDataPath = Application.dataPath + "/OutData";
    }
    public static void SaveCategoricData(string _fileName, string _filePath, List<CategoricData.CategoricPair> _data)
    {
        if (!Directory.Exists(_filePath))
        {
            Directory.CreateDirectory(_filePath);
        }

        string outData = "Categoric " + _data.Count + " ";

        for (int inIter = 0; inIter < _data.Count; inIter++)
        {
            outData += _data[inIter].Key + " " + _data[inIter].Value + " ";

        }
        
        File.WriteAllText(_filePath + "/" + _fileName + ".txt", outData);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

    }

    public static void SaveNumericalData(string _fileName, string _filePath, List<NumericalData.NumericalPair> _data)
    {
        if (!Directory.Exists(_filePath))
        {
            Directory.CreateDirectory(_filePath);
        }

        string outData = "Numerical " + _data.Count + " ";

        foreach (NumericalData.NumericalPair value in _data)
        {
            outData += value.Key + " " + value.value + " ";
        }

        File.WriteAllText(_filePath + "/" + _fileName + ".txt", outData);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public static Data LoadData(string _fileName, string _filePath)
    {
        string path = _filePath + "/" + _fileName + ".txt";
        if (!Directory.Exists(path))
        {
            string inData = File.ReadAllText(path);
            string[] inValues = inData.Split(' ');

            int valueCount = 0;
            int.TryParse(inValues[1], out valueCount);

            if (inValues[0] == "Categoric")
            {
                CategoricData data = new CategoricData();
                 
                for (int valueIter = 0; valueIter < valueCount; valueIter++)
                {
                    int index = (valueIter * 2) + 2;
                    float value = 0;
                    float.TryParse(inValues[index + 1], out value);
                    data.m_data.Add(new CategoricData.CategoricPair(inValues[index], value));
                }

                return data;
            }
            else
            {
                NumericalData data = new NumericalData();

                for (int valueIter = 0; valueIter < valueCount; valueIter++)
                {
                    int index = (valueIter * 2) + 2;
                    float key = 0;
                    float.TryParse(inValues[index], out key);
                    float value = 0;
                    float.TryParse(inValues[index + 1], out value);
                    data.m_data.Add(new NumericalData.NumericalPair(key, value));
                }

                return data;

            }
        }
        return null;
    }

    public static Data LoadData(string _filePath)
    {
        if (!Directory.Exists(_filePath))
        {
            string inData = File.ReadAllText(_filePath);
            string[] inValues = inData.Split(' ');

            int valueCount = 0;
            int.TryParse(inValues[1], out valueCount);

            if (inValues[0] == "Catergoric")
            {
                CategoricData data = new CategoricData();

                for (int valueIter = 0; valueIter < valueCount; valueIter++)
                {
                    int index = (valueIter * 2) + 2;
                    float value = 0;
                    float.TryParse(inValues[index + 1], out value);
                    data.m_data.Add(new CategoricData.CategoricPair(inValues[index], value));
                }

                return data;
            }
            else
            {
                NumericalData data = new NumericalData();

                for (int valueIter = 0; valueIter < valueCount; valueIter++)
                {
                    int index = (valueIter * 2) + 2;
                    float key = 0;
                    float.TryParse(inValues[index], out key);
                    float value = 0;
                    float.TryParse(inValues[index + 1], out value);
                    data.m_data.Add(new NumericalData.NumericalPair(key, value));
                }

                return data;

            }
        }
        return null;
    }
}

public enum DataType
{
    Categoric,
    Numerical
}

[Serializable]
public class Data
{
    public DataType m_type;
}

[Serializable]
public class CategoricData : Data
{
    [Serializable]
    public class CategoricPair
    {
        public string Key;
        public float Value;

        public CategoricPair(string _key, float _value)
        {
            Key = _key;
            Value = _value;
        }
    }

    [SerializeField] public List<CategoricPair> m_data = new List<CategoricPair>();

    public CategoricData()
    {
        m_type = DataType.Categoric;
    }

    public List<string> GetKeys()
    {
        List<string> keys = new List<string>();
        foreach (CategoricPair pair in m_data)
        {
            keys.Add(pair.Key);
        }
        return keys;
    }

    public List<float> GetValues()
    {
        List<float> values = new List<float>();
        foreach (CategoricPair pair in m_data)
        {
            values.Add(pair.Value);
        }
        return values;
    }

    public List<float> GetPercentages()
    {
        float total = 0;
        List<float> percentages = new List<float>();
        foreach (CategoricPair pair in m_data)
        {
            total += pair.Value;
            percentages.Add(pair.Value);
        }

        for (int percentageIter = 0; percentageIter < percentages.Count; percentageIter++)
        {
            percentages[percentageIter] /= total;
        }

        return percentages;

    }
}

[Serializable]
public class NumericalData : Data
{
    [Serializable]
    public class NumericalPair
    {
        public float Key;
        public float value;

        public NumericalPair(float _key, float _value)
        {
            Key = _key;
            value = _value;
        }
    }

    [SerializeField] public List<NumericalPair> m_data = new List<NumericalPair>();

    public NumericalData()
    {
        m_type = DataType.Numerical;
    }

    public List<float> GetKeys()
    {
        List<float> keys = new List<float>();
        foreach (NumericalPair pair in m_data)
        {
            keys.Add(pair.Key);
        }
        return keys;
    }

    public List<float> GetValues()
    {
        List<float> values = new List<float>();
        foreach (NumericalPair pair in m_data)
        {
            values.Add(pair.value);
        }
        return values;
    }
}