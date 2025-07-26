using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float zoomSensitivity, panSensitivity, minDistance, maxDistance, minAngle, maxAngle;


    float distance, angleY;

    // Start is called before the first frame update
    void Start()
    {
        distance = transform.localPosition.magnitude;
        angleY = Mathf.Atan2(transform.localPosition.y, Mathf.Abs( transform.localPosition.z));
    }

    // Update is called once per frame
    void LateUpdate()
    {

        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity * Time.deltaTime, minDistance, maxDistance);
        transform.localPosition *= distance / transform.localPosition.magnitude;

        if(Input.GetMouseButton(1))
        {
            float newAngleY = Mathf.Clamp(angleY + Input.GetAxis("Mouse Y") * panSensitivity * Time.deltaTime, minAngle, maxAngle);
            // Direction from pivot to object
            Vector3 dir = transform.localPosition;

            // Rotate the direction
            dir = Quaternion.AngleAxis(newAngleY - angleY, Vector3.right) * dir;

            // Update position
            transform.localPosition = dir;

            transform.Rotate(Vector3.right, newAngleY - angleY);

            angleY = newAngleY;
        }



    }

    private void OnDrawGizmos()
    {

    }
}
