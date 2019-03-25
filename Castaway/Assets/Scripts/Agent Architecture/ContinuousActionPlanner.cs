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
	private GameManager manager;
	const int maxFail = 3; 	// Maximum number of times a goal can be replanned.

	// Used for initialisation.
	void Start()
	{
		actionDirectory = GetComponent<ActionDirectory>();
		goalDirectory = GetComponent<GoalDirectory>();
		actionQueue = GetComponent<ActionQueue>();
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
					// Create a LinkedList to store the plans and populate it.
					PlanList planList = new PlanList();
					CreatePlans(currentGoal, currentGoal.Target, currentGoal.SuccessCondition, planList, new Plan(), currentGoal.FailedActions);
					// If there are no plans in the collection then the goal is already satisifed or its impossible to reach.
					if(planList.Count() < 1)
					{
						// Set the goal to complete and it will be removed from the collection the next time Update() is run.
						currentGoal.Complete = true;
						Testing.WriteToLog(transform.name, "cancelled goal: " + currentGoal.SuccessCondition + " because a plan is impossible to make.");
					}
					// If we successfully constructed one or more plans then we need to pick one for the agent to use.
					else
					{
						Testing.WriteToLog(transform.name, "created a plan for goal: " + currentGoal.SuccessCondition);
						//pick a plan from List<LinkedList<Action>> plans and assign to g.plan
						currentGoal.SetPlan(planList.GetBestPlan());
					}
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
							currentAction.SetStatus(Status.Sent);
							actionQueue.QueueAction(currentAction);
						break;
						case Status.Failed:
							//action repair
							plan.RemoveAction(currentAction);
						break;
						case Status.Successful:
							plan.RemoveAction(currentAction);
						break;
					}
				}
				if(currentGoal.Plan != null && currentGoal.Plan.CountActions() <= 0)
					currentGoal.SetPlan(null);
			}
		}
	}

	private void CreatePlans(Goal g, string target, string successCondition, PlanList plans, Plan p, List<Action> ignoreList)
	{
		List<Action> satisfyingActions = actionDirectory.FindActionsSatisfyingPrecondition(target, successCondition);		
		//RemovePreviouslyFailedActions(satisfyingActions, ignoreList);
		foreach(Action action in satisfyingActions)
		{
			Plan newPlan = new Plan(p);
			newPlan.AddAction(action);
			if(!action.HasPrecondition()) //|| action.IsPreconditionSatisfied())
			{
				plans.Add(newPlan);
			}
			else
			{
				string nextTarget = transform.name;
				string[] split = manager.SplitParameterString(action.Precondition);
				if(split[0] == "location")
					nextTarget = split[2];		
				CreatePlans(g, nextTarget, action.Precondition, plans, newPlan, ignoreList);
			}
		}
	}

}
