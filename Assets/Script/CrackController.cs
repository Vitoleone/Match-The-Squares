using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrackController : Singleton<CrackController>
{
 
    private void Start()
    {
        EventManager.instance.onCrackControll += CrackCheck;
    }
    /// <summary>
    /// Checks the square table if it has squares that can be cracked or not. Can check just one square by assign square2 parameter to null and square1 is wanted square.
    /// </summary>
    void CrackCheck(Square square1, Square square2)
    {
        if (square2 == null)
        {
            CheckHorizontally(square1.y);
            CheckVertical(square1.x);
        }
        else
        {
            int x1 = square1.x;
            int y1 = square1.y;

            int x2 = square2.x;
            int y2 = square2.y;

            if (x1 == x2)
            {
                CheckVertical(x1);
                CheckHorizontally(y1);
                CheckHorizontally(y2);
            }
            else if (y1 == y2)
            {
                CheckHorizontally(y1);
                CheckVertical(x1);
                CheckVertical(x2);
            }
        }
    }
    /// <summary>
    /// Checks given y value of squareList horizontally and if there are 3 and more same type of squares in a row, crack them.
    /// </summary>
    /// <param name="y"></param>
    private void CheckHorizontally(int y)
    {
        SquareType previousType = SquareType.None;
        List<Square> sameTypeSquares = new List<Square>();
        for(int i = 0; i < SquareManager.instance.squareList.GetLength(1); i++)
        {
            if (previousType == SquareType.None)
            {
                previousType = SquareManager.instance.squareList[y, i].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[y, i]);
            }
            else if (previousType == SquareManager.instance.squareList[y, i].GetSquareType())
            {
                previousType = SquareManager.instance.squareList[y, i].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[y, i]);
            }
            else if (previousType != SquareManager.instance.squareList[y, i].GetSquareType() && sameTypeSquares.Count < 3)
            {
                previousType = SquareManager.instance.squareList[y, i].GetSquareType();
                sameTypeSquares.Clear();
                sameTypeSquares.Add(SquareManager.instance.squareList[y, i]);
            }
            else if (previousType != SquareManager.instance.squareList[y, i].GetSquareType() && sameTypeSquares.Count >= 3)
            {
                CrackSameTypeSquares(sameTypeSquares,true);
                previousType = SquareManager.instance.squareList[y, i].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[y, i]);
            }
        }
        if (sameTypeSquares.Count >= 3)
        {
            CrackSameTypeSquares(sameTypeSquares, true);
        }
    }
    /// <summary>
    /// Checks given x value of squareList horizontally and if there are 3 and more same type of squares in a row, crack them.
    /// </summary>
    /// <param name="x"></param>
    private void CheckVertical(int x)
    {
        SquareType previousType = SquareType.None;
        List<Square> sameTypeSquares = new List<Square>();
        for (int i = 0; i < SquareManager.instance.squareList.GetLength(0); i++)
        {
            if (previousType == SquareType.None)
            {
                previousType = SquareManager.instance.squareList[i, x].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[i, x]);
            }
            else if (previousType == SquareManager.instance.squareList[i, x].GetSquareType())
            {
                previousType = SquareManager.instance.squareList[i, x].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[i, x]);
            }
            else if (previousType != SquareManager.instance.squareList[i, x].GetSquareType() && sameTypeSquares.Count < 3)
            {
                previousType = SquareManager.instance.squareList[i, x].GetSquareType();
                sameTypeSquares.Clear();
                sameTypeSquares.Add(SquareManager.instance.squareList[i, x]);
            }
            else if (previousType != SquareManager.instance.squareList[i, x].GetSquareType() && sameTypeSquares.Count >= 3)
            {
                CrackSameTypeSquares(sameTypeSquares, false);
                previousType = SquareManager.instance.squareList[i, x].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[i, x]);
            }
        }
        if (sameTypeSquares.Count >= 3)
        {
            CrackSameTypeSquares(sameTypeSquares,false);
        }
    }
    /// <summary>
    /// Adds same type squares to crackedSquares list then if its count above and equal 3 then triggers onCracked event.
    /// </summary>
    /// <param name="sameTypeSquares"></param>
    /// <param name="orientation"> false for "Vertical", true for "Horizontal" </param>

    void CrackSameTypeSquares(List<Square> sameTypeSquares, bool orientation)
    {
        int added = 0;
        foreach (var square in sameTypeSquares)
        {
            if (!SquareManager.instance.crackedSquares.Contains(square))
            {
                SquareManager.instance.crackedSquares.Add(square);
                added++;
            }
        }
        if(added == 3)
        {
            EventManager.instance.onCracked?.Invoke();
        }
        else if(added == 4)
        {
            SquareManager.instance.SpawnHorizontalOrVerticalSpecialSquare(orientation);
        }
        else if(added == 5)
        {
            SquareManager.instance.SpawnBombSpecialSquare();
        }
        else
        {
            SquareManager.instance.crackedSquares.Clear();
        }
        
        sameTypeSquares.Clear();
    }

   
}
