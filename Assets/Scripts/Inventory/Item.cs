using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item
{
	public string name;
	public string id;
	public ItemType itemType;
	public string description;
	public Sprite sprite;
	public int stackSize;
	public int maxStackSize;
	public bool isStackable;
	private ItemData itemData;
	public ItemData ItemData { get { return itemData; } }

	public enum ItemType
	{
		Tool = 100,
		Crop = 200,
		Seed = 300,
		Consumable = 400,
		Other = 500
	}

	public Item(ItemData itemData, int stackSize = 1)
	{
		this.name = itemData.name;
		this.itemType = itemData.itemType;
		this.description = itemData.description;
		this.sprite = itemData.sprite;
		this.stackSize = stackSize;
		this.maxStackSize = itemData.maxStackSize;
		this.isStackable = itemData.isStackable;
		this.itemData = itemData;

		if (!itemData.isStackable)
		{
			this.stackSize = 1;
			System.Guid guid = System.Guid.NewGuid();
			string guidString = guid.ToString();
			this.id = itemData.id + ":" + guidString;
		}
		else {
			this.id = itemData.id.ToString();
		}


	}

	public Item(Item item, int stackSize = 1)
	{
		this.name = item.name;
		this.itemType = item.itemType;
		this.description = item.description;
		this.sprite = item.sprite;
		this.stackSize = stackSize;
		this.maxStackSize = item.maxStackSize;
		this.isStackable = item.isStackable;
		this.itemData = item.itemData;

		if (!itemData.isStackable)
		{
			this.stackSize = 1;
			System.Guid guid = System.Guid.NewGuid();
			string guidString = guid.ToString();
			this.id = item.id + ":" + guidString;
		}
		else {
			this.id = item.id.ToString();
		}
	}
}
