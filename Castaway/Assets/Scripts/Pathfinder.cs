using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
	static int mapWidth;
	static int mapHeight;
	static GameObject[,] Map;

	private static void GetMap()
	{
		Map = GameObject.Find("GameManager").GetComponent<GameManager>().Map;
		mapWidth = Map.GetLength(0);
		mapHeight = Map.GetLength(1);
	}

	public static List<Node> FindPath(int cX, int cY, int tX, int tY)
	{
		GetMap();
		Node startNode = Map[cX, cY].GetComponent<Node>();
		Node targetNode = Map[tX, tY].GetComponent<Node>();

		List<Node> openList = new List<Node>();
		HashSet<Node> closedList = new HashSet<Node>();
		openList.Add(startNode);

		while(openList.Count > 0)
		{
			Node currentNode = openList[0];
			for(int i = 1; i < openList.Count; i++)
			{
				if(openList[i].F < currentNode.F || openList[i].F == currentNode.F && openList[i].H < currentNode.H)
					currentNode = openList[i];
			}

			openList.Remove(currentNode);
			closedList.Add(currentNode);

			if(currentNode == targetNode)
			{
				return RetracePath(startNode, targetNode);
			}

			foreach(Node neighbour in GetNeighbours(currentNode))
			{
				if(!neighbour.IsPassable() || closedList.Contains(neighbour))
					continue;
				int newMovementCostToNeighbour = currentNode.G + GetDistance(currentNode, neighbour);
				if(newMovementCostToNeighbour < neighbour.G  || !openList.Contains(neighbour))
				{
					neighbour.G = newMovementCostToNeighbour;
					neighbour.H = GetDistance(neighbour, targetNode);
					neighbour.Parent = currentNode;

					if(!openList.Contains(neighbour))
						openList.Add(neighbour);
				} 
			}

		}
		return null;
	}

	private static List<Node> RetracePath(Node startNode, Node targetNode)
	{
		List<Node> path = new List<Node>();

		Node currentNode = targetNode;

		while(!(currentNode == startNode))
		{
			path.Add(currentNode);
			currentNode = currentNode.Parent;
		}
		path.Reverse();
		
		return path;
	}

	private static int GetDistance(Node a, Node b)
	{
		int dstX = Mathf.Abs(a.X - b.X);
		int dstY = Mathf.Abs(a.Y - b.Y);

		if(dstX > dstY)
			return 14*dstY + 10*(dstX-dstY);
		return 14*dstX + 10*(dstY-dstX);

	}

	private static List<Node> GetNeighbours(Node n)
	{
		List<Node> neighbours = new List<Node>();
		for(int x = -1; x <= 1; x++)
		{
			for(int y = -1; y <= 1; y++)
			{
				if(x == 0 && y == 0)
					continue;
				int checkX = n.X + x;
				int checkY = n.Y + y;

				if(checkX >= 0 && checkX < mapWidth && checkY >= 0 && checkY < mapHeight)
					neighbours.Add(Map[checkX, checkY].GetComponent<Node>());
			}
		}
		return neighbours;
	}
}
