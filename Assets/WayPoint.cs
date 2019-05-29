using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, this.gameObject.GetComponent<BoxCollider>().size);
	}


	private void OnTriggerEnter(Collider other)
	{
		CarController car = other.gameObject.GetComponent<CarController>();

		if (car != null)
		{
			if (car._currentWayPoint == GameManager.Instance._wayPoints.Count - 2)
			{
				car._currentWayPoint = 0;
				car._currentLap += 1;
			}
			else
			{
				car._currentWayPoint += 1;
			}
		}
	}
}
