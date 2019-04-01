using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
	public GameObject[,] Map {get; private set;}
	public GameObject[] Characters;
	private System.Random rnd;


	public bool mouseDown {get;set;}

	void Awake()
	{
		Testing.ClearLogs();
		mouseDown = false;
		rnd = new System.Random(System.DateTime.Now.Second);
		DrawMap();
		AddCharacters();
	}

	public int GetRandomInt(int min, int max)
	{
		return rnd.Next(min, max);
	}

	public void ClearTileSelections()
	{
		foreach(GameObject tile in Map)
		{
			tile.GetComponent<MouseOver>().ClearSelection();
		}
	}

	private void DrawMap()
	{
		Map = GetComponent<TerrainGenerator>().DrawTerrain();
	}

	private void AddCharacters()
	{
		foreach(GameObject c in Characters)
		{
			GameObject character = Instantiate(c) as GameObject;
			character.name = c.name;
		}
	}

	public string[] SplitParameterString(string effect)
	{
		if(effect != "")
		{
			int spaces = effect.Count(char.IsWhiteSpace);
			if(spaces == 2)
			{
				return effect.Split(' ');
			}
		}
		return null;
	}

	public List<GameObject> GetTaggedTreeTiles()
	{
		List<GameObject> trees = new List<GameObject>();
		foreach(GameObject t in Map)
		{
			Tile tile = t.GetComponent<Tile>();
			if(tile.tag == Tag.Cut && tile.item != null && tile.item.tag == "Tree")
			{
				trees.Add(t);
			}
		}
		return trees;
	}

	public List<GameObject> GetItemsOutsideStockpile()
	{
		List<GameObject> items = new List<GameObject>();
		foreach(GameObject t in Map)
		{
			Tile tile = t.GetComponent<Tile>();
			if(tile.item != null && tile.item.GetComponent<Item>().IsStockpilable() && tile.item.tag == "Item" && tile.zone != Zone.Stockpile && tile.item.GetComponent<Item>().IsFree())
			{
				items.Add(tile.item);
			}
		}
		return items;
	}

	public List<GameObject> GetEmptyStockpileTiles()
	{
		List<GameObject> tiles = new List<GameObject>();
		foreach(GameObject t in Map)
		{
			Tile tile = t.GetComponent<Tile>();
			if(tile.zone == Zone.Stockpile && tile.item == null && tile.IsFree())
			{
				tiles.Add(t);
			}
		}
		return tiles;
	}

	public List<GameObject> GetTaggedBuildingTiles()
	{
		List<GameObject> tiles = new List<GameObject>();
		foreach(GameObject t in Map)
		{
			Tile tile = t.GetComponent<Tile>();
			if(tile.tag == Tag.Build && tile.item == null && tile.IsFree())
			{
				tiles.Add(t);
			}
		}
		return tiles;	
	}

	public int GetNumberOfResourcesRequiredForTaggedBuildingTiles(Resource resource)
	{
		int count = 0;
		List<GameObject> taggedTiles = GetTaggedBuildingTiles();
		foreach(GameObject taggedTile in taggedTiles)
		{
			GameObject itemToBuild = taggedTile.GetComponent<Tile>().toBuild;
			if(itemToBuild != null)
			{
				BuildableItem bI = itemToBuild.GetComponent<BuildableItem>();
				if(bI.requiredResource == resource)
					count += bI.resourceCost;
			}
		}
		return count;
	}

	public List<GameObject> GetItemsOfResourceInStockpile(Resource r)
	{
		List<GameObject> items = new List<GameObject>();
		foreach(GameObject t in Map)
		{
			Tile tile = t.GetComponent<Tile>();
			if(tile.zone == Zone.Stockpile && tile.item != null && tile.item.GetComponent<Item>().resource == r && tile.item.GetComponent<Item>().IsFree())
			{
				items.Add(tile.item);
			}
		}
		return items;	
	}

	public List<GameObject> GetItemsOfResource(Resource r)
	{
		List<GameObject> items = new List<GameObject>();
		foreach(GameObject t in Map)
		{
			Tile tile = t.GetComponent<Tile>();
			if(tile.item != null && tile.item.GetComponent<Item>().resource == r && tile.item.GetComponent<Item>().IsFree())
			{
				items.Add(tile.item);
			}
		}
		return items;	
	}

	public GameObject GetPassableNeighbourTile(Tile rootTile)
	{
		int rX = rootTile.X;
		int rY = rootTile.Y;

		for(int x = -1; x < 2; x++)
		{
			for(int y = -1; y < 2; y++)
			{
				if(x != 0 && y != 0 || x == 0 && y == 0)
					continue;
				int nX = rX + x;
				int nY = rY + y;
				if(nX >= 0 && nX < Map.GetLength(0) && nY >= 0 && nY < Map.GetLength(1))
				{
					Tile tile = Map[nX,nY].GetComponent<Tile>();
					Node neighbourTile = Map[nX, nY].GetComponent<Node>();
					if(neighbourTile.IsPassable() && tile.IsFree())
					{
						return Map[nX, nY];
					}
				}
			}
		}
		return null;
	}

	public object GetStatValue(string characterTarget, string statName)
	{
		GameObject target = GameObject.Find(characterTarget);
		if(target != null)
		{
			EmotionalPersonalityModel EPM = target.GetComponent<EmotionalPersonalityModel>();
			Character character = target.GetComponent<Character>();

			if(statName == EmotionRef.Disappointment.ToString())
			{
				return (float)EPM.GetEmotionValue(EmotionRef.Disappointment);
			}
			else if(statName == EmotionRef.Distress.ToString())
			{
				return (float)EPM.GetEmotionValue(EmotionRef.Distress);
			}
			else if(statName == EmotionRef.Fear.ToString())
			{
				return (float)EPM.GetEmotionValue(EmotionRef.Fear);
			}
			else if(statName == EmotionRef.Joy.ToString())
			{
				return (float)EPM.GetEmotionValue(EmotionRef.Joy);
			}
			else if(statName == EmotionRef.Satisfaction.ToString())
			{
				return (float)EPM.GetEmotionValue(EmotionRef.Satisfaction);
			}
			else if(statName == "inventory")
			{
				return character.heldItem;
			}
			else if (statName == "location")
			{
				return new int[2]{character.cX, character.cY};
			}
			else if(statName == "hunger")
			{
				return 0.5f;
			}
		}
		return null;
	}

	/*
	public bool IsActionSatisfied(Action action)
	{
		string[] precondition = SplitParameterString(action.Precondition);
		if(precondition == null)
			return true;
		else
		{
			string statName = precondition[0];
			string operation = precondition[1];
			string value = precondition[2];

			object statValue = GetStatValue(action.Target, statName);
			if(statValue != null)
			{
				if(statName == "inventory")
				{
					GameObject heldItem = (GameObject)statValue;
					if(heldItem.GetComponent<Item>().resource.ToString().ToLower() == value)
						return true;
					else
						return false;
				}
				else if(statName == "location")
				{
					int[] coordinates = (int[])statValue;
					Character sender = GameObject.Find(action.Sender).GetComponent<Character>();
					if(sender.cX == coordinates[0] && sender.cY == coordinates[1])
						return true;
					else
						return false;
				}
				else
				{
					float targetValue = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					if(operation == "gt")
					{
						float? actual = (float?)statValue;
						if(actual == null)
							return false;
						else if(actual > targetValue)
							return true;
						return false;
					}
					else if(operation == "lt")
					{
						float? actual = (float?)statValue;
						if(actual == null)
							return false;
						else if(actual < targetValue)
							return true;
						return false;
					}
				}
			}
			return false;
		}
	}
	*/
}
