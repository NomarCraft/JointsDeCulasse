using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UIManager))]
public class CarController : MonoBehaviour
{

	//GameManagers
	public InputManager _im;
	public UIManager _uim;
	public GameObject _companion;

	//Wheels
	public List<WheelCollider> _throttleWheels;
	public List<WheelCollider> _steeringWheels;
	
	//Lean & Turn
	private bool _isLeaning = false;
	private float _leanDir = 0;
	public bool _turning = false;
	public int _turningDir;
	public bool _isTheCompanionHelping = false;

	//Repair
	public int _repairToolCount = 6;
	private bool _isRepairing = false;
	[SerializeField]private Transform _spotLeft;
	public int _spotLeftLife = 3;
	[SerializeField]private Transform _spotCenter;
	public int _spotCenterLife = 3;
	[SerializeField]private Transform _spotRight;
	public int _spotRightLife = 3;
	[SerializeField] private Transform _defaultSpot;
	private Coroutine _repair;

	//Car Specs
	public float _strenghtCoefficient = 10000f;
	public float _brakeStrenght;
	public float _maxTurnAngle = 20f;
	public float _boostUseSpeed = 50f;
	public float _boostRefillRate = 10f;
	public float _boostAmount = 50f;
	public float _currentSpeed;

	//Components
	public Transform _cm;
	public Rigidbody _rb;
	[SerializeField] private List<Material> _materials;


	private void Start()
	{
		_im = GetComponent<InputManager>();
		_rb = GetComponent<Rigidbody>();
		_uim = GetComponent<UIManager>();

		//CenterOfMass(Depreciated)
		/*if (_cm)
		{
			_rb.centerOfMass = _cm.position;
		}*/

		foreach (WheelCollider wheel in _throttleWheels)
		{
			wheel.ConfigureVehicleSubsteps(300, 24, 24);
		}
		
	}

	private void Update()
	{
		//UIUpdate
		_uim.changeSpeed(transform.InverseTransformVector(_rb.velocity).z);
	}

	private void FixedUpdate()
	{
		_currentSpeed = Mathf.Round(transform.InverseTransformVector(_rb.velocity).z * 3.6f);

		Gravity();
		Companion();
		Accelerate();
		Boost();
		Steer();

		/*
		if (_turning)
		{
			if (_turningDir == _leanDir)
			{
				_isTheCompanionHelping = true;
			}
			else if (_turningDir != _leanDir)
			{
				_isTheCompanionHelping = false;
			}
		}*/
	}
	
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

	private void Gravity()
	{

		bool grounded = CheckGround(_throttleWheels);

		_rb.AddForce(transform.up * -2000, ForceMode.Force);
	}

	private void Companion()
	{
		Repair();
		Lean();
	}

	private void TakeDamage()
	{
		int rand = Random.Range(0, 2);
		switch (rand)
		{
			case 2:
			if (_spotLeftLife > 0 )
				{
					_spotLeftLife -= 1;
				}
			else
				{
					TakeDamage();
				}
				break;
			case 1:
				if (_spotCenterLife > 0)
				{
					_spotCenterLife -= 1;
				}
				else
				{
					TakeDamage();
				}
				break;
			case 0:
				if (_spotRightLife > 0)
				{
					_spotRightLife -= 1;
				}
				else
				{
					TakeDamage();
				}
				break;
			default:
				break;
		}
	}

