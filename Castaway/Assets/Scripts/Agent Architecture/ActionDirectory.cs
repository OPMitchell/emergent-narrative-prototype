using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionDirectory : MonoBehaviour 
{
	private GameManager manager;
	[SerializeField] private Action[] ActionList;

	void Awake()
	{
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    public Action GetAction(int i)
    {
        if(i >= 0 && i < ActionList.Count())
        {
            return new Action(ActionList[i]);
        }
        return null;
    }

    public Action GetAction(string actionName)
    {
		foreach(Action action in ActionList)
		{
			if(action.Name.ToLower() == actionName.ToLower())
				return new Action(action);
		}
        return null;
    }

    public List<Action> FindActionsSatisfyingGoalCondition(GameObject targetCharacter, Precondition goalCondition)
	{
		List<Action> matches = new List<Action>();
		foreach(Action action in ActionList)
		{
			bool satisfiesStat = SatisfiesStat(action, goalCondition, targetCharacter);
			bool satisfiesItem = SatisfiesItem(action, goalCondition);
			if(satisfiesStat && satisfiesItem)
				matches.Add(new Action(action));
		}
		return matches;
	}

	private bool SatisfiesStat(Action action, Precondition goalCondition, GameObject targetCharacter)
	{
		if(goalCondition.Stat == StatName.None)
			return true;
		if(action.TargetObject == targetCharacter)
		{
			StatName stat = goalCondition.Stat;
			StatName actionStat = action.TargetEffect.Stat;
			BooleanCondition condition = goalCondition.BoolCondition;
			float change = action.TargetEffect.Change;

			if(goalCondition.Stat != StatName.None && stat == actionStat)
			{
				EmotionalPersonalityModel epm = action.TargetObject.GetComponent<EmotionalPersonalityModel>();
				PhysicalResourceModel prm = action.TargetObject.GetComponent<PhysicalResourceModel>();

				if(condition == BooleanCondition.LessThan)
				{
					if(change < 0.0f)
						return true;
				}
				else if(condition == BooleanCondition.GreaterThan)
				{
					if(change > 0.0f)
						return true;
				}
				else if(condition == BooleanCondition.GreaterThanOrEqualTo)
				{
					if((int)stat > 0 && (int)stat < Precondition.physicalIndex)
					{
						if(change > 0.0f || ((float)epm.GetEmotionValue(stat.ToString()) + change == goalCondition.Value))
							return true;
					}
					else if((int)stat >= Precondition.physicalIndex && (int)stat < Precondition.limitIndex)
					{
						if(change > 0.0f || ((float)prm.GetPhysicalValue(stat.ToString()) + change == goalCondition.Value))
							return true;
					}
				}
				else if(condition == BooleanCondition.LessThanOrEqualTo)
				{
					if((int)stat > 0 && (int)stat < Precondition.physicalIndex)
					{
						if(change < 0.0f || ((float)epm.GetEmotionValue(stat.ToString()) + change == goalCondition.Value))
							return true;
					}
					else if((int)stat >= Precondition.physicalIndex && (int)stat < Precondition.limitIndex)
					{
						if(change < 0.0f || ((float)prm.GetPhysicalValue(stat.ToString()) + change == goalCondition.Value))
							return true;
					}
				}
				else if(condition == BooleanCondition.EqualTo)
				{
					if((int)stat > 0 && (int)stat < Precondition.physicalIndex)
					{
						if((float)epm.GetEmotionValue(stat.ToString()) + change == goalCondition.Value)
							return true;
					}
					else if((int)stat >= Precondition.physicalIndex && (int)stat < Precondition.limitIndex)
					{
						if((float)prm.GetPhysicalValue(stat.ToString()) + change == goalCondition.Value)
							return true;
					}
				}
			}
		}
		return false;
	}

	private bool SatisfiesItem(Action action, Precondition goalCondition)
	{
		if(goalCondition.HoldingItem != null)
		{
			if(action.SenderEffect.PickedUpItem != null)
			{
				Resource itemType = action.SenderEffect.PickedUpItem.GetComponent<Item>().resource;
				Resource goalItemType = goalCondition.HoldingItem.GetComponent<Item>().resource;
				if(itemType == goalItemType)
					return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return true;
		}
		return false;
	}
}
