﻿using UnityEngine;
using System.Collections;

public class ControllerAddedEventData : ScriptableObject
{
    public string ControllerName;
    public InputMapManager.InputType ControllerType;
    public int ControllerIndex;
}
