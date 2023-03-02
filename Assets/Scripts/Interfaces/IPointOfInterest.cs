using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPointOfInterest
{
	void ActivateHighlight(bool _activate);
	GameObject GetGameObject();

	enum InputType
	{
		Tap = 0,
		Hold = 10
	}
}
