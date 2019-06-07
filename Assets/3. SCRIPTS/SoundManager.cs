using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string _car1Event = "";
    FMOD.Studio.EventInstance _car1Sound;


    // Start is called before the first frame update
    void Start()
    {
        _car1Sound = FMODUnity.RuntimeManager.CreateInstance(_car1Event);
        _car1Sound.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
