using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour 
{
	public GameObject grass;
	public GameObject mountain;
	public GameObject water;
	public GameObject sand;

	public GameObject tree;

	private const float tileWidth = 2.0f;
	private const int heightMapSize = 6;

	private GameObject[,] map;

	public GameObject[,] DrawTerrain()
	{
		int mapwidth = (int)Mathf.Pow(2,heightMapSize)+1;
		map = new GameObject[mapwidth, mapwidth];
		float[,] heightmap = DiamondSquare.CreateHeightmap(heightMapSize, 1233, 1.0f, 0.5f);
		for(int x = 0; x < heightmap.GetLength(0); x++)
		{
			for(int y = 0; y < heightmap.GetLength(1); y++)
			{
				float height = heightmap[x,y];

				GameObject newTile = null;
				GameObject tileSprite = GetSprite(height);
				GameObject tileItem = GetItem(tileSprite.GetComponent<Tile>().Type, x, y);

				newTile = Instantiate(tileSprite, new Vector3(x*tileWidth, y*tileWidth, 0), Quaternion.identity) as GameObject;
				Tile t = newTile.GetComponent<Tile>();
				t.AddItem(tileItem);
				newTile.name = t.Type.ToString() + " ("+ x +"," + y + ")";
				t.X = x;
				t.Y = y;
				newTile.GetComponent<Node>().X = x;
				newTile.GetComponent<Node>().Y = y;

				map[x,y] = newTile;
			}
		}
		return map;
	}

	private GameObject PlantTree(int x, int y)
	{
		int numberOfTrees = Random.Range(0,100);
		if(numberOfTrees <= 75)
			numberOfTrees = 0;
		else if (numberOfTrees > 75 && numberOfTrees <= 100)
			numberOfTrees = 1;
		if(numberOfTrees > 0)
		{
			GameObject t = Instantiate(tree, new Vector3(x*tileWidth,y*tileWidth,0), Quaternion.identity) as GameObject;
			t.name = "Tree (" + x + "," + y + ")";
			return t;
		}
		return null;
	}

	private GameObject GetSprite(float height)
	{
		if(height <= 0.3f)
			return water;
		else if (height > 0.3f && height <= 0.35f)
			return sand;
		else if (height > 0.35f && height <= 0.7f)
			return grass;
		else if (height > 0.7f && height <= 1.0f)
			return mountain;
		else
			return null;
	}

	private GameObject GetItem(TileType type, int x, int y)
	{
		if(type == TileType.Grass)
			return PlantTree(x,y);
		return null;
	}

}
