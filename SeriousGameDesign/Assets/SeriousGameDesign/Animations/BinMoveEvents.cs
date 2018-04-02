using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinMoveEvents : MonoBehaviour
{
    public void OnBinIn()
    {
        OutcomeManager.Instance.OnBinIn();
    }

    public void OnBeginDump(float _length)
    {
        OutcomeManager.Instance.OnDumpBegin(_length);
    }

    public void OnFinishDump()
    {
        OutcomeManager.Instance.OnDumpEnd();
    }

    public void OnBinOut()
    {
        OutcomeManager.Instance.OnBinOut();
    }
}
