using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour 
{
	public MemoryPool memoryPool {get; private set;}
	private int currentID;
	public int CurrentID
	{
		get
		{
			int temp = currentID;
			currentID++;
			return temp;
		}
	}

	void Start() 
	{
		memoryPool = new MemoryPool();
	}

	public MemoryPattern RetrieveMemoryPattern(string keyword)
	{
		MemoryPattern memory = memoryPool.RetrieveMemoryPattern(keyword);
		if(memory != null)
			Testing.WriteToLog(transform.name, "Retrieved memory: " + Testing.GetMemoryInfo(memory));
		return memory;
	}

	public void AddMemoryPattern(MemoryPattern mp)
	{
		memoryPool.AddMemoryPattern(mp);
	}
}
