using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollScript : MonoBehaviour
{
	public WheelCollider _wheelL;
	public WheelCollider _wheelR;

	private Rigidbody _carRigidbody;

	public float AntiRoll = 5000f;

	private void Start()
	{
		_carRigidbody = GetComponent<Rigidbody>();
	}

	private void LateUpdate()
	{
		WheelHit hit = new WheelHit();

		float travelL = 1.0f;
		float travelR = 1.0f;

		bool groundedL = _wheelL.GetGroundHit(out hit);

		if (groundedL)
		{
			travelL = (-_wheelL.transform.InverseTransformPoint(hit.point).y - _wheelL.radius) / _wheelL.suspensionDistance;
		}

		bool groundedR = _wheelR.GetGroundHit(out hit);

		if (groundedR)
		{
			travelR = (-_wheelR.transform.InverseTransformPoint(hit.point).y - _wheelR.radius) / _wheelR.suspensionDistance;
		}

		var antiRollForce = (travelL - travelR) * AntiRoll;

		if (groundedL)
		{
			_carRigidbody.AddForceAtPosition(_wheelL.transform.up * -antiRollForce, _wheelL.transform.position);
		}

		if (groundedR)
		{
			_carRigidbody.AddForceAtPosition(_wheelR.transform.up * antiRollForce, _wheelR.transform.position);
		}
		
	}
}
