using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sirenix.OdinInspector {
    public class CameraRotate : MonoBehaviour
    {   
        [SerializeField, PropertyRange(0.0f, 10.0f)]
        public float speed;
        [SerializeField, PropertyRange(0.0f, 10.0f)]
        public float forwardSpeed;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        private float yRotation;
        private float zPosition;
        // Update is called once per frame
        void Update()
        {
            // if (Input.GetKeyDown("space")) {
            //     transform.position = new Vector3(transform.position.x,transform.position.y, zPosition);
            //     zPosition += forwardSpeed;
            // }
            // else if (Input.GetKeyUp("space")) {
            //     transform.position = new Vector3(0,0,0);
            //     zPosition = 0.0f;
            // }
            // } else {

            // }
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,yRotation,transform.localEulerAngles.z);
            yRotation += speed;
        }
    }
}