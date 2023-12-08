using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour
{
   public static SquareManager instance;
   [SerializeField]private GameObject[] squarePrefab;
   [SerializeField]private Transform spawnPosition;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField]Square[,] squareList;
    public List<Square> crackedSquares = new List<Square>();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GenerateSquares();
        CheckAllSquares();
        EventManager.instance.onCracked += CheckAllSquares;
        EventManager.instance.onSpawned += SpawnAllCrackedSquares;
    }

    [NaughtyAttributes.Button]
    private void GenerateSquares()
    {
        squareList = new Square[height, width];
        GameObject newSquare;
        for (int y = 0;y<height;y++)
        {
            for(int x = 0;x<width;x++)
            {
                int random = Random.Range(0,squarePrefab.Length);
                newSquare = Instantiate(squarePrefab[random], new Vector3(spawnPosition.position.x + x, spawnPosition.position.y + y + 0.1f*y,1),Quaternion.identity);
                newSquare.gameObject.name = "x: " + x + " y: " + y;
                squareList[y,x] = newSquare.GetComponent<Square>();
            }
        }
        Camera.main.transform.position = new Vector3(height / 2 - 0.5f, width / 2 - 0.5f,0);
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
        Debug.Log(crackedSquares.Count);
        if(crackedSquares.Count > 0)
        {
            foreach (var square in crackedSquares)
            {
                Spawn(square);
            }
            crackedSquares.Clear();
        }
        
    }
    void Spawn(Square square)
    {
        square.gameObject.SetActive(true);
        ChangeSquareTypeRandom(square);
        square.transform.DOScale(.75f, 0.25f).OnComplete(() => EventManager.instance.onSquareMoved?.Invoke());
    }
    void ChangeSquareTypeRandom(Square square)
    {
        int randomNumber = Random.Range(0, squarePrefab.Length);
        square.ChangeSquareType(squarePrefab[randomNumber].GetComponent<Square>().GetSquareType());
        square.gameObject.GetComponent<SpriteRenderer>().color = squarePrefab[randomNumber].gameObject.GetComponent<SpriteRenderer>().color;
    }

}
