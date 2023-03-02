using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable : IPointOfInterest
{
	bool Interact(InputType type, bool _isCheckOnly = false);
	// string CanInteract(IPointOfInterest.InputType type);
}
