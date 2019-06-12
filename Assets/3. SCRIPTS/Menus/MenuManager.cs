using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string _ostMenus = "";
    FMOD.Studio.EventInstance _ostMenusInstance;

    [FMODUnity.EventRef]
    public string _clickSound = "";
    FMOD.Studio.EventInstance _clickSoundInstance;

    void Start()
    {
        _ostMenusInstance = FMODUnity.RuntimeManager.CreateInstance(_ostMenus);
        _clickSoundInstance = FMODUnity.RuntimeManager.CreateInstance(_clickSound);
        _ostMenusInstance.start();
    }

    void Update()
    {
        if (Input.GetButton("Boost") || Input.GetButton("Start1") || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
        {
            _clickSoundInstance.start();
        }
    }

    public void RACE_1_Player()
    {
        LoadScene("LVLARTTEST");
    }

    public void RACE_2_Players()
    {
        LoadScene("LVLARTTEST");
    }

    public void RACE_4_Players()
    {
        LoadScene("LVLARTTEST");
    }

    public void TUTO_1_Players()
    {
        LoadScene("LVLARTTEST");
    }

    public void TUTO_2_Players()
    {
        LoadScene("LVLARTTEST");
    }

    public void LoadScene(string SceneName)
    {
        _ostMenusInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _ostMenusInstance.release();
        _clickSoundInstance.release();
        SceneManager.LoadScene(SceneName); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
