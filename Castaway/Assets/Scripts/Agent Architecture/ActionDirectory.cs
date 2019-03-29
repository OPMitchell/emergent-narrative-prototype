using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionDirectory : MonoBehaviour 
{
	[SerializeField] private TextAsset ActionListFile;
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
            return ActionList[i];
        }
        return null;
    }

    public List<Action> FindActionsSatisfyingGoalCondition(GameObject targetCharacter, Precondition goalCondition)
	{
		List<Action> matches = new List<Action>();
		foreach(Action action in ActionList)
		{
			bool satisfiesEmotion = SatisfiesEmotion(action, goalCondition, targetCharacter);
			bool satisfiesItem = SatisfiesItem(action, goalCondition);
			if(satisfiesEmotion && satisfiesItem)
				matches.Add(action);
		}
		return matches;
	}

	private bool SatisfiesEmotion(Action action, Precondition goalCondition, GameObject targetCharacter)
	{
		if(action.TargetObject == targetCharacter)
		{
			EmotionRef emotion = goalCondition.Emotion;
			BooleanCondition condition = goalCondition.BoolCondition;
			float change = action.Effect.Change;

			if(goalCondition.Emotion != EmotionRef.None)
			{
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
				else if(condition == BooleanCondition.EqualTo)
				{
					EmotionalPersonalityModel epm = action.TargetObject.GetComponent<EmotionalPersonalityModel>();
					if((float)epm.GetEmotionValue(emotion) + change == goalCondition.Value)
						return true;
				}
			}
		}
		return false;
	}

	private bool SatisfiesItem(Action action, Precondition goalCondition)
	{
		if(goalCondition.HoldingItem != null)
		{
			if(action.Effect.PickedUpItem != null)
			{
				Resource itemType = action.Effect.PickedUpItem.GetComponent<Item>().resource;
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
