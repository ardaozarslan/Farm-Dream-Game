using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
	private Player player;
	private InputManager inputManager;
	private ItemManager itemManager;

	private void OnEnable()
	{
		player = GetComponent<Player>();
	}

	private void Start()
	{
		inputManager = InputManager.Instance;
		itemManager = ItemManager.Instance;
		inputManager.playerInputActions.Inventory.Inventory.performed += Inventory;
	}

	private void OnDisable()
	{
		inputManager.playerInputActions.Inventory.Inventory.performed -= Inventory;
	}


	private void Inventory(InputAction.CallbackContext context)
	{
		// Debug.Log("Inventory!");
		UIManager.Instance.TriggerInventoryPanel();
	}

	private void Update() {
		if (Keyboard.current.oKey.wasPressedThisFrame) {
			// Debug.Log("O key was pressed");
			player.mainInventory.AddItem(new Item(itemManager.GetItemData("Wheat"), 10));
		}
	}
}
