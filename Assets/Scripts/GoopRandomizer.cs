using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class designed to randomize the goop shader for each ship
/// </summary>
public class GoopRandomizer : MonoBehaviour
{
	[SerializeField]
	private Material goopShader;

	[SerializeField]
	private MeshRenderer shipMeshRenderer;

	[SerializeField]
	private ParticleSystemRenderer goopBubbles;

	void Start()
	{
		if (goopShader == null)
			return;

		Material randomizedGoopShader = Instantiate(goopShader);
		randomizedGoopShader.SetFloat("Vector1_c688aac08c1b4dee94f25807b45645e8", Random.Range(0f,2f));
		List<Material> materialList = new List<Material>();
		materialList.Add(shipMeshRenderer.materials[0]);
		materialList.Add(randomizedGoopShader);
		shipMeshRenderer.materials = materialList.ToArray();
		goopBubbles.material = randomizedGoopShader;
	}
}
