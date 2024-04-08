using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackController : MonoBehaviour
{
    private void Start()
    {
        EventManager.instance.onCrackControll += CrackCheck;
    }
    /// <summary>
    /// Checks the square table if it has squares that can be cracked or not.
    /// </summary>
    void CrackCheck(Square square1, Square square2)
    {
        int x1 = square1.x;
        int y1 = square1.y;

        int x2 = square2.x;
        int y2 = square2.y;

        if(x1 == x2)
        {
            CheckVertical(x1);
            CheckHorizontally(y1);
            CheckHorizontally(y2);
        }
        else if(y1 == y2)
        {
            CheckHorizontally(y1);
            CheckVertical(x1);
            CheckVertical(x2);
        }
    }
    private void CheckHorizontally(int y)
    {
        SquareType previousType = SquareType.None;
        List<Square> sameTypeSquares = new List<Square>();
        for(int i = 0; i < SquareManager.instance.squareList.GetLength(1); i++)
        {
            if(previousType == SquareType.None)
            {
                previousType = SquareManager.instance.squareList[y, i].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[y, i]);
            }
            else if(previousType == SquareManager.instance.squareList[y,i].GetSquareType())
            {
                previousType = SquareManager.instance.squareList[y, i].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[y, i]);
            }
            else if(previousType != SquareManager.instance.squareList[y, i].GetSquareType() && sameTypeSquares.Count < 3)
            {
                previousType = SquareManager.instance.squareList[y, i].GetSquareType();
                sameTypeSquares.Clear();
                sameTypeSquares.Add(SquareManager.instance.squareList[y, i]);
            }
            else if (previousType != SquareManager.instance.squareList[y, i].GetSquareType() && sameTypeSquares.Count >= 3)
            {
                CrackSameTypeSquares(sameTypeSquares);
                previousType = SquareManager.instance.squareList[y, i].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[y, i]);
            }
            //Debug.Log("X: " + SquareManager.instance.squareList[y, i].x + " Y: " + SquareManager.instance.squareList[y, i].y);
            Debug.Log(SquareManager.instance.squareList[y, i].GetSquareType() + " " +SquareManager.instance.squareList[y, i].name);
        }
        if (sameTypeSquares.Count >= 3)
        {
            CrackSameTypeSquares(sameTypeSquares);
        }
        Debug.Log("Horizontal");
    }
    private void CheckVertical(int x)
    {
        SquareType previousType = SquareType.None;
        List<Square> sameTypeSquares = new List<Square>();
        for (int i = 0; i < SquareManager.instance.squareList.GetLength(0); i++)
        {
            if (previousType == SquareType.None)
            {
                previousType = SquareManager.instance.squareList[i, x].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[i,x]);
            }
            else if (previousType == SquareManager.instance.squareList[i, x].GetSquareType())
            {
                previousType = SquareManager.instance.squareList[i, x].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[i, x]);
            }
            else if(previousType != SquareManager.instance.squareList[i, x].GetSquareType() && sameTypeSquares.Count < 3)
            {
                previousType = SquareManager.instance.squareList[i, x].GetSquareType();
                sameTypeSquares.Clear();
                sameTypeSquares.Add(SquareManager.instance.squareList[i,x]);
            }
            else if (previousType != SquareManager.instance.squareList[i, x].GetSquareType() && sameTypeSquares.Count >= 3)
            {
                CrackSameTypeSquares(sameTypeSquares);
                previousType = SquareManager.instance.squareList[i, x].GetSquareType();
                sameTypeSquares.Add(SquareManager.instance.squareList[i, x]);
            }
            if(sameTypeSquares.Count >= 3)
            {
                CrackSameTypeSquares(sameTypeSquares);
            }
            //Debug.Log("X: " + SquareManager.instance.squareList[i, x].x + " Y: " + SquareManager.instance.squareList[i, x].y);
        }
        Debug.Log("Vertical");
        
    }
    void CrackSameTypeSquares(List<Square> sameTypeSquares)
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
        if(added >= 3)
        {
            EventManager.instance.onCracked?.Invoke();
        }
        else
        {
            SquareManager.instance.crackedSquares.Clear();
        }
        
        sameTypeSquares.Clear();
    }
}
