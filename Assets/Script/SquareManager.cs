using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour
{
   public static SquareManager instance;
   [SerializeField]private GameObject[] squarePrefab;
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField]List<Square> squareList;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GenerateSquares();
    }

    [NaughtyAttributes.Button]
    private void GenerateSquares()
    {
        GameObject newSquare;
        for (int y = 0;y<height;y++)
        {
            for(int x = 0;x<width;x++)
            {
                int random = Random.Range(0,squarePrefab.Length);
                newSquare = Instantiate(squarePrefab[random], new Vector3(x + 0.3f*x,y + 0.3f*y,1),Quaternion.identity);
                newSquare.gameObject.name = "x: " + x + " y: " + y;
                squareList.Add(newSquare.GetComponent<Square>());
            }
        }
        Camera.main.transform.position = new Vector3(height / 2 - 0.5f, width / 2 - 0.5f,0);
        GameManager.instance.ChangeGameState(GameState.Playing);
    }

}
