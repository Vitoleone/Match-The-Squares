using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("General Attributes")]
    public GameState gameState = GameState.Placement;

    [Header("Select Attributes")]
    public bool isSquareSelected;
    public Square[] selectedSquares;
    public float movingSpeed = 0.2f;
    public float selectDistance = 1.2f;

 
    private void Start()
    {
        selectedSquares = new Square[2];
        EventManager.instance.onSecondSquareSelected += ChangeSelected;
    }
    private void OnDestroy()
    {
        EventManager.instance.onSecondSquareSelected -= ChangeSelected;
    }
    /// <summary>
    /// Swaps the positions of the two squares between themselves and resets selected squares.
    /// </summary>
    private void ChangeSelected()
    {
        Vector3 square1 = new Vector3(selectedSquares[0].gameObject.transform.position.x, selectedSquares[0].gameObject.transform.position.y,1);
        Vector3 square2 = new Vector3(selectedSquares[1].gameObject.transform.position.x, selectedSquares[1].gameObject.transform.position.y, 1);
        if (Vector2.Distance(square1,square2) <= selectDistance)
        {
            
            selectedSquares[0].transform.DOMove(square2, movingSpeed);
            selectedSquares[1].transform.DOMove(square1, movingSpeed);
            SwapSelectedSquaresValues(selectedSquares[0], selectedSquares[1]);
            ResetSelectedSquares();
        }
    }
    /// <summary>
    /// Swaps squares values and indexes and checks if any posible cracks or not.
    /// </summary>
    /// <param name="square1"></param>
    /// <param name="square2"></param>
    public void SwapSelectedSquaresValues(Square square1, Square square2)
    {
        int tempX = square1.x;
        int tempY = square1.y;
        Square tempSquare = SquareManager.instance.squareList[square1.y, square1.x];

        //change square1 and square2's values each other.
        (SquareManager.instance.squareList[square1.y, square1.x], SquareManager.instance.squareList[square2.y, square2.x]) = (SquareManager.instance.squareList[square2.y, square2.x], SquareManager.instance.squareList[square1.y, square1.x]);

        square1.ChangeSquareXYValues(square2.x, square2.y);
        square2.ChangeSquareXYValues(tempX, tempY);
        EventManager.instance.onCrackControll?.Invoke(square1, square2);
    }
    /// <summary>
    /// Resets selectedSquares array.
    /// </summary>
    private void ResetSelectedSquares()
    {
        selectedSquares = new Square[2];
        isSquareSelected = false;
    }
    /// <summary>
    /// Change GameState to wanted state.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeGameState(GameState newState)
    {
        gameState = newState;
        UIManager.instance.ChangeStateText(gameState);
    }
   
}
