using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SquareManager : Singleton<SquareManager>
{
   [SerializeField]private GameObject[] squarePrefab;
    [SerializeField] GameObject squares;
   [SerializeField]private Camera camera;
   [SerializeField]private Transform spawnPosition;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField]public Square[,] squareList;
    public List<Square> allSquares;
    public List<Square> crackedSquares = new List<Square>();
    public List<Square> spawnedSquares = new List<Square>();
 
    private void Start()
    {
        squareList = new Square[height, width];
        DOTween.SetTweensCapacity(100, 100);
        GetSquares();

        EventManager.instance.onSpawned += SpawnAllCrackedSquares;
        EventManager.instance.onCracked += CrackAllCrackedSquares;
        EventManager.instance.onSquareMoved += GameManager.instance.ControlSelectedSquares;
    }
    public void SelectSquare(Square selectedSquare)
    {
        if (!GameManager.instance.isSquareSelected)
        {
            GameManager.instance.isSquareSelected = true;
            GameManager.instance.selectedSquares[0] = selectedSquare;
        }
        else
        {
            GameManager.instance.selectedSquares[1] = selectedSquare;
            EventManager.instance.onSecondSquareSelected?.Invoke();
        }
    }
    void SpawnAllCrackedSquares()
    {
        if(spawnedSquares.Count > 0)
        {
            GameManager.instance.ChangeGameState(GameState.Placement);
            foreach (var square in spawnedSquares)
            {
                Spawn(square);
            }
            spawnedSquares.Clear();
        }
        GameManager.instance.ChangeGameState(GameState.Playing);
    }
    void CrackAllCrackedSquares()
    {
        if (crackedSquares.Count > 0)
        {
            GameManager.instance.ChangeGameState(GameState.Breaking);
            foreach (var square in crackedSquares)
            {
                square.Crack();
                spawnedSquares.Add(square);
            }
            crackedSquares.Clear();
        }
        else
        {
            GameManager.instance.ChangeGameState(GameState.Playing);
        }
    }
    void Spawn(Square square)
    {
        ChangeSquareTypeRandom(square);
        square.gameObject.SetActive(true);
        square.transform.DOScale(.75f, 0.25f).onComplete = () =>
        {
            square.CheckNeighboors();
        };
    }
    void ChangeSquareTypeRandom(Square square)
    {
        int randomNumber = Random.Range(0, squarePrefab.Length);
        square.ChangeSquareType(squarePrefab[randomNumber].GetComponent<Square>().GetSquareType());
        square.ChangeCrackParticleMaterial(squarePrefab[randomNumber].gameObject.GetComponent<Square>().GetParticleRenderer());
        square.gameObject.GetComponent<SpriteRenderer>().sharedMaterial = squarePrefab[randomNumber].gameObject.GetComponent<SpriteRenderer>().sharedMaterial;
    }

    void GetSquares()
    {
        Square[] sqr = allSquares.ToArray();
        int i = 0;
        for (int y = 0; y < sqr.Length/5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                squareList[y,x] = sqr[i];
                squareList[y, x].x = x;
                squareList[y, x].y = y;
                i++;
            }
        }
    }
}
