using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour 
{
	[Range(1,15)] public float Speed = 5.0f;
	public int cX {get; private set;}
	public int cY {get; private set;}
	private GameObject[,] map;
	private GameManager manager;
	private UIManager ui;
	public GameObject heldItem;	
	public Action currentAction {get;set;}
	private GameObject _lock;
	private float fixedSpeed;
	private Coroutine currentActionCoroutine;

	void Awake() 
	{
		heldItem = null;
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		ui = GameObject.Find("GameManager").GetComponent<UIManager>();
		map = manager.Map;
		SpawnAtRandomPos();
		currentAction = null;
		fixedSpeed = Speed;
	}

	void Update()
	{
		PhysicalResourceModel prm = GetComponent<PhysicalResourceModel>();
		float hunger = prm.GetPhysicalValue(PhysicalRef.Hunger) + 1.0f;
		float tiredness = prm.GetPhysicalValue(PhysicalRef.Tiredness) + 1.0f;
		float total = hunger + tiredness;
		if(total <= 1)
			total = 1;
		Speed = fixedSpeed/total;
	}

	public bool AtPosition(int x, int y)
	{
		if(cX == x && cY == y)
			return true;
		return false;
	}

	public bool PickUpItem(Tile tile)
	{
		if(tile.item != null && AtPosition(tile.X, tile.Y))
		{
			if(heldItem == null)
			{
				string name = tile.item.name;
				heldItem = GameObject.Find(name);
				heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "HeldItem";
				tile.item = null;
			}
			else
			{
				string name = tile.item.name;
				GameObject temp = heldItem;
				heldItem = GameObject.Find(name);
				tile.item = null;
				tile.AddItem(temp);
				temp.GetComponent<Item>().Free(gameObject);
				heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "HeldItem";

			}
			return true;
		}
		else
		{
			return false;
		}
	}

	public void GiveItem(GameObject item)
	{
		if(heldItem == null)
		{
			heldItem = item;
			heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "HeldItem";
		}
		else
		{
			GameObject temp = heldItem;
			heldItem = item;
			temp.GetComponent<Item>().Free(gameObject);
			heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "HeldItem";
		}
	}

	public void DropItem(Tile tile)
	{
		if(tile.item == null && heldItem != null && AtPosition(tile.X, tile.Y))
		{
			string name = heldItem.name;
			heldItem.GetComponent<Item>().Free(gameObject);
			GameObject droppedItem = heldItem;
			tile.GetComponent<Tile>().AddItem(droppedItem);
			heldItem = null;
			droppedItem.name = ReplaceCoordinates(droppedItem.name, tile.X, tile.Y);
			//droppedItem.GetComponent<SpriteRenderer>().sortingLayerName = "Item";
		}
	}

	public void DestroyItem()
	{
		Destroy(heldItem);
		heldItem = null;
	}

	void SpawnAtRandomPos()
	{
		do
		{
			cX = manager.GetRandomInt(0, map.GetLength(0)-1);
			cY = manager.GetRandomInt(0, map.GetLength(1)-1);
		} while (!map[cX,cY].GetComponent<Node>().IsPassable());
		transform.position = map[cX,cY].transform.position;
		Debug.Log(gameObject.name + " spawned at (" + cX + "," + cY +")");
	}

	public bool WalkToCoordinates(int x, int y)
	{
		if(AtPosition(x,y))
			return true;
		Debug.Log("Moving " + gameObject.name + " to (" + x + "," + y +")");
		List<Node> path = FindPathToTarget(x, y);
		if(path == null)
			return false;
		else
		{
			currentActionCoroutine = StartCoroutine(FollowPath(path));
			return true;
		}
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
			Animator animator = GetComponent<Animator>();
			while(cX != path[path.Count -1].X || cY != path[path.Count -1].Y)
			{
				Node currentNode = path[0];

				//calculate direction for animations
				int horizontal = 0;
				int vertical = 0;
				if(currentNode.X < cX)
					horizontal = -1;
				else if(currentNode.X > cX)
					horizontal = 1;
				if(currentNode.Y < cY)
					vertical = -1;
				else if(currentNode.Y > cY)
					vertical = 1;
				if(vertical != 0 && horizontal != 0)
					horizontal = 0;

				animator.SetInteger("Horizontal", horizontal);
				animator.SetInteger("Vertical", vertical);

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
			animator.SetInteger("Horizontal", 0);
			animator.SetInteger("Vertical", 0);
		}
	}

	void OnMouseDown()
	{
		//ui.ShowCharacterPanel();
	}

	private string ReplaceCoordinates(string s, int x, int y)
	{
		int start = s.IndexOf("(");
		int end = s.IndexOf(")");
		string result = s.Substring(start+1, end - start - 1);
		s = s.Replace(result, x + "," + y);
		return s;
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
