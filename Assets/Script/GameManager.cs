using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState gameState = GameState.Placement;
    public bool isSquareSelected;
    public Square[] selectedSquares;
    public Material[] materials;
    public bool canCheckNeighboor = false;
    

    private void Start()
    {

        selectedSquares = new Square[2];
        EventManager.instance.onSecondSquareSelected += ChangeSelected;
        StartCoroutine(SpawnCrackedSquares());
        StartCoroutine(CrackSquares());
    }


    private void OnDestroy()
    {
        EventManager.instance.onSecondSquareSelected -= ChangeSelected;
    }
    private void ChangeSelected()
    {
        Vector3 square1 = new Vector3(selectedSquares[0].gameObject.transform.position.x, selectedSquares[0].gameObject.transform.position.y,1);
        Vector3 square2 = new Vector3(selectedSquares[1].gameObject.transform.position.x, selectedSquares[1].gameObject.transform.position.y, 1);
        if (Vector2.Distance(square1,square2) <= 1.2f)
        {
            selectedSquares[0].transform.DOMove(square2, 0.2f);
            selectedSquares[1].transform.DOMove(square1, 0.2f).onComplete = (() => {

                ControlSelectedSquares(selectedSquares[0], selectedSquares[1]);
                ResetSelectedSquares();
            }) ;
        }
        
    }
    public void ControlSelectedSquares(Square square1, Square square2)
    {
        
        Square holder;
        int xHolder, yHolder;
        holder = SquareManager.instance.squareList[square1.y, square1.x];
        SquareManager.instance.squareList[square1.y, square1.x] = SquareManager.instance.squareList[square2.y, square2.x];
        SquareManager.instance.squareList[square2.y, square2.x] = SquareManager.instance.squareList[holder.y, holder.x];
        xHolder = square1.x;
        yHolder = square1.y;
        square1.x = square2.x;
        square1.y = square2.y;
        square2.x = xHolder;
        square2.y = yHolder;
        square1.CheckNeighboors();
        square2.CheckNeighboors();
    }
    IEnumerator SpawnCrackedSquares()
    {
        while (true)
        {
            yield return new WaitForSeconds(.35f);
            if(gameState == GameState.Breaking)
            {
                EventManager.instance.onSpawned?.Invoke();
            }
            
        }
    }
    IEnumerator CrackSquares()
    {
        while (true)
        {
            yield return new WaitForSeconds(.35f);
            if (gameState == GameState.Playing)
            {
                EventManager.instance.onCracked?.Invoke();
            }
            
        }
    }
    private void ResetSelectedSquares()
    {
        selectedSquares = new Square[2];
        isSquareSelected = false;
    }
    public void ChangeGameState(GameState newState)
    {
        gameState = newState;
        UIManager.instance.ChangeStateText(gameState);
    }
   
}
