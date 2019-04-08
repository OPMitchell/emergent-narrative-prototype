using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour 
{
	public MemoryPool senderPool {get; private set;}
	public MemoryPool receiverPool {get; private set;}

	private int currentID;
	public int CurrentID
	{
		get
		{
			return currentID;
		}
	}

	void Start() 
	{
		senderPool = new MemoryPool();
		receiverPool = new MemoryPool();
	}

	public MemoryPattern RetrieveReceivedMemoryPattern(string keyword)
	{
		return RetrieveMemoryPattern(receiverPool, keyword);
	}

	public void AddReceivedMemoryPattern(MemoryPattern mp)
	{
		AddMemoryPattern(receiverPool, mp);
	}

	public MemoryPattern RetrieveSentMemoryPattern(string keyword)
	{
		return RetrieveMemoryPattern(senderPool, keyword);
	}

	public MemoryPattern RetrieveSentMemoryPatternByID(int id)
	{
		return RetrieveMemoryPattern(senderPool, id);
	}

	public void AddSentMemoryPattern(MemoryPattern mp)
	{
		AddMemoryPattern(senderPool, mp);
	}

	private MemoryPattern RetrieveMemoryPattern(MemoryPool pool, string keyword)
	{
		MemoryPattern memory = pool.RetrieveMemoryPattern(keyword);
		if(memory != null)
			Testing.WriteToLog(transform.name, "Retrieved memory: " + Testing.GetMemoryInfo(memory));
		return memory;
	}

	private MemoryPattern RetrieveMemoryPattern(MemoryPool pool, int id)
	{
		MemoryPattern memory = pool.RetrieveMemoryPattern(id);
		if(memory != null)
			Testing.WriteToLog(transform.name, "Retrieved memory: " + Testing.GetMemoryInfo(memory));
		return memory;
	}

	private void AddMemoryPattern(MemoryPool pool, MemoryPattern mp)
	{
		pool.AddMemoryPattern(mp);
		currentID++;
	}
}
