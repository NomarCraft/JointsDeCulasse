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

	private bool _boost = false;

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

		_throttleWheels[0].ConfigureVehicleSubsteps(500, 20, 20);
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

		Debug.Log(Vector3.Dot(transform.InverseTransformVector(_rb.velocity), transform.forward));
	}

	private void Gravity()
	{
		_rb.AddForce(transform.up * -1500, ForceMode.Force);
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

	private void Boost()
	{
		WheelHit hit = new WheelHit();

		bool grounded = _throttleWheels[0].GetGroundHit(out hit);
		if (grounded == false)
		{
			grounded = _throttleWheels[1].GetGroundHit(out hit);
		}

		if (grounded && _im._boost && !_boost)
		{
			StartCoroutine(BoostDelay(2, transform.InverseTransformVector(_rb.velocity).z));
			_rb.AddForce(transform.forward * 15f, ForceMode.VelocityChange);
			_boost = true;
		}
	}

	private IEnumerator BoostDelay(float delay, float velocity)
	{
		yield return new WaitForSeconds(delay);

		if (transform.InverseTransformVector(_rb.velocity).z > velocity)
		{
			_rb.AddForce(-transform.forward * 4f, ForceMode.VelocityChange);
		}

		_boost = false;
	}

	private void Steer ()
	{
		foreach (WheelCollider wheel in _steeringWheels)
		{
			if (transform.InverseTransformVector(_rb.velocity).z < 0.5f)
			{
				wheel.steerAngle = (_maxTurnAngle * _im._steer) * 8;
			}
			else
			{
				wheel.steerAngle = _maxTurnAngle * _im._steer;
			}
			
		}
	}
}
