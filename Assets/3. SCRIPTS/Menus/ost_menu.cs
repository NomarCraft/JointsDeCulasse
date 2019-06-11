using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ost_menu : MonoBehaviour
{
    //public Animation _buttonAnim;
    [FMODUnity.EventRef]
    public string _event1 = "";
    FMOD.Studio.EventInstance _event1Sound;

    [FMODUnity.EventRef]
    public string _skratch = "";
    FMOD.Studio.EventInstance _skratchSound;

    void Start()
    {
        _event1Sound = FMODUnity.RuntimeManager.CreateInstance(_event1);
        _skratchSound = FMODUnity.RuntimeManager.CreateInstance(_skratch);
        _event1Sound.start();
    }

    void Update()
    {
        if (Input.GetButton("Boost") || Input.GetButton("Start1") || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
        {
            _skratchSound.start();
        }
    }
}