	private void CheckDamage()
	{
		Debug.Log("boum");
		switch (_spotLeftLife)
		{
			case int n when n > 2 :
				_spotLeft.GetComponentInChildren<MeshRenderer>().material = _materials[0];
				break;
			case int n when n > 0 && n <= 2 :
				_spotLeft.GetComponentInChildren<MeshRenderer>().material = _materials[1];
				break;
			case 0:
				_spotLeft.GetComponentInChildren<MeshRenderer>().material = _materials[2];
				break;
			default:
				break;
		}
		switch (_spotCenterLife)
		{
			case int n when n > 2 :
				_spotCenter.GetComponentInChildren<MeshRenderer>().material = _materials[0];
				break;
			case int n when n > 0 && n <= 2:
				_spotCenter.GetComponentInChildren<MeshRenderer>().material = _materials[1];
				break;
			case 0:
				_spotCenter.GetComponentInChildren<MeshRenderer>().material = _materials[2];
				break;
			default:
				break;
		}
		switch (_spotRightLife)
		{
			case int n when n > 2:
				_spotRight.GetComponentInChildren<MeshRenderer>().material = _materials[0];
				break;
			case int n when n > 0 && n <= 2:
				_spotRight.GetComponentInChildren<MeshRenderer>().material = _materials[1];
				break;
			case 0:
				_spotRight.GetComponentInChildren<MeshRenderer>().material = _materials[2];
				break;
			default:
				break;
		}
	}

	private void Repair()
	{
		if (!_isRepairing && _repairToolCount > 0)
		{
			
			if (_im._spotLeft)
			{
				_companion.transform.position = _spotLeft.position;
				_repair = StartCoroutine(Repairing(0));
				_isRepairing = true;
			}
			if (_im._spotCentral)
			{
				_companion.transform.position = _spotCenter.position;
				_repair = StartCoroutine(Repairing(1));
				_isRepairing = true;
			}
			if (_im._spotRight)
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
				_companion.transform.position = _defaultSpot.position;
				_companion.transform.localRotation = Quaternion.Euler(0, 0, 0);
				_isRepairing = false;
				CheckDamage();
				StopCoroutine(_repair);
			}
		}
	}

	private IEnumerator Repairing(int spot)
	{
		yield return new WaitForSeconds(3);
		switch (spot)
		{
			case 0:
				_spotLeftLife = 4;
				break;
			case 1:
				_spotCenterLife = 4;
				break;
			case 2:
				_spotRightLife = 4;
				break;
			default:
				break;
		}

		CheckDamage();
		_companion.transform.position = _defaultSpot.position;
		_companion.transform.localRotation = Quaternion.Euler(0, 0, 0);
	}

	private void Lean()
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

	private void Accelerate()
	{
		foreach (WheelCollider wheel in _throttleWheels)
		{

			if (_im._brake >= 0.1f)
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
				if (_im._boost == true && _im._boostComp == true && _boostAmount > 0f)
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
					else if (_currentSpeed > 100 && _currentSpeed < 170)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 1.10f;
						wheel.brakeTorque = 0;
					}
					else if (_currentSpeed > 170 && _currentSpeed < 225)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 0.70f;
						wheel.brakeTorque = 0;
					}
					else if (_currentSpeed > 225 && _currentSpeed < 300)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 0.50f;
						wheel.brakeTorque = 0;
					}
					else if (_currentSpeed > 300)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 0.25f;
						wheel.brakeTorque = 0;
					}
				}
					
			}
		}
	}

	private void Boost()
	{
		bool grounded = CheckGround(_throttleWheels);

		if (grounded && _im._boost)
		{
			_boostAmount = Mathf.Clamp(_boostAmount - (_boostUseSpeed * Time.deltaTime), 0f, 200f);
		}
		else
		{
			_boostAmount = Mathf.Clamp(_boostAmount + (_boostRefillRate * Time.deltaTime), 0f, 200f);
		}
		    
	}

	private void Steer ()
	{
		foreach (WheelCollider wheel in _steeringWheels)
		{
			if (_currentSpeed < 0.5f)
			{
				wheel.steerAngle = _maxTurnAngle * _im._steer * 4;
			}
			else
			{
				if (_im._steer < 0.1f && _leanDir == 1)
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
	}

	private bool CanLean(int index)
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

	public void StartTurning (int dir)
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
			TakeDamage();
			CheckDamage();
			Debug.Log("hit");
		}
	}
}

