using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ------------- SCRIPT CHECK 21.05.2019 ----------------- //
public class CameraManager : MonoBehaviour
{
	public GameObject _focusTarget;
	private CarController _target;
	private Rigidbody _targetRB;

	public float _distance = 4f;
	public float _shakeMagnitude;
	public float _bonusDistance = 0;
	public float _height = 2f;
	public float _startingHeight;
	public float _dampening = 1f;

	private float _jumpingTime;
	float x;
	float y;

	private bool _isJumping = false;
	private bool _hasJustFinishJumping = false;

	private void Start()
	{
		_startingHeight = _height;
		_target = _focusTarget.GetComponent<CarController>();
		_targetRB = _focusTarget.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		_distance = 3.5f + ((_focusTarget.transform.InverseTransformVector(_focusTarget.GetComponent<Rigidbody>().velocity).z * 3.6f) / 70f);

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
			transform.position = _focusTarget.transform.position + _focusTarget.transform.TransformDirection(new Vector3(0f, _height, -_distance - _bonusDistance));
			transform.LookAt(_focusTarget.transform);
		}
		else if (!_isJumping)
		{
			if (_target._isBoosting == false)
			{
				if (x != 0)
				{
					x = 0;
					y = 0;
				}
				transform.position = _focusTarget.transform.position + _focusTarget.transform.TransformDirection(new Vector3(0f, _height, -_distance));
				transform.LookAt(_focusTarget.transform);
			}

			else if (_target._isBoosting)
			{
				x += Random.Range(-1f, 1f) * _shakeMagnitude;
				y += Random.Range(-1f, 1f) * _shakeMagnitude;

				transform.position = _focusTarget.transform.position + _focusTarget.transform.TransformDirection(new Vector3(0f , _height + y, -_distance + x));
				transform.LookAt(_focusTarget.transform);
			}
		}
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
