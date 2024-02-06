using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public delegate void OnSecondSquareSelected();
    public delegate void OnSquareMoved();
    public delegate void OnCracked();
    public delegate void OnSpawned();
    public OnSecondSquareSelected onSecondSquareSelected;
    public OnSquareMoved onSquareMoved;
    public OnCracked onCracked;
    public OnSpawned onSpawned;
}
