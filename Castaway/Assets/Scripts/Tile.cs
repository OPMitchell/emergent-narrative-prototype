using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Zone
{
	None = 0,
	Stockpile = 1
}

public enum TileType
{
	Water = 0,
	Sand = 1,
	Grass = 2,
	Mountain = 3
}

public enum Tag
{
	None = 0,
	Cut = 1,
	Build = 2
}

public class Tile : MonoBehaviour
{
	public GameObject item;
	public Zone zone{get;private set;}
	private ClickController clickController;
	private BuildManager buildManager;
	private UIManager ui;
	public Tag tag {get; private set;}
	public int X {get; set;}
	public int Y {get; set;}
	public TileType Type;
	private GameObject _lock;
	private Node node;
	public GameObject toBuild {get;set;}

	void Start()
	{
		tag = Tag.None;
		zone = Zone.None;
		clickController = GameObject.Find("GameManager").GetComponent<ClickController>();
		buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
		ui = GameObject.Find("GameManager").GetComponent<UIManager>();
		_lock = null;
		node = GetComponent<Node>();
	}

	public void AddItem(GameObject i)
	{
		if(i != null)
		{
			if(item == null)
			{
				item = i;
				i.GetComponent<Item>().Parent = gameObject;
			}
			else
			{
				Debug.Log("Tried to add item but tile was already taken by another item!");
			}
		}
	}

	void OnMouseDown()
	{
		if(clickController.currentClickAction == ClickAction.Zone_Stockpile)
		{
			ToggleZone(Zone.Stockpile);
		}
		else if(clickController.currentClickAction == ClickAction.Cut)
		{
			if(item != null && item.tag == "Tree")
				ToggleTagged(Tag.Cut);
		}
		else if(clickController.currentClickAction == ClickAction.Build)
		{
			if(item == null)
			{
				if(zone == Zone.Stockpile)
					Unzone();
				else
				{
					GameObject item = ui.currentButton.GetComponent<BuildableItemButton>().buildableItem;
					if(buildManager.CanBuild(item))
					{
						toBuild = item;
						ToggleTagged(Tag.Build);
					}
				}
			}
		}
	}

	public void DisabledTagged()
	{
		tag = Tag.None;
		if(item != null)
		{
			item.GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 0, 1);
		}
	}

	public void ToggleTagged(Tag t)
	{
		if(tag != t)
			tag = t;
		else
			tag = Tag.None;
		if(item != null)
			ColorItem(t);
	}

	private void ColorItem(Tag t)
	{
		if(tag == t)
			item.GetComponent<SpriteRenderer>().color = Color.red;
		else if (tag == Tag.None)
			item.GetComponent<SpriteRenderer>().color = Color.white;
	}

	void ToggleZone(Zone zoneType)
	{
		if(node.IsPassable())
		{
			if(zone != zoneType)
			{
				zone = zoneType;
				Color stockpileColor = Color.red;
				stockpileColor.a = 0.333f;
				GetComponent<SpriteRenderer>().color = stockpileColor;
				//ToggleTagged();
			}
			else
			{
				zone = Zone.None;
				Color stockpileColor = Color.white;
				GetComponent<SpriteRenderer>().color = stockpileColor;
				//ToggleTagged();
			}
		}
	}

	void Unzone()
	{
		zone = Zone.None;
	}

	public bool Free(GameObject owner)
	{
		if(_lock == owner)
		{
			_lock = null;
			return true;
		}
		return false;
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
