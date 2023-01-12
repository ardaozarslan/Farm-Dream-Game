using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid<TGridObject>
{

	public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
	public class OnGridObjectChangedEventArgs : EventArgs
	{
		public int x;
		public int y;
	}

	private int width;
	private int height;
	public float cellSize;
	public Vector3 originPosition;
	private TGridObject[,] gridArray;
	public GameObject debugObject;
	public List<GameObject> debugLinesList;

	private GameObject DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
	{
		GameObject myLine = new GameObject();
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Standard Unlit"));
		lr.startColor = color;
		lr.endColor = color;
		// lr.SetColors(color, color);
		// lr.SetWidth(0.1f, 0.1f);
		lr.startWidth = 0.3f;
		lr.endWidth = 0.3f;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		if (duration > 0)
		{
			GameObject.Destroy(myLine, duration);
		}
		return myLine;
	}

	public Grid(int width, int height, float cellSize, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, GameObject _debugObject, bool showDebug = true)
	{
		this.width = width;
		this.height = height;
		this.cellSize = cellSize;
		this.originPosition = new Vector3(-width * cellSize * 0.5f, 0, -height * cellSize * 0.5f);
		this.debugObject = _debugObject;
		this.debugLinesList = new List<GameObject>();

		gridArray = new TGridObject[width, height];

		for (int x = 0; x < gridArray.GetLength(0); x++)
		{
			for (int y = 0; y < gridArray.GetLength(1); y++)
			{
				gridArray[x, y] = createGridObject(this, x, y);
			}
		}

		// bool showDebug = true;
		if (showDebug)
		{
			TextMesh[,] debugTextArray = new TextMesh[width, height];

			for (int x = 0; x < gridArray.GetLength(0); x++)
			{
				for (int y = 0; y < gridArray.GetLength(1); y++)
				{
					debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, Mathf.FloorToInt(this.cellSize * 3.5f), Color.white, TextAnchor.MiddleCenter);
					debugTextArray[x, y].transform.parent = debugObject.transform;
					// this.debugLinesList.Add(DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 0));
					// this.debugLinesList.Add(DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 0));
					Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 10000000000f);
					Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 10000000000f);
				}
			}
			// this.debugLinesList.Add(DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 0));
			// this.debugLinesList.Add(DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 0));
			Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 10000000000f);
			Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 10000000000f);

			OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
			{
				debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
			};
		}
	}

	public int GetWidth()
	{
		return width;
	}

	public int GetHeight()
	{
		return height;
	}

	public float GetCellSize()
	{
		return cellSize;
	}

	public Vector3 GetWorldPosition(int x, int y, bool atCenter = false)
	{
		if (atCenter) {
			return new Vector3(x * cellSize + cellSize * 0.5f, 0, y * cellSize + cellSize * 0.5f) + originPosition;
		} else {
			return new Vector3(x * cellSize, 0,  y * cellSize) + originPosition;
		}
	}

	public void GetXY(Vector3 worldPosition, out int x, out int y)
	{
		x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
		y = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
	}

	public void SetGridObject(int x, int y, TGridObject gridObject)
	{
		if (x >= 0 && y >= 0 && x < width && y < height)
		{
			gridArray[x, y] = gridObject;
			if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
		}
	}

	public void SetGridObject(Vector3 worldPosition, TGridObject gridObject)
	{
		int x, y;
		GetXY(worldPosition, out x, out y);
		SetGridObject(x, y, gridObject);
	}

	public void TriggerGridObjectChanged(int x, int y)
	{
		if (OnGridObjectChanged != null)
		{
			OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
		}
	}

	public TGridObject GetGridObject(int x, int y)
	{
		if (x >= 0 && y >= 0 && x < width && y < height)
		{
			return gridArray[x, y];
		}
		else
		{
			return default(TGridObject);
		}
	}

	public TGridObject GetGridObject(Vector3 worldPosition)
	{
		int x, y;
		GetXY(worldPosition, out x, out y);
		return GetGridObject(x, y);
	}

}

public class TGridObject
{
	private Grid<PathNode> grid;
	public int x;
	public int y;

	public TGridObject(Grid<PathNode> grid, int x, int y)
	{
		this.grid = grid;
		this.x = x;
		this.y = y;
	}
}
