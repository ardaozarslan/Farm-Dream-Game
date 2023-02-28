using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
	public List<ItemData> itemDataObjects;
	public ManagersManager managersManager;
    // Start is called before the first frame update
    void Start()
    {
		managersManager = ManagersManager.Instance;
        itemDataObjects = new List<ItemData>(Resources.LoadAll<ItemData>("ItemData"));
    }

	public ItemData GetItemData(string id) {
		return itemDataObjects.Find(itemData => itemData.id == id);
	}

	// public ItemData GetItemData(string name) {
	// 	return itemDataObjects.Find(itemData => itemData.name == name);
	// }

    // Update is called once per frame
    void Update()
    {
        
    }
}
