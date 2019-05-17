using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public GameObject _focusTarget;
	public float _distance = 4f;
	public float _height = 2f;
	public float _dampening = 1f;

    // Update is called once per frame
    void FixedUpdate()
    {
		_distance = 3.5f + ((_focusTarget.transform.InverseTransformVector(_focusTarget.GetComponent<Rigidbody>().velocity).z * 3.6f) / 70f);


		transform.position = _focusTarget.transform.position + _focusTarget.transform.TransformDirection(new Vector3(0f, _height, -_distance));
		transform.LookAt(_focusTarget.transform);
    }
}
