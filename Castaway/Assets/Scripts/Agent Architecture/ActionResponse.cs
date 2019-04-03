using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Response
{
	[SerializeField] private Precondition[] preconditions;
	public Precondition[] Preconditions
	{
		get
		{
			return preconditions;
		}
		private set
		{
			preconditions = value;
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
	[SerializeField] private Response[] responses;
	public Response[] Responses
	{
		get
		{
			return responses;
		}
	}
}
