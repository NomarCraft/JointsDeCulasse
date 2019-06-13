using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public GameObject _loadingScreen;
	public Image _loadingBarFillAmount;

	private float _progressAmount = 0f;

    [FMODUnity.EventRef]
    public string _ostMenus = "";
    FMOD.Studio.EventInstance _ostMenusInstance;

    [FMODUnity.EventRef]
    public string _clickSound = "";
    FMOD.Studio.EventInstance _clickSoundInstance;

    void Start()
    {
        Time.timeScale = 1;
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
		if (_progressAmount != 0)
		{
			_loadingBarFillAmount.fillAmount = _progressAmount;
		}
    }

    public void RACE_1_Player()
    {
        StartCoroutine(LoadSceneAsync("LVLARTTEST"));
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

    public AsyncOperation LoadScene(string SceneName)
    {
        _ostMenusInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _ostMenusInstance.release();
        _clickSoundInstance.release();
        return SceneManager.LoadSceneAsync(SceneName); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
	
	IEnumerator LoadSceneAsync(string sceneName)
	{
		_loadingScreen.SetActive(true);
		yield return new WaitForSeconds(.5f);

		AsyncOperation operation = LoadScene(sceneName);

		while (!operation.isDone)
		{
			_progressAmount = operation.progress;

			yield return null;
		}
	}
}
