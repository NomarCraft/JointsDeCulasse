using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public float _throttle;
	public float _steer;
	public float _brake;
	public bool _boost;

	public float _horizontal;
	public float _vertical;

	private void Update()
	{
		_throttle = Input.GetAxis("Accelerate");
		_steer = Input.GetAxis("Horizontal");
		_brake = Input.GetAxis("Brake");
		_boost = Input.GetButton("Boost");
		_horizontal = Input.GetAxis("LeftRight");
		_vertical = Input.GetAxis("UpDown");
	}
}
