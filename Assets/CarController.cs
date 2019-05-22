using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // MARCO UI

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

	//Car Specs
	public float _strenghtCoefficient = 10000f;
	public float _brakeStrenght;
	public float _maxTurnAngle = 20f;
	public float _boostUseSpeed = 50f;
	public float _boostRefillRate = 10f;
	public float _boostAmount = 50f;

	//Components
	public Transform _cm;
	public Rigidbody _rb;

    //MARCO UI
    public Image _boostBar;

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
       // UpdateBoostBar(); //MARCO UI

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

   /* private void UpdateBoostBar() // MARCO UI
    {
        _boostBar.fillAmount = _boostAmount / 2;
    }*/

	private void Gravity()
	{

		bool grounded = CheckGround(_throttleWheels);

		_rb.AddForce(transform.up * -2000, ForceMode.Force);
	}

	private void Companion()
	{
		Lean();
	}

	private void Lean()
	{
		{
			if (_im._vertical > -0.2f && _im._vertical < 0.2f && _im._horizontal > -0.2f && _im._horizontal < 0.2f)
			{
				_companion.transform.SetPositionAndRotation(_companion.transform.position, Quaternion.Euler(0, 0, 0));
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

	private void Accelerate()
	{
		float currentSpeed = Mathf.Round(transform.InverseTransformVector(_rb.velocity).z * 3.6f);

		foreach (WheelCollider wheel in _throttleWheels)
		{

			if (_im._brake >= 0.1f)
			{
				if (transform.InverseTransformVector(_rb.velocity).z <= 0.5f )
				{
					wheel.motorTorque = _strenghtCoefficient * Time.deltaTime * -_im._brake;
					wheel.brakeTorque = 0;
				}
				else
				{
					wheel.motorTorque = 0;
					wheel.brakeTorque = (_im._brake * _brakeStrenght * Time.deltaTime) * 4;
				}
				
			}

			else
			{
				if (_im._boost == true && _boostAmount > 0f)
				{
					wheel.brakeTorque = 0;
					wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime) * 4f;
				}
				else
				{
					if (currentSpeed < 100)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 2f;
						wheel.brakeTorque = 0;
					}
					else if (currentSpeed > 100 && currentSpeed < 170)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 1.10f;
						wheel.brakeTorque = 0;
					}
					else if (currentSpeed > 170 && currentSpeed < 225)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 0.70f;
						wheel.brakeTorque = 0;
					}
					else if (currentSpeed > 225 && currentSpeed < 300)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 0.50f;
						wheel.brakeTorque = 0;
					}
					else if (currentSpeed > 300)
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
				wheel.steerAngle = _maxTurnAngle * _im._steer/* / 3*/;
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
				_companion.transform.SetPositionAndRotation(_companion.transform.position, Quaternion.Euler(0, 0, 0));
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
}
