using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GoalManager : Singleton<GoalManager>
{
    [SerializeField] int redGoal;
    [SerializeField] int blueGoal;
    [SerializeField] int greenGoal;
    [SerializeField] int purpleGoal;
    public Dictionary<SquareType,int> typeGoals = new Dictionary<SquareType,int>();

    private void Start()
    {
        AssignGoalsToTypes();
        UIManager.instance.UpdateGoalTexts();
    }
    /// <summary>
    /// Check goals and if all goals are 0 trigger end game event.
    /// </summary>
    private void CheckGoals()
    {
        int zeroCount = 0;
        foreach(var goalCount in typeGoals)
        {
            if(goalCount.Value == 0)
            {
                zeroCount++;
            }
        }
        if(zeroCount >=  typeGoals.Count)
        {
            EventManager.instance.onEndGame?.Invoke();
        }
    }
    /// <summary>
    /// Updates goal text values.
    /// </summary>
    /// <param name="goalType"></param>
    public void UpdateGoals(SquareType goalType)
    {
        if (typeGoals[goalType] <= 0)
            return;
        typeGoals[goalType]--;
        UIManager.instance.UpdateGoalTexts();
        CheckGoals();
    }
    
    /// <summary>
    /// Assign given goals to related type in dictionary.
    /// </summary>
    void AssignGoalsToTypes()
    {
        typeGoals.Add(SquareType.Red, redGoal);
        typeGoals.Add(SquareType.Blue, blueGoal);
        typeGoals.Add(SquareType.Green, greenGoal);
        typeGoals.Add(SquareType.Purple, purpleGoal);
    }
}
