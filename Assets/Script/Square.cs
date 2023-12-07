using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Square : MonoBehaviour
{
    RaycastHit2D leftHit, rightHit, upHit, downHit;
    [SerializeField]Square[] neighboors;
    [SerializeField]private SquareType type;
    [SerializeField] private int sameTypeCount = 1;
    private void Start()
    {
        CheckNeighboors();
    }
    private void FixedUpdate()
    {
        if(GameManager.instance.gameState == GameState.Placement)
        {
            CheckNeighboors();
        }
    }

    public void CheckNeighboors()
    {
        sameTypeCount = 1;
        neighboors = new Square[4];
        leftHit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x - 0.8f, gameObject.transform.position.y), -transform.right);
        rightHit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + 0.8f, gameObject.transform.position.y), transform.right);
        upHit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.8f), transform.up);
        downHit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.8f), -transform.up);

        if(leftHit.collider != null && leftHit.transform.gameObject != gameObject)
        {
            neighboors[0] = leftHit.transform.gameObject.GetComponent<Square>();
        }
        if (rightHit.collider != null && rightHit.transform.gameObject != gameObject)
        {
            neighboors[1] = rightHit.transform.gameObject.GetComponent<Square>();
        }
        if (upHit.collider != null && upHit.transform.gameObject != gameObject)
        {
            neighboors[2] = upHit.transform.gameObject.GetComponent<Square>();
        }
        if (downHit.collider != null && downHit.transform.gameObject != gameObject)
        {
            neighboors[3] = downHit.transform.gameObject.GetComponent<Square>();
        }
        CheckSameTypeNeighboor();
    }
    private void CheckSameTypeNeighboor()
    {
        if (!neighboors[0].IsUnityNull() && !neighboors[1].IsUnityNull())
        {
            if (neighboors[0].type == this.type && neighboors[1].type == this.type)
            {
                neighboors[0].Crack();
                neighboors[1].Crack();
                Crack();
            }
        }
        if (!neighboors[2].IsUnityNull() && !neighboors[3].IsUnityNull())
        {
            if (neighboors[2].type == this.type && neighboors[3].type == this.type)
            {
                neighboors[2].Crack();
                neighboors[3].Crack();
                Crack();
            }
        }
    }
    private void OnMouseDown()
    {
        SelectSquare();
    }
    void SelectSquare()
    {
        if(!GameManager.instance.isSquareSelected)
        {
            GameManager.instance.isSquareSelected = true;
            GameManager.instance.selectedSquares[0] = this;
        }
        else
        {
            GameManager.instance.selectedSquares[1] = this;
            EventManager.instance.onSecondSquareSelected?.Invoke();
        }
    }
    public SquareType GetSquareType()
    {
        return type;
    }
    public void Crack()
    {
        gameObject.SetActive(false);
    }




}
