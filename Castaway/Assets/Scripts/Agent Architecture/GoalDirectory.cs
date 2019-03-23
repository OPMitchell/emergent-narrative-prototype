using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalDirectory : MonoBehaviour 
{
	[SerializeField] private TextAsset GoalListFile;
	public List<Goal> GoalList {get; private set;}

	void Awake()
	{
		CreateGoalList();
	}

	private void CreateGoalList()
    {
        GoalList = ConfigReader.ReadGoals(GoalListFile.name + ".xml");
    }

    public void AddGoal(Goal goal)
    {
        if(goal != null)
            GoalList.Add(goal);
    }

    public void RemoveGoal(Goal goal)
    {
        if(goal != null)
            GoalList.Remove(goal);
    }

    public int Count()
    {
        return GoalList.Count;
    }

    public Goal GetGoal(int i)
    {
        if(i >= 0 && i < GoalList.Count)
        {
            return GoalList[i];
        }
        return null;
    }
}
