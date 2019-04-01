using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionResponse
{
	[SerializeField] private string receivedActionName;
	public string ReceivedActionName
	{
		get
		{
			return receivedActionName;
		}
	}
	[SerializeField] private Precondition[] precondition;
    public Precondition[] Precondition
    {
        get
        {
            return precondition;
        }
        private set
        {
            precondition = value;
        }
    }
	[SerializeField] private string responseActionName;
	public string ResponseActionName
	{
		get
		{
			return responseActionName;
		}
	}
}
