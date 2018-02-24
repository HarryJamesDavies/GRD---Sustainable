using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneChanger.TransitionScene("Main");
    }
}
