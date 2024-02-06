using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Square : MonoBehaviour
{
    RaycastHit2D leftHit, rightHit, upHit, downHit;
    [SerializeField]Square[] neighboors;
    [SerializeField]private SquareType type;
    [SerializeField] private GameObject crackedEffect;
    Tween crackTween;
    bool spawned = true;
    private void Start()
    {
        EventManager.instance.onSquareMoved += CheckNeighboors;
        EventManager.instance.onSquareMoved += CheckSameTypeNeighboor;
       
        CheckNeighboors();
        CheckSameTypeNeighboor();
    }
    private void OnEnable()
    {
        spawned = true;
        EventManager.instance.onSquareMoved += CheckNeighboors;
        EventManager.instance.onSquareMoved += CheckSameTypeNeighboor;
    }

    private void OnDisable()
    {
        EventManager.instance.onSquareMoved -= CheckNeighboors;
        EventManager.instance.onSquareMoved -= CheckSameTypeNeighboor;
    }
    private void OnMouseDown()
    {
        SquareManager.instance.SelectSquare(this);
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
        
        GameManager.instance.ChangeGameState(GameState.Playing);
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
        crackedEffect.SetActive(true);
        crackTween = transform.DOScale(.3f, .25f).OnComplete(() =>
        {
            //Do particles and deactive
            
            EventManager.instance.onCracked?.Invoke();
            spawned = false;
            SquareManager.instance.crackedSquares.Add(this);
            crackedEffect.SetActive(false);
            crackTween.Kill();
            gameObject.SetActive(false);

        });
    }
    public ParticleSystemRenderer GetParticleRenderer()
    {
        return crackedEffect.GetComponent<ParticleSystemRenderer>();
    }
    public void ChangeCrackParticleMaterial(ParticleSystemRenderer particle )
    {
        crackedEffect.GetComponent<ParticleSystemRenderer>().sharedMaterial = particle.sharedMaterial;
    }

}
