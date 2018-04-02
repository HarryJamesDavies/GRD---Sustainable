using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckInEvents : MonoBehaviour
{
    public void OnTruckInFinish()
    {
        OutcomeManager.Instance.OnTruckInFinish();
    }
}
