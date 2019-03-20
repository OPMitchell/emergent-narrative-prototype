using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClickAction
{
	None = 1,
	Cut = 2,
	Haul = 3,
	Zone_Stockpile = 4,
	Build = 5
}

public class ClickController : MonoBehaviour
{
	public ClickAction currentClickAction {get;private set;}

	void Start()
	{
		currentClickAction = ClickAction.None;
	}

	public void SetClickAction(ClickAction ca)
	{
		currentClickAction = ca;
	}
}
