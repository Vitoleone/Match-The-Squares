using DG.Tweening;
using System;
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
    public int x, y;
    private void Start()
    {
        spawned = true;
        EventManager.instance.onGetScore += GetScoreValueByType;
    }
    private void OnEnable()
    {
        if (EventManager.instance != null)
        {
            spawned = true;
            EventManager.instance.onGetScore += GetScoreValueByType;
        }
        
    }

    private void OnDisable()
    {
        EventManager.instance.onGetScore -= GetScoreValueByType;
    }
    private void OnMouseDown()
    {
        if(GameManager.instance.gameState == GameState.Playing)
        {
            SquareManager.instance.SelectSquare(this);
        }
    }
   
    [NaughtyAttributes.Button]
    public void CheckNeighboors()
    {
        neighboors = new Square[4];
        int xLength = SquareManager.instance.squareList.GetLength(1);
        int yLength = SquareManager.instance.squareList.GetLength(0);

        neighboors[0] = x - 1 <= xLength && x-1 >= 0 ? SquareManager.instance.squareList[y, x - 1]: null;
        neighboors[1] = x + 1 <= xLength ? SquareManager.instance.squareList[y, x + 1]: null;
        neighboors[2] = y - 1 <= yLength && y - 1 >= 0 ? SquareManager.instance.squareList[y - 1, x]: null;
        neighboors[3] = y + 1 <= yLength ? SquareManager.instance.squareList[y + 1, x]: null;
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
        Debug.Log(gameObject + " Cracked");
        SquareManager.instance.spawnedSquares.Add(this);
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
    public void ChangeSquareXYValues(int x, int y)
    {
        this.x = x; 
        this.y = y;
    }

}
