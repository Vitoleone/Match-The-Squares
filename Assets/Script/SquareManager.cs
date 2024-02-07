using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : Singleton<SquareManager>
{
   [SerializeField]private GameObject[] squarePrefab;
   [SerializeField]private Camera camera;
   [SerializeField]private Transform spawnPosition;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField]Square[,] squareList;
    public List<Square> crackedSquares = new List<Square>();
    private void Start()
    {
        GenerateSquares();
        CheckAllSquares();
        EventManager.instance.onCracked += CheckAllSquares;
        EventManager.instance.onSpawned += SpawnAllCrackedSquares;
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
    }
    void SpawnAllCrackedSquares()
    {
        if(crackedSquares.Count > 0)
        {
            GameManager.instance.ChangeGameState(GameState.Placement);
            foreach (var square in crackedSquares)
            {
                Spawn(square);
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
        square.transform.DOScale(.75f, 0.35f).OnComplete(() => EventManager.instance.onSquareMoved?.Invoke());
    }
    void ChangeSquareTypeRandom(Square square)
    {
        int randomNumber = Random.Range(0, squarePrefab.Length);
        square.ChangeSquareType(squarePrefab[randomNumber].GetComponent<Square>().GetSquareType());
        square.ChangeCrackParticleMaterial(squarePrefab[randomNumber].gameObject.GetComponent<Square>().GetParticleRenderer());
        square.gameObject.GetComponent<SpriteRenderer>().sharedMaterial = squarePrefab[randomNumber].gameObject.GetComponent<SpriteRenderer>().sharedMaterial;
    }
    
}
