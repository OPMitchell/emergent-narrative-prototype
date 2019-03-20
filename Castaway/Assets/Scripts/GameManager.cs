using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject[,] Map {get; private set;}
	public GameObject[] Characters;

	public bool mouseDown {get;set;}

	private void Start()
	{
		mouseDown = false;
		DrawMap();
		AddCharacters();
	}

	public void ClearTileSelections()
	{
		foreach(GameObject tile in Map)
		{
			tile.GetComponent<MouseOver>().ClearSelection();
			tile.GetComponent<Node>().Selected = false;
		}
	}

	public void SetSelected(GameObject t)
	{
		foreach(GameObject tile in Map)
		{
			if(tile == t)
				tile.GetComponent<Node>().Selected = true;
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
			character.name = character.GetComponent<Character>().Name;
		}
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
		foreach(GameObject item in GameObject.FindGameObjectsWithTag("Item"))
		{
			if(item.GetComponent<Item>().IsStockpilable())
			{
				if(item.GetComponent<Item>().Parent.GetComponent<Tile>().zone != Zone.Stockpile)
					items.Add(item);
			}

		}
		return items;
	}

	public List<GameObject> GetEmptyStockpileTiles()
	{
		List<GameObject> tiles = new List<GameObject>();
		foreach(GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
		{
			if(tile.GetComponent<Tile>().zone == Zone.Stockpile && tile.GetComponent<Tile>().item == null)
			{
				tiles.Add(tile);
			}
		}
		return tiles;
	}

	public List<GameObject> GetTaggedBuildingTiles()
	{
		List<GameObject> tiles = new List<GameObject>();
		foreach(GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
		{
			if(tile.GetComponent<Tile>().tag == Tag.Build && tile.GetComponent<Tile>().item == null)
			{
				tiles.Add(tile);
			}
		}
		return tiles;
	}
}
