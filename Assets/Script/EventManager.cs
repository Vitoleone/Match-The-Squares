using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    /// <summary>
    /// Triggered on player select second square.
    /// </summary>
    public delegate void OnSecondSquareSelected();
    public OnSecondSquareSelected onSecondSquareSelected;
    /// <summary>
    /// Treggered on a square is cracked.
    /// </summary>
    public delegate void OnCracked();
    public OnCracked onCracked;
    /// <summary>
    /// Triggered on a square is spawned
    /// </summary>
    public Action<Square> onSpawned;
    /// <summary>
    /// Checks changed two squares vertically and horizontally. If second square value is null checks only first square value.
    /// </summary>
    public Action<Square,Square> onCrackControll;

    //UI Section

    /// <summary>
    /// Updates values when score value is changed.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public delegate int OnGetScore(SquareType type);
    public OnGetScore onGetScore;
  
   
}
