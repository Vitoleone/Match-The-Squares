using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Square : MonoBehaviour
{
    RaycastHit2D leftHit, rightHit, upHit, downHit;
    [SerializeField]Square[] neighboors;
    [SerializeField]private SquareType type;
    [SerializeField] private ParticleSystem crackedEffect;
    Tween crackTween;
    bool spawned = true;
    private void Start()
    {
        

        CheckNeighboors();
        CheckSameTypeNeighboor();
        spawned = true;
        EventManager.instance.onSquareMoved += CheckNeighboors;
        EventManager.instance.onSquareMoved += CheckSameTypeNeighboor;
        EventManager.instance.onGetScore += GetScoreValueByType;
        DOTween.SetTweensCapacity(100,100);
    }
    private void OnEnable()
    {
        if (EventManager.instance != null)
        {
            spawned = true;
            EventManager.instance.onSquareMoved += CheckNeighboors;
            EventManager.instance.onSquareMoved += CheckSameTypeNeighboor;
            EventManager.instance.onGetScore += GetScoreValueByType;
        }
        
    }

    private void OnDisable()
    {
        EventManager.instance.onSquareMoved -= CheckNeighboors;
        EventManager.instance.onSquareMoved -= CheckSameTypeNeighboor;
        EventManager.instance.onGetScore -= GetScoreValueByType;
    }
    private void OnMouseDown()
    {
        if(GameManager.instance.gameState == GameState.Playing)
        {
            SquareManager.instance.SelectSquare(this);
        }
    }
   
    private void OnDrawGizmos()
    {
        Debug.DrawRay(new Vector2(gameObject.transform.position.x - 0.4f, gameObject.transform.position.y), -transform.right * 0.5f);
        Debug.DrawRay(new Vector2(gameObject.transform.position.x + 0.4f, gameObject.transform.position.y), transform.right * 0.5f);
        Debug.DrawRay(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.4f), transform.up * 0.5f);
        Debug.DrawRay(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.4f), -transform.up * 0.5f);
    }
    [NaughtyAttributes.Button]
    public void CheckNeighboors()
    {
        neighboors = new Square[4];
        leftHit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x - 0.4f, gameObject.transform.position.y), -transform.right*0.5f, 0.4f);
        rightHit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + 0.4f, gameObject.transform.position.y), transform.right * 0.5f,0.4f);
        upHit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.4f), transform.up * 0.5f, 0.4f);
        downHit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.4f), -transform.up * 0.5f, 0.4f);

        if(leftHit.collider != null && leftHit.collider.gameObject.GetComponent<Square>().spawned)
        {
            neighboors[0] = leftHit.transform.gameObject.GetComponent<Square>();
        }
        if (rightHit.collider != null && rightHit.collider.gameObject.GetComponent<Square>().spawned)
        {
            neighboors[1] = rightHit.transform.gameObject.GetComponent<Square>();
        }
        if (upHit.collider != null && upHit.collider.gameObject.GetComponent<Square>().spawned)
        {
            neighboors[2] = upHit.transform.gameObject.GetComponent<Square>();
        }
        if (downHit.collider != null && downHit.collider.gameObject.GetComponent<Square>().spawned)
        {
            neighboors[3] = downHit.transform.gameObject.GetComponent<Square>();
        }
    }
    public void CheckSameTypeNeighboor()
    {
        if (!neighboors[0].IsUnityNull() && !neighboors[1].IsUnityNull())
        {
            if (neighboors[0].type == this.type && neighboors[1].type == this.type)
            {
                SquareManager.instance.crackedSquares.Add(this);
                SquareManager.instance.crackedSquares.Add(neighboors[0]);
                SquareManager.instance.crackedSquares.Add(neighboors[1]);
            }
        }
        if (!neighboors[2].IsUnityNull() && !neighboors[3].IsUnityNull())
        {
            if (neighboors[2].type == this.type && neighboors[3].type == this.type)
            {
                SquareManager.instance.crackedSquares.Add(this);
                SquareManager.instance.crackedSquares.Add(neighboors[3]);
                SquareManager.instance.crackedSquares.Add(neighboors[2]);
            }
        }
        
    }

    public SquareType GetSquareType()
    {
        return type;
    }
    public void ChangeSquareType(SquareType newType)
    {
        type = newType;
    }
    public void Crack()
    {
        GameManager.instance.ChangeGameState(GameState.Breaking);
        UIManager.instance.AddScore(EventManager.instance.onGetScore, this.type);
        crackedEffect.Play();
        if(crackTween != null)
        {
            crackTween.Restart();
        }
        else
        {
            crackTween = transform.DOScale(.3f, .25f).OnComplete(() =>
            {
                //Do particles and deactive
                spawned = false;
                //crackedEffect.SetActive(false);
                gameObject.SetActive(false);

            })
            .SetAutoKill(false)
            .SetRecyclable(true)
            .Pause();
        }
        
    }
    public ParticleSystemRenderer GetParticleRenderer()
    {
        return crackedEffect.GetComponent<ParticleSystemRenderer>();
    }
    public void ChangeCrackParticleMaterial(ParticleSystemRenderer particle )
    {
        crackedEffect.GetComponent<ParticleSystemRenderer>().sharedMaterial = particle.sharedMaterial;
    }
    int GetScoreValueByType(SquareType type)
    {
        return (((int)type) + 1) * 5;
    }

}
