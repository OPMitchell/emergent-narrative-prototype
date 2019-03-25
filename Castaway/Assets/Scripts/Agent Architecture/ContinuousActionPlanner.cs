using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// The Continuous Action Planner (CAP) is used by each agent to construct plan; a series of actions that, when executed, achieve a goal.
// The CAP is "continuous" because it is able to react to real-time changes in the game-world state, identifying if/when the constituent actions
// comprising a plan fail to execute, and re-plan accordingly. This is known as "action repair".
public class ContinuousActionPlanner : MonoBehaviour 
{
	private ActionDirectory actionDirectory;
	private GoalDirectory goalDirectory;
	private ActionQueue actionQueue;
	const int maxFail = 3; 	// Maximum number of times a goal can be replanned.

	// Used for initialisation.
	void Start()
	{
		actionDirectory = GetComponent<ActionDirectory>();
		goalDirectory = GetComponent<GoalDirectory>();
		actionQueue = GetComponent<ActionQueue>();
	}

	void Update()
	{
		foreach(Goal currentGoal in goalDirectory.GoalList)
		{
			//if the goal is already satisified then remove it if it's a pursuit goal. Otherwise, don't do anything.
			if(currentGoal.IsSatisfied())
			{
				if(currentGoal.Type == GoalType.Pursuit)
					goalDirectory.RemoveGoal(currentGoal);
			}
			//if the goal isn't satisifed then check if the goal has a plan associated with it.
			else
			{
				//If the goal hasn't got a plan then let's create one!
				if(currentGoal.Plan == null)
				{
					//create a plan
					PlanList planList = new PlanList();
					List<Action> satisfyingActions = actionDirectory.FindActionsSatisfyingPrecondition(currentGoal.Target, currentGoal.SuccessCondition);
					
				}
				// if the goal has got a plan...
				else
				{
					//manage its execution
					Plan plan = currentGoal.Plan;
					Action currentAction = plan.GetCurrentAction();
					switch(currentAction.Status)
					{
						case Status.notSent: 
							actionQueue.QueueAction(currentAction);
						break;
						case Status.Failed:
							//action repair
						break;
						case Status.Successful:
							plan.RemoveAction(currentAction);
						break;
					}
				}
			}
		}
	}

}
