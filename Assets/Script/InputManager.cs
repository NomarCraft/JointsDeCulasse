﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ------------- SCRIPT CHECK 21.05.2019 ----------------- //
public class InputManager : MonoBehaviour
{
	private CarController _playerRef;

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
	public float _leftTrigger;
	public float _rightTrigger;
	public bool _leftTriggerIsInUse = false;
	public bool _rightTriggerIsInUse = false;

	private void Start()
	{
		_playerRef = GetComponent<CarController>();
	}

	private void Update()
	{
		if (_playerRef._playerIndex == 1)
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
			_leftTrigger = Input.GetAxisRaw("LeftTrigger");
			_rightTrigger = Input.GetAxisRaw("RightTrigger");
		}
	
		else if (_playerRef._playerIndex == 2)
		{
			_throttle = Input.GetAxis("Accelerate2");
			_steer = Input.GetAxis("Horizontal2");
			_brake = Input.GetAxis("Brake2");
			_boost = Input.GetButton("Boost");
			_horizontal = Input.GetAxis("LeftRight2");
			_vertical = Input.GetAxis("UpDown2");
			_boostComp = Input.GetButton("BoostComp2");
			_spotLeft = Input.GetButton("Spot12");
			_spotCentral = Input.GetButton("Spot22");
			_spotRight = Input.GetButton("Spot32");
			_grabTools = Input.GetButton("GrabTools2");
			_leftTrigger = Input.GetAxisRaw("LeftTrigger2");
			_rightTrigger = Input.GetAxisRaw("RightTrigger2");
		}
	}
}