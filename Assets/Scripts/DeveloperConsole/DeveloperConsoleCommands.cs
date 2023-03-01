using System.Collections.Generic;
using Avocado.DeveloperCheatConsole.Scripts.Core;
using Avocado.DeveloperCheatConsole.Scripts.Core.Commands;
using UnityEngine;

public class DeveloperConsoleCommands : MonoBehaviour
{
	public InputManager inputManager;
	public InventoryManager inventoryManager;
	public ItemManager itemManager;

	private void Start()
	{
		inputManager = InputManager.Instance;
		inventoryManager = InventoryManager.Instance;
		itemManager = ItemManager.Instance;
	}

	private void Awake()
	{
		DeveloperConsole.Instance.AddCommand(new DevCommand("add_item", "adds item with given [id] and (optional = 1)[count]", delegate (int parameter)
		{
			DeveloperConsole.Instance.InvokeCommand("add_item " + parameter + " 1");

			// Debug.Log("success execute command add_item with range number parameters " + parameter + " 1");
		}));
		DeveloperConsole.Instance.AddCommand(new DevCommand("add_item", "adds item with given [id] and (optional = 1)[count]", delegate (List<int> parameters)
		{
			// Debug.Log("success execute command add_item with range number parameters " + string.Join(" ", parameters));
			if (parameters.Count == 1)
			{
				string id = parameters[0].ToString();
				inventoryManager.AddItem(new Item(itemManager.GetItemData(id), 1));
			}
			else if (parameters.Count == 2)
			{
				string id = parameters[0].ToString();
				inventoryManager.AddItem(new Item(itemManager.GetItemData(id), parameters[1]));
			}

		}));

		DeveloperConsole.Instance.AddCommand(new DevCommand("seed", "adds 30 tomato seeds", delegate ()
		{
			inventoryManager.AddItem(new Item(itemManager.GetItemData("101"), 30));

		}));
	}
}