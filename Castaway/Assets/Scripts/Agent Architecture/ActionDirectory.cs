using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionDirectory : MonoBehaviour 
{
	[SerializeField] private TextAsset ActionListFile;
	public List<Action> ActionList {get; private set;}
	private GameManager manager;

	void Awake()
	{
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		CreateActionList();
	}

	private void CreateActionList()
    {
        this.ActionList = ConfigReader.ReadActionList(ActionListFile.name + ".xml");
    }

    public Action GetAction(int i)
    {
        if(i >= 0 && i < ActionList.Count)
        {
            return new Action(ActionList[i]);
        }
        return null;
    }

    public List<Action> FindActionsSatisfyingPrecondition(string target, string precondition)
	{
		List<Action> result = new List<Action>();
		string[] split = manager.SplitParameterString(precondition);
		switch(split[1])
		{
			case "lt":
				result = FindActionsByParameterAndOperation(target, split[0], "-");
			break;
			case "gt":
				result = FindActionsByParameterAndOperation(target, split[0], "+");
			break;
			case "contains":
				result = FindActionsByParameterOperationAndValue(target, split[0], "contains", split[2]);
			break;
			case "at":
				result = FindActionsByParameterOperationAndValue(target, split[0], "at", split[2]);
			break;
		}
		return result;
	}

	private List<Action> FindActionsByParameterAndOperation(string target, string parameter, string operation)
	{
		List<Action> matches = ActionList.ConvertAll(x => new Action(x));
		matches = matches.Where(
			x => x.Effect != ""
			&& string.Equals(manager.SplitParameterString(x.Effect)[0], parameter)
			&& string.Equals(manager.SplitParameterString(x.Effect)[1], operation)
			&& (string.Equals(x.Target, target) || string.Equals(x.Target, "%tgt"))
		).ToList();
		ReplaceGenericParameters(matches, target);
		return matches;
	}

	private List<Action> FindActionsByParameterOperationAndValue(string target, string parameter, string operation, string value)
	{
		List<Action> matches = ActionList.ConvertAll(x => new Action(x));
		matches = matches.Where(
			x => x.Effect != ""
			&& string.Equals(manager.SplitParameterString(x.Effect)[0], parameter)
			&& string.Equals(manager.SplitParameterString(x.Effect)[1], operation)
			&& (string.Equals(manager.SplitParameterString(x.Effect)[2], value)  || string.Equals(manager.SplitParameterString(x.Effect)[2], "%val") || string.Equals(manager.SplitParameterString(x.Effect)[2], "%tgt"))
			&& (string.Equals(x.Target, target) || string.Equals(x.Target, "%tgt"))
		).ToList();
		ReplaceGenericParameters(matches, target, value);
		return matches;
	}

	private void ReplaceGenericParameters(List<Action> actions, string target, string value)
	{
		foreach(Action action in actions)
		{
			action.Effect = action.Effect.Replace("%val", value);
			action.Effect = action.Effect.Replace("%tgt", target);
			action.Target = action.Target.Replace("%tgt", target);
			action.Precondition = action.Precondition.Replace("%val", value);
			action.Precondition = action.Precondition.Replace("%tgt", target);
			action.Parameters = action.Parameters.Replace("%val", value);
		}
	}

	private void ReplaceGenericParameters(List<Action> actions, string target)
	{
		foreach(Action action in actions)
		{
			action.Effect = action.Effect.Replace("%tgt", target);
			action.Target = action.Target.Replace("%tgt", target);
			action.Precondition = action.Precondition.Replace("%tgt", target);
		}
	}
}
