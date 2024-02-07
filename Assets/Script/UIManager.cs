using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI stateText;
    int score = 0;

    public void AddScore(EventManager.OnGetScore onGetScore, SquareType type)
    {
        score += onGetScore.Invoke(type);
        scoreText.text = "Score: " + score; 
    }
    public void ChangeStateText(GameState gameState)
    {
        stateText.text = gameState.ToString();
    }
    
}
