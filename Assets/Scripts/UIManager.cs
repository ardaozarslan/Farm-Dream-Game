using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
	public GameObject inventoryPanel;
	public InputManager inputManager;
	public ManagersManager managersManager;


	private void Start()
	{
		managersManager = ManagersManager.Instance;
		inputManager = InputManager.Instance;
	}

	public void TriggerInventoryPanel()
	{
		UIManager.Instance.inventoryPanel.SetActive(!UIManager.Instance.inventoryPanel.activeSelf);
		if (UIManager.Instance.inventoryPanel.activeSelf)
		{
			inputManager.SwitchActionMap(inputManager.playerInputActions.Inventory);
		}
		else
		{
			inputManager.SwitchActionMap(inputManager.playerInputActions.Player);
		}
	}
}
