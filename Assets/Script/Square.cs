using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Square : MonoBehaviour
{
    RaycastHit2D leftHit, rightHit, upHit, downHit;
    [SerializeField]Square[] neighboors;
    [SerializeField]private SquareType type;
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

    void CheckNeighboors()
    {
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
        Debug.Log("çalýþtý");
    }
    public SquareType GetSquareType()
    {
        return type;
    }




}
