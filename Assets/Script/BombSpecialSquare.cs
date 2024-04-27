using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombSpecialSquare : MonoBehaviour,SpecialSquare
{
    public Square currentSquare;
    public void DeSpawn()
    {
        currentSquare.gameObject.SetActive(true);
        currentSquare.specialSpawned = false;
        Destroy(gameObject);
    }

    public void DoSpecial(Square square)
    {
       
        if(!SquareManager.instance.squareList[square.y, square.x].IsUnityNull())
        {
            SquareManager.instance.squareList[square.y, square.x].Crack();
        }
        if (!SquareManager.instance.squareList[square.y+1, square.x].IsUnityNull())
        {
            SquareManager.instance.squareList[square.y+1, square.x].Crack();
        }
        if (!SquareManager.instance.squareList[square.y-1, square.x].IsUnityNull())
        {
            SquareManager.instance.squareList[square.y-1, square.x].Crack();
        }
        if (!SquareManager.instance.squareList[square.y, square.x+1].IsUnityNull())
        {
            SquareManager.instance.squareList[square.y, square.x+1].Crack();
        }
        if (!SquareManager.instance.squareList[square.y, square.x-1].IsUnityNull())
        {
            SquareManager.instance.squareList[square.y, square.x-1].Crack();
        }
        if (!SquareManager.instance.squareList[square.y-1, square.x-1].IsUnityNull())
        {
            SquareManager.instance.squareList[square.y-1, square.x-1].Crack();
        }
        if (!SquareManager.instance.squareList[square.y+1, square.x+1].IsUnityNull())
        {
            SquareManager.instance.squareList[square.y+1, square.x+1].Crack();
        }
        if (!SquareManager.instance.squareList[square.y + 1, square.x - 1].IsUnityNull())
        {
            SquareManager.instance.squareList[square.y + 1, square.x - 1].Crack();
        }
        if (!SquareManager.instance.squareList[square.y - 1, square.x + 1].IsUnityNull())
        {
            SquareManager.instance.squareList[square.y - 1, square.x + 1].Crack();
        }
        DeSpawn();
    }

    public void Spawn(Square newSquare)
    {
        if (newSquare.specialSpawned)
            return;
        currentSquare = newSquare;
        Instantiate(this, newSquare.transform.position, Quaternion.identity);
        newSquare.gameObject.SetActive(false);
        newSquare.specialSpawned = true;
    }
    private void OnMouseDown()
    {
        DoSpecial(currentSquare);
    }
}
