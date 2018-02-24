using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyclingBin : MonoBehaviour
{
    public void BinObject(GameObject _object)
    {
        if (_object)
        {
            if(_object.CompareTag("Recycling"))
            {
                WorldManager.Instance.RecyclingRecycled();
            }
            else
            {
                WorldManager.Instance.RubbishRecycled();
            }
            Destroy(_object);
        }
    }
}
