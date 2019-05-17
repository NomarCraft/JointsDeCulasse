using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UIManager))]
public class CarController : MonoBehaviour
{
	public InputManager _im;
	public UIManager _uim;
	public List<WheelCollider> _throttleWheels;
	public List<WheelCollider> _steeringWheels;

	public float _strenghtCoefficient = 10000f;
	public float _brakeStrenght;
	public float _maxTurnAngle = 20f;
	public float _boostUseSpeed = 50f;
	public float _boostRefillRate = 10f;
	public float _boostAmount = 50f;


	public Transform _cm;
	public Rigidbody _rb;


	private void Start()
	{
		_im = GetComponent<InputManager>();
		_rb = GetComponent<Rigidbody>();
		_uim = GetComponent<UIManager>();

		if (_cm)
		{
			_rb.centerOfMass = _cm.position;
		}

		_throttleWheels[0].ConfigureVehicleSubsteps(300, 24, 24);
	}

	private void Update()
	{
		_uim.changeText(transform.InverseTransformVector(_rb.velocity).z);
	}

	private void FixedUpdate()
	{
		Gravity();
		Accelerate();
		Boost();
		Steer();

		Debug.Log(_boostAmount);
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

	private void Accelerate()
	{
		float currentSpeed = Mathf.Round(_rb.velocity.z * 3.6f);

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
				wheel.steerAngle = _maxTurnAngle * _im._steer;
		}
	}
}
