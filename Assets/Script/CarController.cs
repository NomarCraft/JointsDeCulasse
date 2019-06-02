﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UIManager))]
public class CarController : MonoBehaviour
{

	//GameManagers
	[Header("Managers")]
	[SerializeField] private InputManager _im;
	public UIManager _uim;
	[SerializeField] private GameObject _companion;

	//Components
	[Header("Components")]
	public Transform _cm;
	public Rigidbody _rb;
	[SerializeField] private List<Material> _materials;


	//Race
	[Header ("Race Settings")]
	public int _playerIndex;
	[HideInInspector] public bool _raceHasStarted = false;
	[HideInInspector] public int _currentLap = 1;
	public int _currentWayPoint = 1;
	public float _distanceFromWayPoints;

	//Wheels
	[Header("Physical Settings")]
	[SerializeField] private List<WheelCollider> _throttleWheels;
	[SerializeField] private List<WheelCollider> _steeringWheels;
	private bool _isLeaning = false;
	private float _leanDir = 0;
	public bool _turning = false;
	public int _turningDir;
	public bool _isTheCompanionHelping = false;
	[SerializeField] private float _strenghtCoefficient = 10000f;
	[SerializeField] private float _brakeStrenght;
	[SerializeField] private float _maxTurnAngle = 20f;
	[SerializeField] private float _boostUseSpeed = 50f;
	[SerializeField] private float _boostRefillRate = 10f;
	[SerializeField] private float _boostStartAmount = 50f;
	[SerializeField] private float _boostAmount;
	[SerializeField] private float _boostMaxAmount = 200f;
	[HideInInspector] public float _currentSpeed;
	[SerializeField] private float _additionalGravity = 4000f;

	//Repair
	[Header("Repair Settings")]
	private int _carLife = 3;
	public int _repairToolCount = 6;
	public int _repairToolMaxCount = 6;
	[SerializeField] private Transform _grabSpot;
	private bool _grabingTools = false;
	private bool _isRepairing = false;
	[SerializeField] private Transform _spotLeft;
	[HideInInspector] public int _spotLeftLife = 3;
	[SerializeField] private int _spotLeftMaxLife = 3;
	[SerializeField] private Transform _spotCenter;
	[HideInInspector] public int _spotCenterLife = 3;
	[SerializeField] private int _spotCenterMaxLife = 3;
	[SerializeField] private Transform _spotRight;
	[HideInInspector] public int _spotRightLife = 3;
	[SerializeField] private int _spotRightMaxLife = 3;
	[SerializeField] private Transform _defaultSpot;
	private bool _miniGameFailing = false;
	private Coroutine _repair;

	//Minigame
	[Header("MiniGames")]
	[SerializeField] private bool _miniGame1IsOn = false;
	[SerializeField] private GameObject _miniGame1;
	[SerializeField] private int _score1;
	[SerializeField] private int _objective1;
	[SerializeField] private float _timeLeft1;
	[SerializeField] private float _initialTime1 = 10f;

	private void Start()
	{
		_im = GetComponent<InputManager>();
		_rb = GetComponent<Rigidbody>();
		_uim = GetComponent<UIManager>();

		_boostAmount = _boostStartAmount;

		//CenterOfMass(Depreciated)
		/*if (_cm)
		{
			_rb.centerOfMass = _cm.position;
		}*/

		foreach (WheelCollider wheel in _throttleWheels)
		{
			wheel.ConfigureVehicleSubsteps(300, 24, 24);
		}

		CheckDamage();
		
	}

	private void Update()
	{
		CalculateDistanceToWayPoint();
		//UIUpdate
		_uim.changeSpeed(transform.InverseTransformVector(_rb.velocity).z);

		if (_miniGame1IsOn)
		{
			MiniGame1();
		}
	}

	private void FixedUpdate()
	{
		_currentSpeed = Mathf.Round(transform.InverseTransformVector(_rb.velocity).z * 3.6f);

		if (_raceHasStarted == true)
		{
			Gravity();
			Companion();
			Accelerate();
			BoostAmount();
			Steer();

			if (_im._respawn)
			{
				Respawn();
			}
		}
	}

