using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey;

public class GameManager : Singleton<GameManager>
{
	// public static GameManager Instance { get; private set; }
	public event EventHandler OnLoaded;

	public TextMeshProUGUI balanceText;
	public float balance;
	public TilemapManager tilemapManager;
	public PathfindingManager pathfindingManager;
	// public GameObject makerInstance;
	// public GameObject farmerWorkerInstance;
	// public GameObject cashierWorkerInstance;
	// public GameObject cashierRequesterInstance;
	// public GameObject customerRequesterInstance;
	// public MakerScriptable[] allMakerScriptables;

	// public List<Maker> makerList = new List<Maker>();
	// public List<Worker> workerList = new List<Worker>();
	// public List<Requester> requesterList = new List<Requester>();

	// public List<Sprite> workerSpriteList = new List<Sprite>();
	// public List<Sprite> productSpriteList = new List<Sprite>();
	// public Sprite farmerSprite;
	// public Sprite cashierSprite;
	// public Sprite farmerAndCashier;


	// Start is called before the first frame update
	void Start()
	{
		// Instance = this;
		// allMakerScriptables = (MakerScriptable[])Resources.FindObjectsOfTypeAll(typeof(MakerScriptable));

		tilemapManager = FindObjectOfType<TilemapManager>();
		pathfindingManager = FindObjectOfType<PathfindingManager>();
		balance = 0f;
		// balanceText.text = "Balance: $" + balance.ToString("0");

	}

	public void AddBalance(float amount)
	{
		balance += amount;
		Debug.Log("Balance: " + balance);
		balanceText.text = "Balance: $" + balance.ToString("0");
	}

	// public Requester GetEmptyRequester() {
	// 	foreach (Requester requester in requesterList) {
	// 		if (!requester.isReserved || requester.GetCurrentProduct() != MakerScriptable.productTypes.None) {
	// 			return requester;
	// 		}
	// 	}
	// 	// random requester from list
	// 	return requesterList[UnityEngine.Random.Range(0, requesterList.Count)];
	// }

	/*
	 * Save - Load
 	 * */
	public class SaveObject
	{
		public Tilemap.TilemapObject.SaveObject[] tilemapObjectSaveObjectArray;
		// public Maker.SaveObject[] makerSaveObjectArray;
	}

	public void Save()
	{
		List<Tilemap.TilemapObject.SaveObject> tilemapObjectSaveObjectList = new List<Tilemap.TilemapObject.SaveObject>();
		for (int x = 0; x < tilemapManager.tilemap.grid.GetWidth(); x++)
		{
			for (int y = 0; y < tilemapManager.tilemap.grid.GetHeight(); y++)
			{
				Tilemap.TilemapObject tilemapObject = tilemapManager.tilemap.grid.GetGridObject(x, y);
				tilemapObjectSaveObjectList.Add(tilemapObject.Save());
			}
		}

		// List<Maker.SaveObject> makerSaveObjectList = new List<Maker.SaveObject>();
		// for (int x = 0; x < pathfindingManager.pathfinding.grid.GetWidth(); x++)
		// {
		// 	for (int y = 0; y < tilemapManager.tilemap.grid.GetHeight(); y++)
		// 	{
		// 		PathNode pathNode = pathfindingManager.pathfinding.grid.GetGridObject(x, y);
		// 		if (pathNode.hasMaker)
		// 		{
		// 			GameObject maker = pathNode.maker.gameObject;
		// 			makerSaveObjectList.Add(maker.GetComponent<Maker>().Save());
		// 		}

		// 	}
		// }

		SaveObject saveObject = new SaveObject
		{
			tilemapObjectSaveObjectArray = tilemapObjectSaveObjectList.ToArray(),
			// makerSaveObjectArray = makerSaveObjectList.ToArray()
		};

		SaveSystem.SaveObject(saveObject);
	}

	public void Load()
	{
		SaveObject saveObject = SaveSystem.LoadMostRecentObject<SaveObject>();
		if (saveObject != null)
		{
			for (int i = 0; i < saveObject.tilemapObjectSaveObjectArray.Length; i++)
			{
				Tilemap.TilemapObject.SaveObject tilemapObjectSaveObject = saveObject.tilemapObjectSaveObjectArray[i];
				Tilemap.TilemapObject tilemapObject = tilemapManager.tilemap.grid.GetGridObject(tilemapObjectSaveObject.x, tilemapObjectSaveObject.y);
				tilemapObject.Load(tilemapObjectSaveObject);
			}


			for (int x = 0; x < pathfindingManager.pathfinding.grid.GetWidth(); x++)
			{
				for (int y = 0; y < tilemapManager.tilemap.grid.GetHeight(); y++)
				{
					PathNode pathNode = pathfindingManager.pathfinding.grid.GetGridObject(x, y);
					// if (pathNode.hasMaker)
					// {
					// 	GameObject maker = pathNode.maker.gameObject;
					// 	makerList.Remove(maker.GetComponent<Maker>());
					// 	DestroyImmediate(maker);
					// 	pathNode.hasMaker = false;
					// 	pathNode.maker = null;
					// }
				}
			}
			// for (int i = 0; i < saveObject.makerSaveObjectArray.Length; i++)
			// {
			// 	Maker.SaveObject makerSaveObject = saveObject.makerSaveObjectArray[i];
			// 	PathNode pathNode = pathfindingManager.pathfinding.grid.GetGridObject(makerSaveObject.currentX, makerSaveObject.currentY);

			// 	pathNode.GetWorldPosition(out float realX, out float realY);
			// 	pathNode.SetIsWalkable(false);

			// 	GameObject newMaker = Instantiate(makerInstance, new Vector3(realX, realY), Quaternion.identity);
			// 	makerList.Add(newMaker.GetComponent<Maker>());
			// 	newMaker.GetComponent<Maker>().workerPathNodeIndex = makerSaveObject.workerPathNodeIndex;
			// 	newMaker.GetComponent<Maker>().SetCurrentPathNode(pathNode);
			// 	foreach (var makerScriptable in allMakerScriptables)
			// 	{
			// 		if (makerScriptable.product == makerSaveObject.productType)
			// 		{
			// 			newMaker.GetComponent<Maker>().properties = makerScriptable;
			// 			break;
			// 		}
			// 	}
			// 	pathNode.SetMaker(newMaker.GetComponent<Maker>());
			// }
		}
		OnLoaded?.Invoke(this, EventArgs.Empty);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			Save();
			CMDebug.TextPopupMouse("Saved!");
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			Load();
			CMDebug.TextPopupMouse("Loaded!");
		}
		if (Input.GetKeyDown(KeyCode.H))
		{
			pathfindingManager.pathfinding.pathfindingDebugObject.SetActive(!pathfindingManager.pathfinding.pathfindingDebugObject.activeSelf);
		}
	}
}
