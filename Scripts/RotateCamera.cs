using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour {
    public float rotationSpeed;
    public Quaternion startRotation;
    public Quaternion curRotation;
    public float vIn;
    // Start is called before the first frame update
    void Start() {
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float YRot = horizontalInput * rotationSpeed * Time.deltaTime;

        float XRot = 0;
        float verticalInput = Input.GetAxis("Vertical");
        if ((verticalInput < 0 && transform.localRotation.x > -Mathf.PI / 12) || (verticalInput > 0 && transform.localRotation.x < Mathf.PI / 8)) {
            XRot = verticalInput * rotationSpeed * Time.deltaTime;
        }
        transform.Rotate(XRot, YRot, 0);

        curRotation = transform.localRotation;
        vIn = verticalInput;


        if (Input.GetKeyDown(KeyCode.Space)) {
            transform.localRotation = startRotation;
        }
    }
}
