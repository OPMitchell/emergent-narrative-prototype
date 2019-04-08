using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resource
{
	nature = 0,
	buildable = 1,
	logs = 2,
	food = 3,
	flowers = 4,
};

public class ResourceManager : MonoBehaviour
{
	private int[] Resources = new int[5];

	public void AddResource(Resource resource, int quantity)
	{
		Resources[(int)resource] += quantity;
	}

	public void RemoveResource(Resource resource, int quantity)
	{

		if(Resources[(int)resource] - quantity < 0)
			Resources[(int)resource] = 0;
		else
			Resources[(int)resource] -= quantity;
	}

	public int GetResourceQuantity(Resource resource)
	{
		return Resources[(int)resource];
	}
}
