using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public float _throttle;
	public float _steer;
	public float _brake;
	public bool _boost;

	private void Update()
	{
		_throttle = Input.GetAxis("Accelerate");
		_steer = Input.GetAxis("Horizontal");
		_brake = Input.GetAxis("Brake");
		_boost = Input.GetButton("Boost");
	}
}
