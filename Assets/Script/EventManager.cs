using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    //In-Game
    public delegate void OnSecondSquareSelected();
    public delegate void OnSquareMoved(Square square1, Square square2);
    public delegate void OnCracked();
   
    public OnSecondSquareSelected onSecondSquareSelected;
    public OnCracked onCracked;
    public Action<Square> onSpawned;
    public OnSquareMoved onSquareMoved;
    public Action<Square,Square> onCrackControll;

    //UI
    public delegate int OnGetScore(SquareType type);
    public OnGetScore onGetScore;
  
   
}
