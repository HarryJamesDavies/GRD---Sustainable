using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSceneManager : MonoBehaviour
{
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChanger.TransitionScene("MainMenu");
        }
	}
}
