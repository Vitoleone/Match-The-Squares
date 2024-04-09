using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Square : MonoBehaviour
{
    [Header("General Attributes")]
    [SerializeField]private SquareType type;
    [SerializeField] public ParticleSystem crackedEffect;
 
    public int x, y;
    private void Start()
    {
        
        EventManager.instance.onGetScore += GetScoreValueByType;
    }
    private void OnEnable()
    {
        if (EventManager.instance != null)
        {
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
    /// <summary>
    /// Returns square type.
    /// </summary>
    /// <returns></returns>
    public SquareType GetSquareType()
    {
        return type;
    }
    /// <summary>
    /// Changes square's type to newType.
    /// </summary>
    /// <param name="newType"></param>
    public void ChangeSquareType(SquareType newType)
    {
        type = newType;
    }
    /// <summary>
    /// Play cracked effect particle. Do a scale tween  and when tween is completed trigger onSpawnedFunction.
    /// </summary>
    public void Crack()
    {
        SquareManager.instance.spawnedSquares.Add(this);
        GameManager.instance.ChangeGameState(GameState.Breaking);
        UIManager.instance.AddScore(EventManager.instance.onGetScore, this.type);
        crackedEffect.Play();
        transform.DOScale(.3f, .5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            EventManager.instance.onSpawned?.Invoke(this);

        }).SetAutoKill(true);

    }
    /// <summary>
    /// Changes cracked particle effect's material when square spawned with different color.
    /// </summary>
    /// <param name="particleSystemRenderer"></param>
    public void ChangeCrackParticleMaterial(ParticleSystemRenderer particle )
    {
        crackedEffect.GetComponent<ParticleSystemRenderer>().sharedMaterial = particle.sharedMaterial;
    }
    /// <summary>
    /// Return a value that depends on squareType.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    int GetScoreValueByType(SquareType type)
    {
        return (((int)type) + 1) * 5;
    }
    /// <summary>
    /// Changes squares x and y values to wanted values.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ChangeSquareXYValues(int x, int y)
    {
        this.x = x; 
        this.y = y;
    }

}
