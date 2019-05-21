using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------- SCRIPT CHECK 21.05.2019 ----------------- //
public class Turning : MonoBehaviour
{
	public enum Direction { Left, Right, Top, Down, Exit }


	[SerializeField] private Direction _dir;
	private int _dirIndex;

	private void OnDrawGizmos()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, this.gameObject.GetComponent<BoxCollider>().size);
	
	}

	private void Start()
	{
		switch (_dir)
		{
			case Direction.Left:
				_dirIndex = 1;
				break;
			case Direction.Right:
				_dirIndex = 2;
				break;
			case Direction.Top:
				_dirIndex = 4;
				break;
			case Direction.Down:
				_dirIndex = 3;
				break;
			case Direction.Exit:
				_dirIndex = 5;
				break;
			default:
				break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<CarController>())
		{
			other.gameObject.GetComponent<CarController>().StartTurning(_dirIndex);
		}
	}
}
