using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiomeManager : MonoBehaviour
{
	[SerializeField]
	private Material mat1;
	[SerializeField]
	private Material mat2;

	private Image _image;

	[SerializeField, Range(0,1)]
	private float blend = 0;

    // Start is called before the first frame update
    private void Start()
    {
	    _image = GetComponent<Image>();
	    _image.material = Instantiate(mat1);
    }

    // Update is called once per frame
    private void Update()
    {
		_image.material.Lerp(mat1, mat2, blend);
    }
}
