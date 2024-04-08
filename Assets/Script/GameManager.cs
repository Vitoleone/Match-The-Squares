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
    bool canSpawn = false;

    

    private void Start()
    {

        selectedSquares = new Square[2];
        EventManager.instance.onSecondSquareSelected += ChangeSelected;
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
            selectedSquares[1].transform.DOMove(square1, 0.2f);
            ControlSelectedSquares(selectedSquares[0], selectedSquares[1]);
            ResetSelectedSquares();
            
        }
        
    }
    public void ControlSelectedSquares(Square square1, Square square2)
    {
        Debug.Log("Before: " + SquareManager.instance.squareList[square1.y, square1.x]);
        Debug.Log("Before2: " + SquareManager.instance.squareList[square2.y, square2.x]);
        int tempX = square1.x;
        int tempY = square1.y;
        Square tempSquare = SquareManager.instance.squareList[square1.y, square1.x];

        (SquareManager.instance.squareList[square1.y, square1.x], SquareManager.instance.squareList[square2.y, square2.x]) = (SquareManager.instance.squareList[square2.y, square2.x], SquareManager.instance.squareList[square1.y, square1.x]);

        square1.ChangeSquareXYValues(square2.x, square2.y);
        square2.ChangeSquareXYValues(tempX, tempY);
        //SquareManager.instance.squareList[square1.y, square1.x] = SquareManager.instance.squareList[square2.y, square2.x];
        //SquareManager.instance.squareList[square2.y, square2.x] = SquareManager.instance.squareList[tempSquare.y, tempSquare.x];

        EventManager.instance.onCrackControll?.Invoke(square1, square2);

        Debug.Log("After: " + SquareManager.instance.squareList[square1.y, square1.x]);
        Debug.Log("After2: " + SquareManager.instance.squareList[square2.y, square2.x]);
        //square1.CheckNeighboors();
        //square2.CheckNeighboors();
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
