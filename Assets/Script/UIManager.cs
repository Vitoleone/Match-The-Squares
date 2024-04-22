using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : Singleton<UIManager>
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI redGoalText;
    [SerializeField] private TextMeshProUGUI blueGoalText;
    [SerializeField] private TextMeshProUGUI greenGoalText;
    [SerializeField] private TextMeshProUGUI purpleGoalText;
    int score = 0;

    /// <summary>
    /// Adds score by the type of square then update scoreText value.
    /// </summary>
    /// <param name="onGetScore"></param>
    /// <param name="type"></param>
    public void AddScore(EventManager.OnGetScore onGetScore, SquareType type)
    {
        score += onGetScore.Invoke(type);
        scoreText.text = "Score: " + score; 
    }
    /// <summary>
    /// Updates stateText to given state value.
    /// </summary>
    /// <param name="gameState"></param>
    public void ChangeStateText(GameState gameState)
    {
        stateText.text = gameState.ToString();
    }
    
    public void UpdateGoalTexts()
    {
        redGoalText.text = " : " + GoalManager.instance.typeGoals[SquareType.Red].ToString();
        blueGoalText.text = " : " + GoalManager.instance.typeGoals[SquareType.Blue].ToString();
        greenGoalText.text = " : " + GoalManager.instance.typeGoals[SquareType.Green].ToString();
        purpleGoalText.text = " : " + GoalManager.instance.typeGoals[SquareType.Purple].ToString();
    }
}
