using System.Collections;
using System.Collections.Generic;
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

	private void OnDrawGizmos()
	{
		module.ModuleLayout.ForEach((x, y) =>
		{
			if (module.ModuleLayout[x, y] == true)
			{


				var coords = transform.position + new Vector3(y, x, 0);

				Gizmos.color = Color.green;
				Gizmos.DrawWireCube(coords, Vector3.one);
			}
		});
	}

    [Button]
    private void GenerateFloorsWithColliders() {
        roomFloors = new GameObject("ColliderObject");
        roomFloors.transform.parent = gameObject.transform;

        module.ModuleLayout.ForEach((x, y) =>
		{
			if (module.ModuleLayout[x, y] == true)
			{
				var coords = transform.position + new Vector3(y, x, 0);
                var roomCollider = roomFloors.AddComponent<BoxCollider>();
                var floor = Instantiate(floorPrefab, coords, Quaternion.identity);
                floor.transform.parent = roomFloors.transform;
                roomCollider.center = coords;
                roomCollider.isTrigger = true;
                roomCollider.tag = "ModuleRoomCollider"; //Needs to be a const
			}
		});
    }
}
