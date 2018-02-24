using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishBin : MonoBehaviour
{
    public void BinObject(GameObject _object)
    {
        if (_object.CompareTag("Rubbish"))
        {
            GameManager.Instance.RubbishDestroyed();
        }
        else
        {
            GameManager.Instance.RecyclingDestroyed();
        }
        Destroy(_object);
    }
}
