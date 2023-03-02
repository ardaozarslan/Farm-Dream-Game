using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemUsable : IPointOfInterest
{
	void UseItemOnThis(Item item);
}
