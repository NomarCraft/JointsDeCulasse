using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
	public TextMeshProUGUI _text;

	public virtual void changeText (float speed)
	{
		float s = speed * 3.6f;
		_text.text = Mathf.Round(s) + "KM/H";
	}
}
