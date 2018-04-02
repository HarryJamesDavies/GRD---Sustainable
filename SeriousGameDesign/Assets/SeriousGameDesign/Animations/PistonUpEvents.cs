using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonUpEvents : MonoBehaviour
{
    public void OnPistonUpFinish()
    {
        OutcomeManager.Instance.OnPistonUpFinish();
    }
}
