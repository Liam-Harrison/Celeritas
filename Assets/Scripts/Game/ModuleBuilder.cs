using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Celeritas.Extensions;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

public class ModuleBuilder : SerializedMonoBehaviour
{
	[SerializeField, AssetList, InlineEditor]
	private ModuleData module;
	[SerializeField]
	private GameObject floorPrefab;

	private GameObject roomFloors;

	private GameObject roomIcon;

	private void OnDrawGizmos()
	{
		module.ModuleLayout.ForEach((x, y) =>
		{
			if (module.ModuleLayout[x, y] == true)
			{
				var coords = transform.position + new Vector3(y, x, -0.5f);

				Gizmos.color = Color.green;
				Gizmos.DrawWireCube(coords, Vector3.one);
			}
		});

		List<float> aX = new List<float>();
		List<float> aY = new List<float>();

		module.IconLayout.ForEach((x, y) =>
		{
			if (module.IconLayout[x, y] == true)
			{
				aX.Add(x);
				aY.Add(y);
			}
		});

		var coords = transform.position + new Vector3(aY.Average(), aX.Average(), -0.9f);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(coords, new Vector3(0.8f, 0.8f, 0.1f));
	}

	[Button]
	private void GenerateFloors()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			var child = transform.GetChild(i);
			if (child.name == "FloorGroup")
			{
#if UNITY_EDITOR
				DestroyImmediate(child.gameObject);
#else
				Destroy(child.gameObject);
#endif
				break;
			}
		}

		for (int i = 0; i < transform.childCount; i++)
		{
			var child = transform.GetChild(i);
			if (child.name == "IconGroup")
			{
#if UNITY_EDITOR
				DestroyImmediate(child.gameObject);
#else
				Destroy(child.gameObject);
#endif
				break;
			}
		}

		roomFloors = new GameObject("FloorGroup");
		roomFloors.transform.parent = gameObject.transform;

		module.ModuleLayout.ForEach((x, y) =>
		{
			if (module.ModuleLayout[x, y] == true)
			{
				var coords = transform.position + new Vector3(y, x, 0);
				var floor = Instantiate(floorPrefab, coords, Quaternion.identity);
				var matName = "";
				switch (module.ModuleCatagory)
				{
					case ModuleCatagory.Offensive: { matName = "ModuleOffensive"; break; }
					case ModuleCatagory.Defense: { matName = "ModuleDefence"; break; }
					case ModuleCatagory.Utility: { matName = "ModuleUtility"; break; }
				}
				floor.GetComponent<Renderer>().sharedMaterial = (Material)Resources.Load($"Materials/{matName}", typeof(Material));
				floor.transform.parent = roomFloors.transform;
			}
		});

		roomIcon = new GameObject("IconGroup");
		roomIcon.transform.parent = gameObject.transform;

		List<float> aX = new List<float>();
		List<float> aY = new List<float>();

		module.IconLayout.ForEach((x, y) =>
		{
			if (module.IconLayout[x, y] == true)
			{
				aX.Add(x);
				aY.Add(y);
			}
		});

		var coords = transform.position + new Vector3(aY.Average(), aX.Average(), -1);
		var icon = new GameObject("Icon");
		icon.transform.position = coords;
		icon.transform.localScale = Vector3.one / 3;
		var iconRenderer = icon.AddComponent<SpriteRenderer>();
		iconRenderer.sprite = module.Icon;
		icon.transform.parent = roomIcon.transform;
	}
}
