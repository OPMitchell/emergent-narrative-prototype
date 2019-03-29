using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalDirectory : MonoBehaviour 
{
	[SerializeField] private Goal[] goalList;
    public Goal[] GoalList
    {
        get
        {
            return goalList;
        }
        private set
        {
            goalList = value;
        }
    }

    public int Count()
    {
        return GoalList.Count();
    }

    public Goal GetGoal(int i)
    {
        if(i >= 0 && i < GoalList.Count())
        {
            return GoalList[i];
        }
        return null;
    }

    public void RemoveGoal(Goal goal)
    {
        List<Goal> temp = goalList.ToList();
        temp.Remove(goal);
        goalList = temp.ToArray();
    }
}
