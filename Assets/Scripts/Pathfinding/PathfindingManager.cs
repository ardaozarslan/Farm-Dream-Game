using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PathfindingManager : Singleton<PathfindingManager>
{
	[SerializeField] private PathfindingVisual pathfindingVisual;
	public Pathfinding pathfinding;

	public Vector3 lastPosition = new Vector3(0, 0, 0);
	public GameManager gameManager;
	public event EventHandler PathfindingLoaded;

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
		pathfinding = new Pathfinding(14, 20, 4f);
		PathfindingLoaded?.Invoke(this, EventArgs.Empty);
		// pathfindingVisual.SetGrid(pathfinding.GetGrid());
	}

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
		lr.startWidth = 0.15f;
		lr.endWidth = 0.15f;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		if (duration > 0)
		{
			GameObject.Destroy(myLine, duration);
		}
		return myLine;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
			pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
			// pathfinding.GetGrid().GetXY(lastPosition, out int startX, out int startY);
			List<PathNode> path = pathfinding.FindPath(Mathf.FloorToInt(lastPosition.x), Mathf.FloorToInt(lastPosition.y), x, y);
			if (path != null)
			{
				for (int i = 0; i < path.Count - 1; i++)
				{
					path[i].GetWorldPosition(out float currentX, out float currentY);
					path[i + 1].GetWorldPosition(out float nextX, out float nextY);
					// GameObject newPathLine = DrawLine(new Vector3(currentX, currentY), new Vector3(nextX, nextY), Color.red, 5f);
					// pathfinding.grid.debugLinesList.Add(newPathLine);
					// newPathLine.transform.parent = pathfinding.pathfindingDebugObject.transform;
					Debug.DrawLine(new Vector3(currentX, currentY), new Vector3(nextX, nextY), Color.red, 5f);
				}
				lastPosition = new Vector3(x, y);
			}
		}
		// if (Input.GetMouseButtonDown(1))
		// {
		// 	Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
		// 	pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
		// 	pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
		// }


		// Maker Instance
		// if (Input.GetKeyDown(KeyCode.A))
		// {
		// 	Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
		// 	pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
		// 	if (!pathfinding.GetNode(x, y).hasMaker && !pathfinding.GetNode(x, y).hasRequester)
		// 	{
		// 		pathfinding.GetNode(x, y).GetWorldPosition(out float realX, out float realY);
		// 		pathfinding.GetNode(x, y).SetIsWalkable(false);

		// 		GameObject newMaker = Instantiate(gameManager.makerInstance, new Vector3(realX, realY), Quaternion.identity);
		// 		gameManager.makerList.Add(newMaker.GetComponent<Maker>());
		// 		newMaker.GetComponent<Maker>().SetCurrentPathNode(pathfinding.GetNode(x, y));
		// 		pathfinding.GetNode(x, y).SetMaker(newMaker.GetComponent<Maker>());
		// 	}
		// }

		// Farmer Worker Instance
		// if (Input.GetKeyDown(KeyCode.C))
		// {
		// 	Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
		// 	pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
		// 	if (!pathfinding.GetNode(x, y).hasMaker && !pathfinding.GetNode(x, y).hasRequester && pathfinding.GetNode(x, y).isWalkable)
		// 	{
		// 		pathfinding.GetNode(x, y).GetWorldPosition(out float realX, out float realY);
		// 		// pathfinding.GetNode(x, y).SetIsWalkable(false);

		// 		GameObject newWorker = Instantiate(gameManager.farmerWorkerInstance, new Vector3(realX, realY), Quaternion.identity);
		// 		gameManager.workerList.Add(newWorker.GetComponent<Worker>());
		// 		newWorker.GetComponent<Worker>().SetCurrentPathNode(pathfinding.GetNode(x, y));
		// 		// pathfinding.GetNode(x, y).SetMaker(newMaker);
		// 	}
		// }


		// Cashier Worker Instance
		// if (Input.GetKeyDown(KeyCode.V))
		// {
		// 	Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
		// 	pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
		// 	if (!pathfinding.GetNode(x, y).hasMaker && !pathfinding.GetNode(x, y).hasRequester && pathfinding.GetNode(x, y).isWalkable)
		// 	{
		// 		pathfinding.GetNode(x, y).GetWorldPosition(out float realX, out float realY);
		// 		// pathfinding.GetNode(x, y).SetIsWalkable(false);

		// 		GameObject newWorker = Instantiate(gameManager.cashierWorkerInstance, new Vector3(realX, realY), Quaternion.identity);
		// 		gameManager.workerList.Add(newWorker.GetComponent<Worker>());
		// 		newWorker.GetComponent<Worker>().SetCurrentPathNode(pathfinding.GetNode(x, y));
		// 		// pathfinding.GetNode(x, y).SetMaker(newMaker);
		// 	}
		// }


		// Cashier Requester Instance
		// if (Input.GetKeyDown(KeyCode.Z))
		// {
		// 	Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
		// 	pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
		// 	if (!pathfinding.GetNode(x, y).hasMaker && !pathfinding.GetNode(x, y).hasRequester)
		// 	{
		// 		pathfinding.GetNode(x, y).GetWorldPosition(out float realX, out float realY);
		// 		// pathfinding.GetNode(x, y).SetIsWalkable(false);

		// 		GameObject newRequester = Instantiate(gameManager.cashierRequesterInstance, new Vector3(realX, realY), Quaternion.identity);
		// 		gameManager.requesterList.Add(newRequester.GetComponent<Requester>());
		// 		newRequester.GetComponent<Requester>().SetCurrentPathNode(pathfinding.GetNode(x, y));
		// 		pathfinding.GetNode(x, y).SetRequester(newRequester.GetComponent<Requester>());
		// 	}
		// }

		// Customer Requester Instance
		// if (Input.GetKeyDown(KeyCode.X))
		// {
		// 	Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
		// 	pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
		// 	if (!pathfinding.GetNode(x, y).hasMaker && !pathfinding.GetNode(x, y).hasRequester)
		// 	{
		// 		pathfinding.GetNode(x, y).GetWorldPosition(out float realX, out float realY);
		// 		// pathfinding.GetNode(x, y).SetIsWalkable(false);

		// 		GameObject newRequester = Instantiate(gameManager.customerRequesterInstance, new Vector3(realX, realY), Quaternion.identity);
		// 		gameManager.requesterList.Add(newRequester.GetComponent<Requester>());
		// 		newRequester.GetComponent<Requester>().SetCurrentPathNode(pathfinding.GetNode(x, y));
		// 		pathfinding.GetNode(x, y).SetRequester(newRequester.GetComponent<Requester>());
		// 	}
		// }
	}
}
