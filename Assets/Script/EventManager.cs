using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public Action onSecondSquareSelected;
    public Action onSquareMoved;
    public Action onCracked;
    public Action onSpawned;
}
