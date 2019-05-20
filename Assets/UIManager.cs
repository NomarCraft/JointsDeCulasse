using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
	public TextMeshProUGUI _speed;
	public TextMeshProUGUI _dir;

	public virtual void changeSpeed (float speed)
	{
		float s = speed * 3.6f;
		_speed.text = Mathf.Round(s) + "KM/H";
	}

	public virtual void changeDir (float ind)
	{
		switch (ind)
		{
			case 0:
				_dir.text = "";
				break;
			case 1:
				_dir.text = "Left";
				break;
			case 2:
				_dir.text = "Right";
				break;
			case 3:
				_dir.text = "Top";
				break;
			case 4:
				_dir.text = "Down";
				break;
			default:
			break;
		}
	}
}
