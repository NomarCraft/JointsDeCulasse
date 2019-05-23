using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
	[HideInInspector] public bool _hasBeenUsed = false;

	public int _toolsAmount = 1;

	public IEnumerator RegenTime()
	{
		_hasBeenUsed = true;
		yield return new WaitForSeconds(1f);
		_hasBeenUsed = false;
	}
}
