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
}
