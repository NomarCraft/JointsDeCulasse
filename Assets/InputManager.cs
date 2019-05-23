using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ------------- SCRIPT CHECK 21.05.2019 ----------------- //
public class InputManager : MonoBehaviour
{
	public float _throttle;
	public float _steer;
	public float _brake;
	public bool _boost;

	public float _horizontal;
	public float _vertical;
	public bool _boostComp;
	public bool _spotLeft;
	public bool _spotCentral;
	public bool _spotRight;
	public bool _grabTools;

	private void Update()
	{
		_throttle = Input.GetAxis("Accelerate");
		_steer = Input.GetAxis("Horizontal");
		_brake = Input.GetAxis("Brake");
		_boost = Input.GetButton("Boost");
		_horizontal = Input.GetAxis("LeftRight");
		_vertical = Input.GetAxis("UpDown");
		_boostComp = Input.GetButton("BoostComp");
		_spotLeft = Input.GetButton("Spot1");
		_spotCentral = Input.GetButton("Spot2");
		_spotRight = Input.GetButton("Spot3");
		_grabTools = Input.GetButton("GrabTools");
	}
}
