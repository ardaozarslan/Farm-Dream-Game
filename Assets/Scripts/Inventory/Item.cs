using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public string name;
	public int id;
	public ItemType itemType;
    public string description;
    public Sprite sprite;
    public int stackSize;
	public int maxStackSize;

	public enum ItemType {
		Seed = 100,
		Crop = 200,
		Tool = 300,
		Consumable = 400,
		Other = 500
	}

    public Item(ItemData itemData, int stackSize = 1) 
    {
        this.name = itemData.name;
		this.id = itemData.id;
		this.itemType = itemData.itemType;
        this.description = itemData.description;
        this.sprite = itemData.sprite;
        this.stackSize = stackSize;
		this.maxStackSize = itemData.maxStackSize;
    }

	public Item(Item item, int stackSize = 1) 
	{
		this.name = item.name;
		this.id = item.id;
		this.itemType = item.itemType;
		this.description = item.description;
		this.sprite = item.sprite;
		this.stackSize = stackSize;
		this.maxStackSize = item.maxStackSize;
	}
}
