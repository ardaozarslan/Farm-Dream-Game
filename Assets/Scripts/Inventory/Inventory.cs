using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	private List<Item> items = new List<Item>();
	private List<Item> hotbarItems = new List<Item>();

	public void AddItem(Item item)
	{
		bool stacked = false;
		foreach (Item i in items)
		{
			if (i.name == item.name && i.stackSize < i.maxStackSize)
			{
				i.stackSize += item.stackSize;
				stacked = true;
				break;
			}
		}
		if (!stacked)
		{
			items.Add(item);
		}
	}

	public void RemoveItem(Item item)
	{
		items.Remove(item);
	}

	public void SortByName()
	{
		items.Sort((x, y) => string.Compare(x.name, y.name));
	}

	public void SortByStackSize()
	{
		items.Sort((x, y) => x.stackSize.CompareTo(y.stackSize));
	}
}