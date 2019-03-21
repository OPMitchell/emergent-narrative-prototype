using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour 
{
	private ItemManager items;
	private ResourceManager resources;

	void Start()
	{
		items = GameObject.Find("GameManager").GetComponent<ItemManager>();
		resources = GameObject.Find("GameManager").GetComponent<ResourceManager>();
	}

	public bool CanBuild(GameObject item)
	{
		BuildableItem build = item.GetComponent<BuildableItem>();
		if(resources.GetResourceQuantity(build.requiredResource) >= build.resourceCost)
			return true;
		else
		{
			Debug.Log("Not enough resources! " + "You need " + build.resourceCost + " " + build.requiredResource.ToString() + ", but you only have " + resources.GetResourceQuantity(build.requiredResource)+"!");
			return false;
		}
	}

	public void Build(GameObject t, GameObject item)
	{
		Tile tile = t.GetComponent<Tile>();
		BuildableItem build = item.GetComponent<BuildableItem>();
		resources.RemoveResource(build.requiredResource, build.resourceCost);
		Instantiate(item, t.GetComponent<Renderer>().bounds.center, Quaternion.identity);
		if(!build.passable)
			t.GetComponent<Node>().SetPassable(false);
	}
}
