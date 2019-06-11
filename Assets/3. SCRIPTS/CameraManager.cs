using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ------------- SCRIPT CHECK 21.05.2019 ----------------- //
public class CameraManager : MonoBehaviour
{
	private Vector3 _targetPos;
	private Quaternion _lookAtTarget;

	public GameObject _focusTarget;
	private CarController _target;
	private Rigidbody _targetRB;

	public float _distance = 4f;
	//public float _shakeMagnitude;
	public float _bonusDistance = 0;
	public float _height = 2f;
	public float _startingHeight;
	public float _dampening = 1f;

	private float _jumpingTime;
	private bool _isJumping = false;
	private bool _hasJustFinishJumping = false;

	[Header("Camera Shake")]
	[Range(0, 1)] public float _trauma;
	[SerializeField] private float _traumaMultiply = 5f;
	[SerializeField] private float _traumaMagnitude = 0.8f;
	private float _timeCounter;

	public float Trauma
	{
		get
		{
			return _trauma;
		}
		set
		{
			_trauma = Mathf.Clamp01(value);
		}
	}


	private void Start()
	{
		_startingHeight = _height;
		_target = _focusTarget.GetComponent<CarController>();
		_targetRB = _focusTarget.GetComponent<Rigidbody>();
	}

	private float GetFloat(float seed)
	{
		return (Mathf.PerlinNoise(seed, _timeCounter) - 0.5f) * 2;
	}

	private Vector3 GetVect3()
	{
		return new Vector3(GetFloat(1), GetFloat(10), 0);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		_distance = 2.5f + ((_focusTarget.transform.InverseTransformVector(_focusTarget.GetComponent<Rigidbody>().velocity).z * 3.6f) / 100f);
		_lookAtTarget = Quaternion.LookRotation(_focusTarget.transform.position - transform.position);

		if (!_isJumping)
		{
			if (!_target.CheckGround(_target._throttleWheels))
			{
				_isJumping = true;
			}

		}
		else if (_isJumping)
		{
			if (_target.CheckGround(_target._throttleWheels) && !_hasJustFinishJumping)
			{
				_hasJustFinishJumping = true;
				StartCoroutine(Reset(_jumpingTime));
			}
		}

		if (_isJumping )
		{
			_jumpingTime += Time.deltaTime;
			if (_hasJustFinishJumping)
			{
				// Play Landing Sound
				if (_bonusDistance + _distance > _distance)
				{
					_bonusDistance -= 1.5f * Time.deltaTime;
				}
				if (_height > _startingHeight)
				{
					_height -= 2f * Time.deltaTime;
				}	
			}
			else
			{
				_bonusDistance += 0.75f * Time.deltaTime;
				_height += 1f * Time.deltaTime;
			}
			_targetPos = _focusTarget.transform.position + _focusTarget.transform.TransformDirection(new Vector3(0f, _height, -_distance - _bonusDistance));
			
		}
		else if (!_isJumping)
		{
			if (_target._isBoosting == false)
			{
				_targetPos = _focusTarget.transform.position + _focusTarget.transform.TransformDirection(new Vector3(0f, _height, -_distance));
				
			}

			else if (_target._isBoosting)
			{
				_timeCounter += Time.deltaTime * Mathf.Pow(_trauma, 0.3f) * _traumaMultiply;
				Vector3 shakePos = GetVect3() * _traumaMagnitude;
				_targetPos = _focusTarget.transform.position + _focusTarget.transform.TransformDirection(new Vector3(0f , _height, -_distance) + shakePos) ;
				
			}
		}

		if (_focusTarget.transform.InverseTransformVector(_targetRB.velocity).z > 1)
		{
			transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * 15);
			//transform.rotation = Quaternion.Slerp(transform.rotation, _lookAtTarget, Time.deltaTime);
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * 1.5f);
			//transform.rotation = Quaternion.Slerp(transform.rotation, _lookAtTarget, Time.deltaTime * 200);
		}

		transform.LookAt(_focusTarget.transform.position);
		
	}

	private IEnumerator Reset(float time)
	{
		yield return new WaitForSeconds(time / 3f);
		_hasJustFinishJumping = false;
		_isJumping = false;
		_bonusDistance = 0;
		_height = _startingHeight;
	}
}
