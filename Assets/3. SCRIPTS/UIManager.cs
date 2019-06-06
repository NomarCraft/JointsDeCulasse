using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// ------------- SCRIPT CHECK 21.05.2019 ----------------- //
public class UIManager : MonoBehaviour
{
	public TextMeshProUGUI _position;
	public TextMeshProUGUI _speed;
	public TextMeshProUGUI _dir;

	//Minigame 1
	public TextMeshProUGUI _playerScore;
	public TextMeshProUGUI _scoreObjective;
	public TextMeshProUGUI _timeLeft;
	public TextMeshProUGUI _toolsLeft;
	public Image _power1;
	public Image _power2;
	public Image _power3;
	public Image _clock;
	public Image _buttonA;
	public Image _buttonB;
	public Image _buttonX;
	public Image _buttonY;
	public Image _fillAmountMiniGame;
	public Image _fillAmountBoost;
	public Image _LT;
	public Image _RT;
	
	public virtual void UpdateMiniGame1(float score,float objective)
	{
		_fillAmountMiniGame.fillAmount = score / objective;
		Debug.Log(_fillAmountMiniGame.fillAmount);
	}

	public virtual void UpdatePosition(int pos)
	{
		_position.text = pos + "";
	}

	public void UpdateToolsCount(int tools)
	{
		_toolsLeft.text = tools + "";
	}

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
