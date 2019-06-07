using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string _car1Event = "";
    FMOD.Studio.EventInstance _car1Sound;
    public CarController _carOne;

    void Start()
    {
        _car1Sound = FMODUnity.RuntimeManager.CreateInstance(_car1Event);
		_car1Sound.start();
    }

    void Update()
    {
		_car1Sound.getParameter("Velocity", out FMOD.Studio.ParameterInstance velocityS1);
		velocityS1.setValue(_carOne._currentSpeed);
    }
}
