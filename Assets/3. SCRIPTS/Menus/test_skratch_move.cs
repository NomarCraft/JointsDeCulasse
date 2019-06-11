using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_skratch_move : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string _skratch = "";
    FMOD.Studio.EventInstance _skratchSound;

    void PlaySkratchSound1()
    {
        _skratchSound = FMODUnity.RuntimeManager.CreateInstance(_skratch);
        _skratchSound.start();
    }
}