	private void CalculateDistanceToWayPoint()
	{
		_distanceFromWayPoints = Vector3.Distance(transform.position, GameManager.Instance._wayPoints[_currentWayPoint].position);
	}

	private void Respawn()
	{
		Transform respawn = GameManager.Instance._wayPoints[_currentWayPoint - 1].transform.GetComponentInChildren<RespawnPoints>().transform;
		_rb.velocity = Vector3.zero;
		transform.SetPositionAndRotation(respawn.position , respawn.localRotation);
	}

	private void Gravity()
	{
		if (CheckGround(_throttleWheels))
		{
			_rb.AddForce(Vector3.down * _additionalGravity, ForceMode.Force);
		}
	}

	private void Companion()
	{
		if (!_grabingTools)
		{
			Repair();
			Lean();
		}
		GrabingTools();
	}

	private void TakeDamage(int amount)  //OK
	{
		if (_spotLeftLife > 0 && _spotCenterLife > 0 && _spotRightLife > 0)
		{
			int rand = Random.Range(0, 2);
			switch (rand)
			{
				case 2:
					if (_spotLeftLife > 0)
					{
						_spotLeftLife -= amount;
						if (_spotLeftLife == 0)
						{
							_carLife -= 1;
						}
					}
					else
					{
						TakeDamage(amount);
					}
					break;
				case 1:
					if (_spotCenterLife > 0)
					{
						_spotCenterLife -= amount;
						if (_spotCenterLife == 0)
						{
							_carLife -= 1;
						}
					}
					else
					{
						TakeDamage(amount);
					}
					break;
				case 0:
					if (_spotRightLife > 0)
					{
						_spotRightLife -= amount;
						if (_spotRightLife == 0)
						{
							_carLife -= 1;
						}
					}
					else
					{
						TakeDamage(amount);
					}
					break;
				default:
					break;
			}
		}

		else
		{
			return;
		}

	}

	private void CheckDamage() // OK
	{
		CheckLife(_spotLeftLife, _spotLeft);
		CheckLife(_spotCenterLife, _spotCenter);
		CheckLife(_spotRightLife, _spotRight);
	}

	private void Repair() // OK
	{
		if (!_isRepairing && _repairToolCount > 0)
		{
			
			if (_im._spotLeft && _spotLeftLife < _spotLeftMaxLife)
			{
				_companion.transform.position = _spotLeft.position;
				_repair = StartCoroutine(Repairing(0));
				_isRepairing = true;
			}
			if (_im._spotCentral && _spotCenterLife < _spotCenterMaxLife)
			{
				_companion.transform.position = _spotCenter.position;
				_repair = StartCoroutine(Repairing(1));
				_isRepairing = true;
			}
			if (_im._spotRight && _spotRightLife < _spotRightMaxLife)
			{
				_companion.transform.position = _spotRight.position;
				_repair = StartCoroutine(Repairing(2));
				_isRepairing = true;
			}
		}
		else if (_isRepairing)
		{
			if (Input.GetButtonUp("Spot1") || Input.GetButtonUp("Spot2") || Input.GetButtonUp("Spot3"))
			{
				//Stop All Repair fonctions
				if (_miniGame1IsOn)
				{
					StopMiniGame1();
				}
				_companion.transform.position = _defaultSpot.position;
				_companion.transform.localRotation = Quaternion.Euler(0, 0, 0);
				_isRepairing = false;
				CheckDamage();
				StopCoroutine(_repair);
			}
		}
	}

	private IEnumerator Repairing(int spot)  // OK
	{
		switch (spot)
		{
			case 0:
				StartMiniGame1();
				break;
			case 1:
				StartMiniGame1();
				break;
			case 2:
				StartMiniGame1();
				break;
			default:
				break;
		}
		yield return new WaitForSeconds(10);
		switch (spot)
		{
			case 0:
				if (_score1 >= _objective1)
				{
					_spotLeftLife = _spotLeftMaxLife;
				}
				StopMiniGame1();
				break;
			case 1:
				if (_score1 >= _objective1)
				{
					_spotCenterLife = _spotCenterMaxLife;
				}
				StopMiniGame1();
				break;
			case 2:
				if (_score1 >= _objective1)
				{
					_spotRightLife = _spotRightMaxLife;
				}
				StopMiniGame1();
				break;
			default:
				break;
		}

		_repairToolCount -= 1;
		_carLife += 1;
		CheckDamage();
		_companion.transform.position = _defaultSpot.position;
		_companion.transform.localRotation = Quaternion.Euler(0, 0, 0);
	} 

