using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour 
{
	[SerializeField] private bool isStockpilable;
	private GameObject _lock;
	public GameObject Parent;
	public Resource resource;
	
	public bool InStockpile{get;set;}

	void Awake()
	{
		_lock = null;
	}
	
	public bool IsStockpilable()
	{
		return isStockpilable;
	}

	public void Free(GameObject owner)
	{
		if(_lock == owner)
			_lock = null;
	}

	public bool GetLock(GameObject owner)
	{
		if(_lock == null)
		{
			_lock = owner;
			return true;
		}
		return false;
	}

	public bool IsFree()
	{
		if(_lock == null)
			return true;
		return false;
	}
}
