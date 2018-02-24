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
                GameManager.Instance.RecyclingRecycled();
            }
            else
            {
                GameManager.Instance.RubbishRecycled();
            }
            Destroy(_object);
        }
    }
}
