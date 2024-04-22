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

    public void UpdateGoals(SquareType goalType)
    {
        if (typeGoals[goalType] <= 0)
            return;
        typeGoals[goalType]--;
        UIManager.instance.UpdateGoalTexts();
    }

    void AssignGoalsToTypes()
    {
        typeGoals.Add(SquareType.Red, redGoal);
        typeGoals.Add(SquareType.Blue, blueGoal);
        typeGoals.Add(SquareType.Green, greenGoal);
        typeGoals.Add(SquareType.Purple, purpleGoal);
    }
}
