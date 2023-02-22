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
		Seed,
		Tool,
		Consumable,
		Other
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
}
