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
			if(zone == Zone.None)
				SetZone(Zone.Stockpile);
			else
				ClearZone();
		}
		else if(clickController.currentClickAction == ClickAction.Cut)
		{
			if(item != null && item.tag == "Tree")
				if(tag == Tag.None)
					SetTag(Tag.Cut);
				else
					ClearTag();
		}
		else if(clickController.currentClickAction == ClickAction.Build)
		{
			if(item == null)
			{
				if(zone == Zone.Stockpile)
					ClearZone();
				GameObject item = ui.currentButton.GetComponent<BuildableItemButton>().buildableItem;
				if(buildManager.CanBuild(item))
				{
					toBuild = item;
					if(tag == Tag.None)
						SetTag(Tag.Build);
					else
						ClearTag();
				}
			}
		}
	}

	public void ClearTag()
	{
		tag = Tag.None;
		//ColorItem();
		ColorTile();
		ColorZone();
	}

	public void SetTag(Tag t)
	{
		tag = t;
		//ColorItem();
		ColorTile();
	}

	private void ColorItem()
	{
		if(item != null)
		{
			if(tag == Tag.None)
				item.GetComponent<SpriteRenderer>().color = Color.white;
			else
				item.GetComponent<SpriteRenderer>().color = Color.red;
		}
	}

	private void ColorTile()
	{
		if(tag == Tag.None)
			GetComponent<SpriteRenderer>().color = Color.white;
		else
			GetComponent<SpriteRenderer>().color = Color.red;
	}

	private void ColorZone()
	{
		if(zone == Zone.None)
			GetComponent<SpriteRenderer>().color = Color.white;
		else
			GetComponent<SpriteRenderer>().color = Color.blue;
	}

	void SetZone(Zone z)
	{
		if(node.IsPassable())
		{
			zone = z;
			ColorZone();
		}
	}

	void ClearZone()
	{
		zone = Zone.None;
		ColorZone();
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
