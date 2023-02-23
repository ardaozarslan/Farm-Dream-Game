using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersManager : Singleton<ManagersManager>
{
	public InputManager inputManager;
	public UIManager uiManager;
	public ItemManager itemManager;
	public InventoryManager inventoryManager;

	private void Start() {
		inputManager = InputManager.Instance;
		uiManager = UIManager.Instance;
		itemManager = ItemManager.Instance;
		inventoryManager = InventoryManager.Instance;
	}
}
