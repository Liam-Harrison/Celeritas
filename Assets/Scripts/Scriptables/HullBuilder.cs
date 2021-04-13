using System.Collections;
using System.Collections.Generic;
using Celeritas.Scriptables;
using UnityEngine;
using Sirenix.OdinInspector;

public class HullBuilder : MonoBehaviour
{
    [OnValueChanged(nameof(onAnyDataChanged))]
    public GameObject HullWall;
    [OnValueChanged(nameof(onAnyDataChanged))]
	public HullData Hull;

    public void Generate() {
        GenerateWalls();
    }

    [Button("Re-Generate")]
    public void GenerateWalls() {
        var group = GameObject.Find("Hull");
        if (group != null) {
            DestroyImmediate(group.gameObject);
        }
        group = new GameObject("Hull");
        group.transform.parent = transform;
        if (Hull != null) {            
            for (int x = 0; x < Hull.HullLayout.GetLength(0); x++)
            {
                for (int y = 0; y < Hull.HullLayout.GetLength(1); y++)
                {
                    if (Hull.HullLayout[x,y] == true) {
                        int center = y - (Hull.HullLayout.GetLength(1) - 1) / 2;
                        var wall = Instantiate(HullWall, new Vector3(x,0,center), Quaternion.identity);
                        wall.transform.parent = group.gameObject.transform;
                    }
                }
            }
        }
    }

    public void onAnyDataChanged() {
        Generate();
    }
}