	private void StartMiniGame1()
	{
		_miniGame1.SetActive(true);
		_timeLeft1 = _initialTime1;
		_score1 = 0;
		_miniGame1IsOn = true;
	}

	private void MiniGame1()
	{
		if (_im._leftTrigger != 0)
		{
			if (_im._leftTriggerIsInUse == false)
			{
				_score1 += 1;
				_im._leftTriggerIsInUse = true;
			}
			
		}
		if (_im._leftTrigger == 0)
		{
			_im._leftTriggerIsInUse = false;
		}

		if (_im._rightTrigger != 0)
		{
			if (_im._rightTriggerIsInUse == false)
			{
				_score1 += 1;
				_im._rightTriggerIsInUse = true;
			}

		}
		if (_im._rightTrigger == 0)
		{
			_im._rightTriggerIsInUse = false;
		}

		_timeLeft1 -= Time.deltaTime;
		

		_uim.UpdateMiniGame1(_score1, _objective1, Mathf.RoundToInt(_timeLeft1));
	}

	private void StopMiniGame1()
	{
		_miniGame1.SetActive(false);
		_miniGame1IsOn = false;
	}

	private void Lean()  // OK
	{
		{
			if (!_isRepairing)
			{
				if (_im._vertical > -0.2f && _im._vertical < 0.2f && _im._horizontal > -0.2f && _im._horizontal < 0.2f)
				{
					_companion.transform.position = _defaultSpot.position;
					_companion.transform.localRotation = Quaternion.Euler(0, 0, 0);
					_isLeaning = false;
					_leanDir = 0;
				}
				else if (_im._vertical < -0.2f)
				{
					if (CanLean(4))
					{
						_companion.transform.localRotation = Quaternion.Euler(-30, 0, 0);
					}
				}
				else if (_im._vertical > 0.2f)
				{
					if (CanLean(3))
					{
						_companion.transform.localRotation = Quaternion.Euler(30, 0, 0);
					}
				}
				else if (_im._horizontal > 0.2f)
				{
					if (CanLean(2))
					{
						_companion.transform.localRotation = Quaternion.Euler(0, 0, -30);
					}
				}
				else if (_im._horizontal < -0.2f)
				{
					if (CanLean(1))
					{
						_companion.transform.localRotation = Quaternion.Euler(0, 0, 30);
					}
				}
			}
		}
	}

	private void GrabingTools()  //OK
	{
		if (!_isRepairing && !_isLeaning && _im._grabTools)
		{
			_grabingTools = true;
			_companion.transform.position = _grabSpot.position;
		}
		else
		{
			_grabingTools = false;
			_companion.transform.position = _defaultSpot.position;
		}
	}

