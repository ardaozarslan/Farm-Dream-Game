using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmField : MonoBehaviour
{
	FarmFieldHighlight farmFieldHighlight;
    // Start is called before the first frame update
    void Start()
    {
        farmFieldHighlight = GetComponentInChildren<FarmFieldHighlight>();
		ActivateHighlight(false);
    }

	public void ActivateHighlight(bool _activate)
	{
		farmFieldHighlight.gameObject.SetActive(_activate);
	}
}
