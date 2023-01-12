using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{

	private const int MOVE_STRAIGHT_COST = 10;
	private const int MOVE_DIAGONAL_COST = 14;

	public static Pathfinding Instance { get; private set; }
	public Grid<PathNode> grid;
	private List<PathNode> openList;
	private List<PathNode> closedList;
	public GameObject pathfindingDebugObject;


	public Pathfinding(int width, int height, float cellSize)
	{
		Instance = this;
		pathfindingDebugObject = new GameObject("PathfindingDebug");
		pathfindingDebugObject.transform.parent = GameObject.Find("Pathfinding").transform;
		grid = new Grid<PathNode>(width, height, cellSize, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y), pathfindingDebugObject, true);
		foreach (var debugLine in grid.debugLinesList)
		{
			debugLine.transform.parent = pathfindingDebugObject.transform;
		}
		pathfindingDebugObject.SetActive(false);

		FindNeighbors();

	}

	public Grid<PathNode> GetGrid()
	{
		return grid;
	}

	public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
	{
		grid.GetXY(startWorldPosition, out int startX, out int startY);
		grid.GetXY(endWorldPosition, out int endX, out int endY);

		List<PathNode> path = FindPath(startX, startY, endX, endY);
		if (path == null)
		{
			return null;
		}
		else
		{
			List<Vector3> vectorPath = new List<Vector3>();
			foreach (var pathNode in path)
			{
				vectorPath.Add(grid.GetWorldPosition(pathNode.x, pathNode.y, true));
			}
			return vectorPath;
		}
	}

	public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
	{
		PathNode startNode = grid.GetGridObject(startX, startY);
		PathNode endNode = grid.GetGridObject(endX, endY);

		openList = new List<PathNode> { startNode };
		closedList = new List<PathNode>();


		for (int x = 0; x < grid.GetWidth(); x++)
		{
			for (int y = 0; y < grid.GetHeight(); y++)
			{
				PathNode pathNode = grid.GetGridObject(x, y);
				pathNode.gCost = int.MaxValue;
				pathNode.CalculateFCost();
				pathNode.cameFromNode = null;
			}
		}

		startNode.gCost = 0;
		startNode.hCost = CalculateDistanceCost(startNode, endNode);
		startNode.CalculateFCost();

		while (openList.Count > 0)
		{
			PathNode currentNode = GetLowestFCostNode(openList);
			if (currentNode == endNode)
			{
				return CalculatePath(endNode);
			}

			openList.Remove(currentNode);
			closedList.Add(currentNode);

			foreach (PathNode neighbourNode in currentNode.neighbourList)
			{
				if (closedList.Contains(neighbourNode))
				{
					continue;
				}
				if (!neighbourNode.isWalkable)
				{
					closedList.Add(neighbourNode);
					continue;
				}
				int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
				if (tentativeGCost < neighbourNode.gCost)
				{
					neighbourNode.cameFromNode = currentNode;
					neighbourNode.gCost = tentativeGCost;
					neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
					neighbourNode.CalculateFCost();

					if (!openList.Contains(neighbourNode))
					{
						openList.Add(neighbourNode);
					}
				}
			}

		}

		// Out of nodes on the openList
		return null;

	}

	private void FindNeighbors()
	{
		for (int x = 0; x < grid.GetWidth(); x++)
		{
			for (int y = 0; y < grid.GetHeight(); y++)
			{
				PathNode currentNode = grid.GetGridObject(x, y);
				List<PathNode> neighbourList = new List<PathNode>();
				if (currentNode.x - 1 >= 0)
				{
					// Left Down
					if (currentNode.y - 1 >= 0)
					{
						neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y - 1));
					}
					// Left
					neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y));
					//Left Up
					if (currentNode.y + 1 < grid.GetHeight())
					{
						neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y + 1));
					}
				}
				// Up
				if (currentNode.y + 1 < grid.GetHeight())
				{
					neighbourList.Add(grid.GetGridObject(currentNode.x, currentNode.y + 1));
				}
				if (currentNode.x + 1 < grid.GetWidth())
				{
					// Right Up
					if (currentNode.y + 1 < grid.GetHeight())
					{
						neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y + 1));
					}
					// Right
					neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y));
					// Right Down
					if (currentNode.y - 1 >= 0)
					{
						neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y - 1));
					}

				}
				// Down
				if (currentNode.y - 1 >= 0)
				{
					neighbourList.Add(grid.GetGridObject(currentNode.x, currentNode.y - 1));
				}

				currentNode.neighbourList = neighbourList;

			}
		}

	}

	public PathNode GetNode(int x, int y)
	{
		return grid.GetGridObject(x, y);
	}

	private List<PathNode> CalculatePath(PathNode endNode)
	{
		List<PathNode> path = new List<PathNode>();
		path.Add(endNode);
		PathNode currentNode = endNode;

		while (currentNode.cameFromNode != null)
		{
			path.Add(currentNode.cameFromNode);
			currentNode = currentNode.cameFromNode;
		}
		path.Reverse();
		return path;
	}

	private int CalculateDistanceCost(PathNode a, PathNode b)
	{
		int xDistance = Mathf.Abs(a.x - b.x);
		int yDistance = Mathf.Abs(a.y - b.y);
		int remaining = Mathf.Abs(xDistance - yDistance);
		return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
	}

	private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
	{
		PathNode lowestFCostNode = pathNodeList[0];
		for (int i = 1; i < pathNodeList.Count; i++)
		{
			if (pathNodeList[i].fCost < lowestFCostNode.fCost)
			{
				lowestFCostNode = pathNodeList[i];
			}
		}
		return lowestFCostNode;
	}
}
