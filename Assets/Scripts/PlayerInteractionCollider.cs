using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionCollider : MonoBehaviour
{

	public Player player;
	public Collider playerInteractionCollider;

	private void Awake()
	{
		player = Player.Instance;
		playerInteractionCollider = GetComponent<Collider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("OnTriggerEnter: " + other.name);
		if (other.CompareTag("Interactable"))
		{
			IPointOfInterest pointOfInterest = other.GetComponent<IPointOfInterest>();
			player.AddPointOfInterest(pointOfInterest);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Interactable"))
		{
			IPointOfInterest pointOfInterest = other.GetComponent<IPointOfInterest>();
			player.RemovePointOfInterest(pointOfInterest);
		}
	}
}