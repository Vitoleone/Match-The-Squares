using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalSpecialSquare : MonoBehaviour, SpecialSquare
{
    public Square currentSquare;
    public void DeSpawn()
    {
        currentSquare.gameObject.SetActive(true);
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
        currentSquare = newSquare;
        Instantiate(this,newSquare.transform.position,Quaternion.identity);
        newSquare.gameObject.SetActive(false);
    }
    private void OnMouseDown()
    {
        DoSpecial(currentSquare);
    }

}
