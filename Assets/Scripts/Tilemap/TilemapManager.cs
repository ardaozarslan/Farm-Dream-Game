using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;

public class TilemapManager : Singleton<TilemapManager>
{
	[SerializeField] private TilemapVisual tilemapVisual;
	public Tilemap tilemap;
	private Pathfinding pathfinding;
	private Tilemap.TilemapObject.TilemapSprite tilemapSprite;
	public event EventHandler TilemapLoaded;

	private void Start()
	{
		Debug.Log(PathfindingManager.Instance);
		PathfindingManager.Instance.PathfindingLoaded += InitPathfinding;
		tilemap = new Tilemap(14, 20, 4f);

		tilemap.SetTilemapVisual(tilemapVisual);
		// pathfinding = FindObjectOfType<PathfindingManager>().pathfinding;
		// pathfinding = GameManager.Instance.pathfindingManager.pathfinding;
		TilemapLoaded?.Invoke(this, EventArgs.Empty);

	}

	private void InitPathfinding(object sender, EventArgs e)
	{
		pathfinding = PathfindingManager.Instance.pathfinding;
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPositionWithZ();
			mouseWorldPosition.y = 0;
			tilemap.SetTilemapSprite(mouseWorldPosition, tilemapSprite);

			if (pathfinding != null)
			{
				pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
				switch (tilemapSprite)
				{
					case Tilemap.TilemapObject.TilemapSprite.None:
						pathfinding.GetNode(x, y)?.SetIsWalkable(true);
						break;
					case Tilemap.TilemapObject.TilemapSprite.Ground:
						pathfinding.GetNode(x, y)?.SetIsWalkable(true);
						break;
					case Tilemap.TilemapObject.TilemapSprite.Path:
						pathfinding.GetNode(x, y)?.SetIsWalkable(true);
						break;
					case Tilemap.TilemapObject.TilemapSprite.Dirt:
						pathfinding.GetNode(x, y)?.SetIsWalkable(false);
						break;

					default:
						break;
				}
			}

		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			tilemapSprite = Tilemap.TilemapObject.TilemapSprite.None;
			CMDebug.TextPopupMouse(tilemapSprite.ToString());
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Ground;
			CMDebug.TextPopupMouse(tilemapSprite.ToString());
		}
		if (Input.GetKeyDown(KeyCode.U))
		{
			tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Path;
			CMDebug.TextPopupMouse(tilemapSprite.ToString());
		}
		if (Input.GetKeyDown(KeyCode.I))
		{
			tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Dirt;
			CMDebug.TextPopupMouse(tilemapSprite.ToString());
		}

	}
}
