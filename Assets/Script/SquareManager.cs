using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SquareManager : Singleton<SquareManager>
{
    [Header("Square Attributes")]
    public List<Square> allSquares;
    public List<Square> crackedSquares = new List<Square>();
    public List<Square> spawnedSquares = new List<Square>();
    [SerializeField]private GameObject[] squarePrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField]public Square[,] squareList;

    Dictionary<SquareType, SpriteRenderer> SquareTypeAndRendererDictionary = new Dictionary<SquareType, SpriteRenderer>();
 
    private void Start()
    {
        squareList = new Square[height, width];
        DOTween.SetTweensCapacity(100, 100);
        GetSquares();

        EventManager.instance.onSpawned += Spawn;
        EventManager.instance.onCracked += CrackAllCrackedSquares;
        SetDictionary();
    }
    /// <summary>
    /// Selects given square and checks if it is first or second selected square with isSquareSelected. If it is second triggers onSecondSquareSelected event.
    /// </summary>
    /// <param name="selectedSquare"></param>
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
    /// <summary>
    /// Crack all squares which is in crackedSquares list. Change gamestate to breaking in that operation, after finish assign it to playing.
    /// </summary>
    void CrackAllCrackedSquares()
    {
        if (crackedSquares.Count >= 3)
        {
            GameManager.instance.ChangeGameState(GameState.Breaking);
            foreach (var square in crackedSquares)
            {
                square.Crack();
                spawnedSquares.Add(square);
            }
            crackedSquares.Clear();
            GameManager.instance.ChangeGameState(GameState.Playing);
        }

    }
    /// <summary>
    /// Spawn given square with random values and checks if any posible cracks or not.
    /// </summary>
    /// <param name="square"></param>
    public void Spawn(Square square)
    {
        square.gameObject.SetActive(true);
        ChangeSquareTypeRandom(square);
        square.transform.DOScale(.75f, 0.25f).OnComplete(() =>
        {
            EventManager.instance.onCrackControll?.Invoke(square,null);
        });
        
    }
    /// <summary>
    /// Changes give square type, renderer material and particle material to random value.
    /// </summary>
    /// <param name="square"></param>
    void ChangeSquareTypeRandom(Square square)
    {
        int randomNumber = Random.Range(0, squarePrefab.Length);

        square.ChangeSquareType(squarePrefab[randomNumber].GetComponent<Square>().GetSquareType());
        square.ChangeCrackParticleMaterial(squarePrefab[randomNumber].gameObject.GetComponent<Square>().crackedEffect.GetComponent<ParticleSystemRenderer>());
        square.gameObject.GetComponent<SpriteRenderer>().sharedMaterial = SquareTypeAndRendererDictionary[square.GetSquareType()].sharedMaterial;
    }

    /// <summary>
    /// Sets dictionary keys and values.
    /// </summary>
    void SetDictionary()
    {
        foreach (var prefab in squarePrefab)
        {
            Square square = prefab.GetComponent<Square>();
            SquareTypeAndRendererDictionary.Add(square.GetSquareType(), prefab.GetComponent<SpriteRenderer>());
        }
    }
   
    /// <summary>
    /// Assing list values to two dimension array which name is squareList.
    /// </summary>
    public void GetSquares()
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
