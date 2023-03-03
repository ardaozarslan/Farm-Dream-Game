using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
	public FarmField farmField;
	public Item seedItem;
	public List<GameObject> plantScaleModels = new List<GameObject>();

	public void Initialize(FarmField _farmField, Item _seedItem)
	{
		farmField = _farmField;
		farmField.plant = this;
		seedItem = _seedItem;


		foreach (GameObject child in plantScaleModels)
		{
			child.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
			child.transform.localScale = Vector3.zero;
			StartCoroutine(GrowIndividualModel(child.transform));
		}
		StartCoroutine(Grow());
	}

	private IEnumerator GrowIndividualModel(Transform childTransform)
	{
		float growTime = (seedItem.ItemData as SeedItemData).GetGrowTime() + Random.Range(-(seedItem.ItemData as SeedItemData).GetGrowTimeVariance(), 0);
		float growScale = (seedItem.ItemData as SeedItemData).GetGrowScale() + Random.Range(-(seedItem.ItemData as SeedItemData).GetGrowScaleVariance(), (seedItem.ItemData as SeedItemData).GetGrowScaleVariance());
		Vector3 growScaleVector = Vector3.one * growScale;
		float timer = 0;
		while (timer < growTime)
		{
			timer += Time.deltaTime;
			childTransform.localScale = growScaleVector * (timer / growTime);
			yield return null;
		}
		childTransform.localScale = growScaleVector;
	}

	private IEnumerator Grow()
	{
		farmField.farmState = FarmField.FarmState.Growing;
		float growTime = (seedItem.ItemData as SeedItemData).GetGrowTime();
		yield return new WaitForSeconds(growTime);
		farmField.farmState = FarmField.FarmState.ReadyToHarvest;
		Utils.CreateWorldTextPopup("Ready to\nharvest!", transform, Vector3.one * 0.2f);
	}

	public void Harvest()
	{
		int harvestAmount = UnityEngine.Random.Range((seedItem.ItemData as SeedItemData).GetHarvestAmount()[0], (seedItem.ItemData as SeedItemData).GetHarvestAmount()[1] + 1);
		int returnSeedAmount = UnityEngine.Random.Range((seedItem.ItemData as SeedItemData).GetReturnSeedAmount()[0], (seedItem.ItemData as SeedItemData).GetReturnSeedAmount()[1] + 1);
		InventoryManager.Instance.AddItem(new Item((seedItem.ItemData as SeedItemData).GetHarvestItemData(), harvestAmount));
		InventoryManager.Instance.AddItem(new Item(seedItem.ItemData, returnSeedAmount));
		
		farmField.farmState = FarmField.FarmState.Empty;
		farmField.plant = null;
		Destroy(gameObject);
	}
}
