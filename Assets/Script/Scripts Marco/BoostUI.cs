using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour
{
    public Image _boostBar;
    public float _boostAmount;
    private bool _isBoosting = false;

    private void Start()
    {
        _boostAmount = 200f;
    }

    private void Update()
    {
        Debug.Log(_boostAmount);
        if (_boostAmount < 200f && _isBoosting == false)
        {
            _boostAmount = _boostAmount + 10f * Time.deltaTime;
        }
        if(_boostAmount <=50)
        {
            _boostAmount = 50;
        }
        _boostBar.fillAmount = (_boostAmount / 200f);

        GoBoost();
        StopBoost();
    }

    private void GoBoost()
    {
        if (Input.GetKey(KeyCode.G))
        {
            _isBoosting = true;
            _boostAmount = _boostAmount - 1.1f;
        }
    }

    private void StopBoost()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            _isBoosting = false;
        }
    }

}
