using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FarmFieldHighlight : MonoBehaviour
{
	// private Material material;

	private void Start()
	{
		// MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		// CombineInstance[] combine = new CombineInstance[meshFilters.Length];

		// int i = 0;
		// while (i < meshFilters.Length)
		// {
		// 	combine[i].mesh = meshFilters[i].sharedMesh;
		// 	combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
		// 	material = meshFilters[i].GetComponent<MeshRenderer>().material;
		// 	meshFilters[i].gameObject.SetActive(false);

		// 	i++;
		// }
		// transform.GetComponent<MeshFilter>().mesh = new Mesh();
		// transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		// if (material != null)
		// {
		// 	transform.GetComponent<MeshRenderer>().material = material;
		// 	material = transform.GetComponent<MeshRenderer>().material;
		// }

		// transform.gameObject.SetActive(true);
	}
}
