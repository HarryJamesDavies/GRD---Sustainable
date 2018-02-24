using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollection : MonoBehaviour
{    
	void Awake ()
    {
        gameObject.AddComponent<RetensionTracker>();
	}
	
	void Update ()
    {
		
	}
}
