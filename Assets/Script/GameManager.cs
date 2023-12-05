using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState = GameState.Placement;
    public bool isSquareSelected;
    public Square[] selectedSquares;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
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

        selectedSquares[0].transform.DOMove(square2, 0.2f);
        selectedSquares[1].transform.DOMove(square1, 0.2f);
        ResetSelectedSquares();
    }
    private void ResetSelectedSquares()
    {
        selectedSquares = new Square[2];
        isSquareSelected = false;
    }
    public void ChangeGameState(GameState newState)
    {
        gameState = newState;
    }
}
