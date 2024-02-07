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
    

    private void Start()
    {
        // VSync'i devre d��� b�rak
        QualitySettings.vSyncCount = 0;

        // FPS de�erini 60 olarak sabitle
        Application.targetFrameRate = 60;

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
            selectedSquares[1].transform.DOMove(square1, 0.2f).OnComplete(() => EventManager.instance.onSquareMoved?.Invoke());
        }
        ResetSelectedSquares();
    }
    IEnumerator SpawnCrackedSquares()
    {
        while (true)
        {
            yield return new WaitForSeconds(.35f);
            EventManager.instance.onSpawned?.Invoke();
        }
    }
    IEnumerator CrackSquares()
    {
        while (true)
        {
            yield return new WaitForSeconds(.35f);
            EventManager.instance.onCracked?.Invoke();
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
