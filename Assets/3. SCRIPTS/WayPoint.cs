using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

	private void OnTriggerEnter(Collider other)
	{
		CarController car = other.gameObject.GetComponent<CarController>();
		
		if (car != null)
		{
			Transform wayPoint = GameManager.Instance._wayPoints[car._currentWayPoint];
			if (car._currentWayPoint == GameManager.Instance._wayPoints.Count - 1)
			{
				car._currentWayPoint = 0;
				car._currentLap += 1;
				car.gameObject.GetComponent<UIManager>().UpdateLapPosition(car._currentLap);
				Debug.Log("Lap");
			}
			else if (this.transform == wayPoint)
			{
				car._currentWayPoint += 1;
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, this.gameObject.GetComponent<BoxCollider>().size);
	}

}
