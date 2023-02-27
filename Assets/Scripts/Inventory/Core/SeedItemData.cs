using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedItemData", menuName = "ScriptableObjects/SeedItemData", order = 1)]
public class SeedItemData : ItemData
{
	public override void OnUse()
	{
		Debug.Log("Using seed: " + name);
	}
}