	private void Accelerate() // OK
	{
		foreach (WheelCollider wheel in _throttleWheels)
		{

			if (_im._brake >= 0.2f)
			{
				if (transform.InverseTransformVector(_rb.velocity).z <= 0.5f )
				{
					wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * -_im._brake) * 2;
					wheel.brakeTorque = 0;
				}
				else
				{
					wheel.motorTorque = 0;
					wheel.brakeTorque = (_im._brake * _brakeStrenght * Time.deltaTime) * 3;
				}
				
			}

			else
			{
				if (_im._boost == true && _im._boostComp == true && _boostAmount > 0f && _carLife >= 3)
				{
					wheel.brakeTorque = 0;
					wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime) * 4f;
				}
				else
				{
					if (_currentSpeed < 100)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 2f;
						wheel.brakeTorque = 0;
					}
					else if (_currentSpeed > 100 && _currentSpeed < 170 && _carLife > 0)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 1.30f;
						wheel.brakeTorque = 0;
					}
					else if (_currentSpeed > 170 && _currentSpeed < 225 && _carLife > 1)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 0.90f;
						wheel.brakeTorque = 0;
					}
					else if (_currentSpeed > 225 && _currentSpeed < 300 && _carLife > 1)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 0.45f;
						wheel.brakeTorque = 0;
					}
					else if (_currentSpeed > 300 && _carLife > 2)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 0.25f;
						wheel.brakeTorque = 0;
					}
				}
					
			}
		}
	}

	private void BoostAmount() // OK
	{
		bool grounded = CheckGround(_throttleWheels);

		if (grounded && _im._boost)
		{
			_boostAmount = Mathf.Clamp(_boostAmount - (_boostUseSpeed * Time.deltaTime), 0f, _boostMaxAmount);
		}
		else
		{
			_boostAmount = Mathf.Clamp(_boostAmount + (_boostRefillRate * Time.deltaTime), 0f, _boostMaxAmount);
		}
		    
	}

	private void Steer () // OK
	{
		foreach (WheelCollider wheel in _steeringWheels)
		{
			if (CheckGround(_throttleWheels))
			{
				if (_currentSpeed < 0.5f)
				{
					wheel.steerAngle = _maxTurnAngle * _im._steer * 4;
				}
				else
				{
					if (_im._steer < -0.1f && _leanDir == 1)
					{
						wheel.steerAngle = _maxTurnAngle * _im._steer;
					}
					else if (_im._steer > 0.1f && _leanDir == 2)
					{
						wheel.steerAngle = _maxTurnAngle * _im._steer;
					}
					else
					{
						wheel.steerAngle = _maxTurnAngle * _im._steer / 2.5f;
					}
				}
			}
			else
			{
				if (_im._brake > 0.25f)
				{
					transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - .5f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
				}
				else if (_im._throttle > 0.25f)
				{
					transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + .5f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
				}
				else if (_im._steer < -0.25f)
				{
					transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y -.5f, transform.rotation.eulerAngles.z);
				}
				else if (_im._steer > 0.25f)
				{
					transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + .5f, transform.rotation.eulerAngles.z);
				}
			}
		}
	}

	private bool CanLean(int index)  // OK
	{
		if( _isLeaning == false)
		{
			_leanDir = index;
			_isLeaning = true;
			return true;
		}
		else if (_isLeaning == true)
		{
			if (index == _leanDir)
			{
				return false;
			}
			else
			{
				_companion.transform.localRotation = Quaternion.Euler(0, 0, 0);
				_isLeaning = false;
				return false;
			}
			
		}

		else
		{
			return false;
		}
		

	}

	public void StartTurning (int dir) // OK
	{
		if (dir == 5)
		{
			_turning = false;
			_turningDir = 0;
			_uim.changeDir(_turningDir);
		}
		else
		{
			_turning = true;
			_turningDir = dir;
			_uim.changeDir(_turningDir);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Obstacles")
		{
			_rb.AddForce(-transform.forward * 20000);
			TakeDamage(1);
			CheckDamage();
			Debug.Log("hit");
		}

		if (other.gameObject.tag == "Tools")
		{
			if (_repairToolCount < _repairToolMaxCount && _grabingTools && !other.GetComponent<Tools>()._hasBeenUsed)
			{
				_repairToolCount += other.GetComponent<Tools>()._toolsAmount;
				StartCoroutine(other.GetComponent<Tools>().RegenTime());
				Debug.Log(_repairToolCount);
			}
		}
	}

	//Generic Functions

	private bool CheckGround(List<WheelCollider> targets)
	{
		WheelHit hit = new WheelHit();

		foreach (WheelCollider target in targets)
		{
			bool grounded = target.GetGroundHit(out hit);
			if (grounded)
			{
				return true;
			}
		}

		return false;
	}

	private void CheckLife (int life, Transform target)
	{
		switch (life)
		{
			case int n when n > 2:
				target.GetComponentInChildren<MeshRenderer>().material = _materials[0];
				break;
			case int n when n > 0 && n <= 2:
				target.GetComponentInChildren<MeshRenderer>().material = _materials[1];
				break;
			case 0:
				target.GetComponentInChildren<MeshRenderer>().material = _materials[2];
				break;
			default:
				break;
		}
	}

}

