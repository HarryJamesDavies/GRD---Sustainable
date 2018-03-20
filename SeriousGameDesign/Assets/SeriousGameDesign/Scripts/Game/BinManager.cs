using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinManager : MonoBehaviour
{
    public static BinManager Instance;

    public List<BinCore> m_bins = new List<BinCore>();

    void Awake()
    {
        CreateInstance();
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

    public void AddBin(BinCore _bin)
    {
        m_bins.Add(_bin);
    }

    public void EmptyBins()
    {
        foreach(BinCore bin in m_bins)
        {
            bin.EmptyBin();
        }
    }
}
