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
    [SerializeField]Square[,] squareList;
    public List<Square> crackedSquares = new List<Square>();
    public List<Square> spawnedSquares = new List<Square>();
 
    private void Start()
    {
        squareList = new Square[height, width];
        DOTween.SetTweensCapacity(100, 100);
        GetSquares();
        CheckAllSquares();
        EventManager.instance.onSpawned += SpawnAllCrackedSquares;
        EventManager.instance.onCracked += CrackAllCrackedSquares;
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

    [NaughtyAttributes.Button]
    private void GenerateSquares()
    {
        int previousIndex = -1;
        int nY = 0, nX=0;
        squareList = new Square[height, width];
        GameObject newSquare;
        for (int y = 0;y<height;y++)
        {
            for(int x = 0;x<width;x++)
            {
                int random = Random.Range(0,squarePrefab.Length);

                if (random != previousIndex && y != 0 && ((int)squareList[y-1,x].GetSquareType() != random ))
                {
                    newSquare = Instantiate(squarePrefab[random], new Vector3(spawnPosition.position.x + x, spawnPosition.position.y + y + 0.1f * y, 1), Quaternion.identity);
                    newSquare.gameObject.name = "x: " + x + " y: " + y;
                    squareList[y, x] = newSquare.GetComponent<Square>();
                    previousIndex = random;
                }
                else if(y == 0)
                {
                    newSquare = Instantiate(squarePrefab[random], new Vector3(spawnPosition.position.x + x, spawnPosition.position.y + y + 0.1f * y, 1), Quaternion.identity);
                    newSquare.gameObject.name = "x: " + x + " y: " + y;
                    squareList[y, x] = newSquare.GetComponent<Square>();
                }
                else
                {
                    x--;
                }
                
            }
        }
        camera.transform.position = new Vector3(height / 2 - 0.5f, width / 2 - 0.5f,-10f);
        GameManager.instance.ChangeGameState(GameState.Playing);
    }
    void CheckAllSquares()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (squareList[y,x] != null)
                squareList[y, x].CheckNeighboors();
            }
        }
        Debug.Log("Kontrol edildi");
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
            CheckAllSquares();
        }
        else
        {
            GameManager.instance.ChangeGameState(GameState.Playing);
        }
    }
    void CrackAllCrackedSquares()
    {
        if (crackedSquares.Count > 0)
        {
            GameManager.instance.ChangeGameState(GameState.Placement);
            foreach (var square in crackedSquares)
            {
                square.Crack();
                spawnedSquares.Add(square);
            }
            crackedSquares.Clear();
            CheckAllSquares();
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
        square.transform.DOScale(.75f, 0.25f).OnComplete(() => EventManager.instance.onSquareMoved?.Invoke());
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
        Square[] sqr = squares.GetComponentsInChildren<Square>();
        int i = 0;
        Debug.Log(sqr.Length);
        for (int y = 0; y < sqr.Length/5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                squareList[y,x] = sqr[i];
                i++;
            }
        }
    }
}
