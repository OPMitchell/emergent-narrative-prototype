using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour 
{
	public float Speed = 5.0f;
	public string Name;
	public int cX {get; private set;}
	public int cY {get; private set;}
	private GameObject[,] map;
	private GameManager manager;
	private UIManager ui;
	public GameObject heldItem;
	public Action currentAction;	

	private Coroutine currentActionCoroutine;

	void Start () 
	{
		heldItem = null;
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		ui = GameObject.Find("GameManager").GetComponent<UIManager>();
		map = manager.Map;
		currentAction = null;
		SpawnAtRandomPos();
	}

	public bool AtPosition(int x, int y)
	{
		if(cX == x && cY == y)
			return true;
		return false;
	}

	public void PickUpItem(Tile tile)
	{
		if(tile.item != null)
		{
			if(heldItem == null)
			{
				string name = tile.item.name;
				heldItem = Instantiate(tile.item);
				tile.DeleteItem();
				heldItem.name = name;
			}
			else
			{
				string name = tile.item.name;
				GameObject temp = heldItem;
				heldItem = Instantiate(tile.item);
				tile.DeleteItem();
				tile.AddItem(temp);
				heldItem.name = name;
			}
		}
	}

	public void DropItem(Tile tile)
	{
		if(tile.item == null && heldItem != null && AtPosition(tile.X, tile.Y))
		{
			string name = heldItem.name;
			GameObject droppedItem = Instantiate(heldItem);
			tile.GetComponent<Tile>().AddItem(droppedItem);
			Destroy(heldItem);
			heldItem = null;
			droppedItem.name = name;
		}
	}

	public void DestroyItem()
	{
		Destroy(heldItem);
		heldItem = null;
	}

	void SpawnAtRandomPos()
	{
		Random.InitState((int)Time.time);
		do
		{
			cX = Random.Range(0, map.GetLength(0)-1);
			cY = Random.Range(0, map.GetLength(1)-1);
		} while (!map[cX,cY].GetComponent<Node>().IsPassable());
		transform.position = map[cX,cY].transform.position;
		Debug.Log(gameObject.name + " spawned at (" + cX + "," + cY +")");
	}

	public void WalkToCoordinates(int x, int y)
	{
		if(AtPosition(x,y))
			return;
		if(currentAction != null)
			StopCoroutine(currentActionCoroutine);
		Debug.Log("Moving " + Name + " to (" + x + "," + y +")");
		List<Node> path = FindPathToTarget(x, y);
		currentActionCoroutine = StartCoroutine(FollowPath(path));
	}

	List<Node> FindPathToTarget(int tX, int tY)
	{
		List<Node> path = Pathfinder.FindPath(cX, cY, tX, tY);
		if(path == null)
		{
			Debug.Log("No path could be found.");
		}
		return path;
	}

	private IEnumerator FollowPath(List<Node> path)
	{
		if(path != null)
		{
			while(cX != path[path.Count -1].X || cY != path[path.Count -1].Y)
			{
				Node currentNode = path[0];
				float step = Speed * Time.deltaTime;
				Vector3 target = map[currentNode.X, currentNode.Y].GetComponent<Renderer>().bounds.center;
				transform.position = Vector3.MoveTowards(transform.position, target, step);
				if(heldItem != null) 
					heldItem.transform.position = Vector3.MoveTowards(transform.position, target, step);
				if(transform.position == target)
				{
					cX = currentNode.X;
					cY = currentNode.Y;
					path.Remove(path[0]);
					if(path.Count <= 0)
					{
						break;
					}
				}
				yield return null;
			}
		}
	}

	void OnMouseDown()
	{
		//ui.ShowCharacterPanel();
	}
}
