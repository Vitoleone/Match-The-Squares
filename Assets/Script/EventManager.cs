using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    //In-Game
    public delegate void OnSecondSquareSelected();
    public delegate void OnSquareMoved();
    public delegate void OnCracked();
    public delegate void OnSpawned();
    public OnSecondSquareSelected onSecondSquareSelected;
    public OnCracked onCracked;
    public OnSpawned onSpawned;
    public OnSquareMoved onSquareMoved;

    //UI
    public delegate int OnGetScore(SquareType type);
    public OnGetScore onGetScore;
  
   
}
