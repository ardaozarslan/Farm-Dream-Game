using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
	public Grid<PathNode> grid;
	public int x;
	public int y;


	public int gCost;
	public int hCost;
	public int fCost;

	public bool isWalkable;
	public bool hasMaker;
	public bool hasRequester;
	public PathNode cameFromNode;
	public List<PathNode> neighbourList;

	public PathNode(Grid<PathNode> grid, int x, int y)
	{
		this.grid = grid;
		this.x = x;
		this.y = y;
		this.isWalkable = true;
		this.hasRequester = false;
		this.hasMaker = false;
	}

	public void CalculateFCost()
	{
		fCost = gCost + hCost;
	}

	public void SetIsWalkable(bool isWalkable)
	{
		this.isWalkable = isWalkable;
		grid.TriggerGridObjectChanged(x, y);
	}

	public void SetIsWalkable() {
		bool checkBool = true;
		if (hasMaker || Tilemap.Instance.grid.GetGridObject(x, y).tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Dirt) {
			checkBool = false;
		}
		SetIsWalkable(checkBool);

	}

	// public void SetMaker(Maker _maker)
	// {
	// 	this.hasMaker = true;
	// 	this.maker = _maker;
	// 	grid.TriggerGridObjectChanged(x, y);
	// }

	// public void SetRequester(Requester _requester)
	// {
	// 	this.hasRequester = true;
	// 	this.requester = _requester;
	// 	grid.TriggerGridObjectChanged(x, y);
	// }

	public void GetWorldPosition(out float realX, out float realY)
	{
		realX = this.x * grid.cellSize + grid.originPosition.x + grid.cellSize * 0.5f;
		realY = this.y * grid.cellSize + grid.originPosition.y + grid.cellSize * 0.5f;
	}

	public override string ToString()
	{
		return x + ", " + y;
	}

	// [System.Serializable]
	// public class SaveObject
	// {
	// 	public bool hasMaker;
	// 	public int x;
	// 	public int y;
	// }

	// /*
	//  * Save - Load
	//  * */
	// public SaveObject Save()
	// {
	// 	return new SaveObject
	// 	{
	// 		hasMaker = hasMaker,
	// 		x = x,
	// 		y = y,
	// 	};
	// }

	// public void Load(SaveObject saveObject)
	// {
	// 	hasMaker = saveObject.hasMaker;
	// }
}
