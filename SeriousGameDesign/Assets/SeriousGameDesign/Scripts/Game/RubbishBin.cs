using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishBin : MonoBehaviour
{
    public void BinObject(GameObject _object)
    {
        if (_object.CompareTag("Trash"))
        {
            WorldManager.Instance.RubbishDestroyed();
        }
        else
        {
            WorldManager.Instance.RecyclingDestroyed();
        }
        Destroy(_object);
    }
}
