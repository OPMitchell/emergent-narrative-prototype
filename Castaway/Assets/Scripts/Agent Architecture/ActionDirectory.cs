using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionDirectory : MonoBehaviour 
{
	[SerializeField] private TextAsset ActionListFile;
	public List<Action> ActionList {get; private set;}

	void Awake()
	{
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
		List<Action> result = null;
		string[] split = GameManager.SplitParameterString(precondition);
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


}
