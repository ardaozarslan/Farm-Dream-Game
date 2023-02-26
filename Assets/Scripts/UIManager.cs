using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
	public GameObject inventoryPanel;
	public GameObject hotbarToolbarPanel;
	public InputManager inputManager;
	public ManagersManager managersManager;


	private void Start()
	{
		managersManager = ManagersManager.Instance;
		inputManager = InputManager.Instance;
	}

	public void TriggerInventoryPanel()
	{
		CanvasGroup inventoryCanvasGroup = inventoryPanel.GetComponent<CanvasGroup>();
		CanvasGroup hotbarCanvasGroup = hotbarToolbarPanel.GetComponent<CanvasGroup>();
		if (inventoryCanvasGroup.alpha == 0)
		{
			inventoryCanvasGroup.alpha = 1;
			inventoryCanvasGroup.interactable = true;
			inventoryCanvasGroup.blocksRaycasts = true;

			hotbarCanvasGroup.alpha = 0;
			hotbarCanvasGroup.interactable = false;
			hotbarCanvasGroup.blocksRaycasts = false;

			inputManager.SwitchActionMap(inputManager.playerInputActions.Inventory);
		}
		else
		{
			inventoryCanvasGroup.alpha = 0;
			inventoryCanvasGroup.interactable = false;
			inventoryCanvasGroup.blocksRaycasts = false;

			hotbarCanvasGroup.alpha = 1;
			hotbarCanvasGroup.interactable = true;
			hotbarCanvasGroup.blocksRaycasts = true;

			inputManager.SwitchActionMap(inputManager.playerInputActions.Player);
		}
	}
}
