using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ------------- SCRIPT CHECK 21.05.2019 ----------------- //
public class InputManager : MonoBehaviour
{
	private CarController _playerRef;

	[HideInInspector] public float _throttle;
	[HideInInspector] public float _steer;
	[HideInInspector] public float _brake;
	[HideInInspector] public bool _boost;

	[HideInInspector] public float _horizontal; 
	[HideInInspector] public float _vertical;
	[HideInInspector] public bool _boostComp;
	[HideInInspector] public bool _spotLeft;
	[HideInInspector] public bool _spotCentral;
	[HideInInspector] public bool _spotRight;
	[HideInInspector] public bool _grabTools;
	[HideInInspector] public float _leftTrigger;
	[HideInInspector] public float _rightTrigger;
	[HideInInspector] public bool _leftTriggerIsInUse = false;
	[HideInInspector] public bool _rightTriggerIsInUse = false;
	[HideInInspector] public bool _respawn = false;
	[HideInInspector] public bool _start = false;
	[HideInInspector] public bool _klaxon = false;

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
			_respawn = Input.GetButtonDown("Respawn");
			_start = Input.GetButtonDown("Start1");
			_klaxon = Input.GetButtonDown("Klaxon1");
		}
	
		else if (_playerRef._playerIndex == 2)
		{
			_throttle = Input.GetAxis("Accelerate2");
			_steer = Input.GetAxis("Horizontal2");
			_brake = Input.GetAxis("Brake2");
			_boost = Input.GetButton("Boost2");
			_horizontal = Input.GetAxis("LeftRight2");
			_vertical = Input.GetAxis("UpDown2");
			_boostComp = Input.GetButton("BoostComp2");
			_spotLeft = Input.GetButton("Spot12");
			_spotCentral = Input.GetButton("Spot22");
			_spotRight = Input.GetButton("Spot32");
			_grabTools = Input.GetButton("GrabTools2");
			_leftTrigger = Input.GetAxisRaw("LeftTrigger2");
			_rightTrigger = Input.GetAxisRaw("RightTrigger2");
			_respawn = Input.GetButtonDown("Respawn2");
			_start = Input.GetButtonDown("Start2");
			_klaxon = Input.GetButtonDown("Klaxon2");
		}
	}
}
