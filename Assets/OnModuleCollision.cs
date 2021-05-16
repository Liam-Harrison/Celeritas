using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnModuleCollision : MonoBehaviour
{       
        [SerializeField]
        private Material placingMaterial;
		private void OnCollisionEnter(Collision col) {
            if (col.gameObject.tag == "ModuleRoomCollider") {
                placingMaterial.color = new Color(1,0,0);
            }
		}

		private void OnCollisionExit(Collision col) {
            if (col.gameObject.tag == "ModuleRoomCollider") {
                placingMaterial.color = new Color(0,1,0);
            }
		}
}
