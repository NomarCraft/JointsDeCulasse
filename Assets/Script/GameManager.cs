using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public CarController _player1;
	public CarController _player2;

	public List<Transform> _wayPoints;

	private void Update()
	{
		if (_player2 != null)
		{
			CheckPositions();
		}
	}

	private void CheckPositions()
	{
		if (CheckLap())
		{
			if (CheckWayPoints())
			{
				CheckDistance();
			}
		}
	}

	private bool CheckLap()
	{
		if (_player1._currentLap > _player2._currentLap)
		{
			UpdatePosition(1);
			return false;
		}
		else if (_player2._currentLap > _player1._currentLap)
		{
			UpdatePosition(2);
			return false;
		}
		else
		{
			return true;
		}
	}

	private bool CheckWayPoints()
	{
		if (_player1._currentWayPoint > _player2._currentWayPoint)
		{
			UpdatePosition(1);
			return false;
		}

		else if (_player2._currentWayPoint > _player1._currentWayPoint)
		{
			UpdatePosition(2);
			return false;
		}

		else
		{
			return true;
		}
	}

	private void CheckDistance()
	{
		if (_player1._distanceFromWayPoints < _player2._distanceFromWayPoints)
		{
			UpdatePosition(1);
		}
		
		else if (_player2._distanceFromWayPoints < _player1._distanceFromWayPoints)
		{
			UpdatePosition(2);
		}
	}

	private void UpdatePosition (int player)
	{
		if (player == 1)
		{
			_player1._uim.UpdatePosition(1);
			_player2._uim.UpdatePosition(2);
		}
		else if (player == 2)
		{
			_player1._uim.UpdatePosition(2);
			_player2._uim.UpdatePosition(1);
		}
	}
}
