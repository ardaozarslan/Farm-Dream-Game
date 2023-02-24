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
		CanvasGroup canvasGroup = inventoryPanel.GetComponent<CanvasGroup>();
		if (canvasGroup.alpha == 0)
		{
			canvasGroup.alpha = 1;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			inputManager.SwitchActionMap(inputManager.playerInputActions.Inventory);
		}
		else
		{
			canvasGroup.alpha = 0;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
			inputManager.SwitchActionMap(inputManager.playerInputActions.Player);
		}
		// inventoryPanel.SetActive(!inventoryPanel.activeSelf);
		// if (inventoryPanel.activeSelf)
		// {
		// 	inputManager.SwitchActionMap(inputManager.playerInputActions.Inventory);
		// }
		// else
		// {
		// 	inputManager.SwitchActionMap(inputManager.playerInputActions.Player);
		// }
	}
}
