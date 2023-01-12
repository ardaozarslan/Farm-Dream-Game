using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap
{
	public static Tilemap Instance { get; private set; }
	public Grid<TilemapObject> grid;

	public Tilemap(int width, int height, float cellSize)
	{
		Instance = this;
		GameObject tilemapDebugObject = new GameObject("TilemapDebug");
		tilemapDebugObject.transform.parent = GameObject.Find("Tilemap").transform;
		grid = new Grid<TilemapObject>(width, height, cellSize, (Grid<TilemapObject> g, int x, int y) => new TilemapObject(g, x, y), tilemapDebugObject, false);
		tilemapDebugObject.SetActive(false);

		FindNeighbors();
	}

	public void SetTilemapSprite(Vector3 worldposition, TilemapObject.TilemapSprite tilemapSprite)
	{
		TilemapObject tilemapObject = grid.GetGridObject(worldposition);
		if (tilemapObject != null)
		{
			tilemapObject.SetTilemapSprite(tilemapSprite);
		}
	}

	public void SetTilemapVisual(TilemapVisual tilemapVisual)
	{
		tilemapVisual.SetGrid(this, grid);
	}

	private void FindNeighbors()
	{
		for (int x = 0; x < grid.GetWidth(); x++)
		{
			for (int y = 0; y < grid.GetHeight(); y++)
			{
				TilemapObject currentNode = grid.GetGridObject(x, y);
				List<TilemapObject> neighbourList = new List<TilemapObject>();
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


	public class TilemapObject
	{

		public enum TilemapSprite
		{
			None,
			Ground,
			Path,
			Dirt,
		}

		public Grid<TilemapObject> grid;
		public int x;
		public int y;
		public TilemapSprite tilemapSprite;
		public List<TilemapObject> neighbourList;

		public TilemapObject(Grid<TilemapObject> grid, int x, int y)
		{
			this.grid = grid;
			this.x = x;
			this.y = y;
		}

		public void SetTilemapSprite(TilemapSprite tilemapSprite)
		{
			this.tilemapSprite = tilemapSprite;
			grid.TriggerGridObjectChanged(x, y);
		}

		public TilemapSprite GetTilemapSprite()
		{
			return tilemapSprite;
		}

		public override string ToString()
		{
			return tilemapSprite.ToString();
		}

		[System.Serializable]
		public class SaveObject
		{
			public TilemapSprite tilemapSprite;
			public int x;
			public int y;
		}

		/*
		 * Save - Load
		 * */
		public SaveObject Save()
		{
			return new SaveObject
			{
				tilemapSprite = tilemapSprite,
				x = x,
				y = y,
			};
		}

		public void Load(SaveObject saveObject)
		{
			SetTilemapSprite(saveObject.tilemapSprite);
			// tilemapSprite = saveObject.tilemapSprite;
		}
	}
}
