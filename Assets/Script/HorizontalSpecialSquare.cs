using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalSpecialSquare : MonoBehaviour, SpecialSquare
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
        for (int i = 0; i < SquareManager.instance.squareList.GetLength(1); i++)
        {
            SquareManager.instance.squareList[square.y, i].Crack();
        }
        DeSpawn();
    }

    public void Spawn(Square newSquare)
    {
        if (newSquare.specialSpawned)
            return;
        currentSquare = newSquare;
        Instantiate(this,newSquare.transform.position,Quaternion.identity);
        newSquare.gameObject.SetActive(false);
        newSquare.specialSpawned = true;
    }
    private void OnMouseDown()
    {
        DoSpecial(currentSquare);
    }

}
