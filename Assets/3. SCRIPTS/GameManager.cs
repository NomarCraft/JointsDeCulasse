using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	public CarController _player1;
	public CarController _player2;
	public int _lapNumber = 5;

	[SerializeField] private TextMeshProUGUI _startCounter;
	//[SerializeField] private TextMeshProUGUI _pauseScreen;
	[SerializeField] private GameObject _pauseCanvas;

	public List<Transform> _wayPoints;

	private int _playerInd;
    public bool _cameraGo = false;
    public bool _isTuto = false;

    [FMODUnity.EventRef]
    public string _cdStart = "";
    FMOD.Studio.EventInstance _cdStartInstance;
    [FMODUnity.EventRef]
    public string _goStart = "";
    FMOD.Studio.EventInstance _goStartInstance;
    [FMODUnity.EventRef]
    public string _ost = "";
    FMOD.Studio.EventInstance _ostInstance;
    [FMODUnity.EventRef]
    public string _backGround = "";
    FMOD.Studio.EventInstance _backGroundInstance;

    private void Start()
	{
        _cdStartInstance = FMODUnity.RuntimeManager.CreateInstance(_cdStart);
        _goStartInstance = FMODUnity.RuntimeManager.CreateInstance(_goStart);
        _ostInstance = FMODUnity.RuntimeManager.CreateInstance(_ost);
        _backGroundInstance = FMODUnity.RuntimeManager.CreateInstance(_backGround);
        _backGroundInstance.start();

        StartCoroutine(StartDelay());

		if (_player2 == null)
		{
			_player1._positionInRace = 1;
		}
	}

	private void Update()
	{
		if (_player2 == null)
		{
			if (_player1._currentLap == _lapNumber)
			{
				StartCoroutine(EndGame());
			}
		}
		else if (_player1._currentLap == _lapNumber || _player2._currentLap == _lapNumber)
		{
			StartCoroutine(EndGame());
		}

		if (_player2 != null)
		{
			CheckPositions();
		}

		/*if (Time.timeScale == 0)
		{
			DePauseGame(_playerInd);
		}*/
	}

	private IEnumerator StartDelay()
	{
        UpdateStartCounter("");
        if (_isTuto)
        {
            _cameraGo = true;
        }
        else
        {
            yield return new WaitForSeconds(3);
            _cameraGo = true;
            yield return new WaitForSeconds(9);
        }
        _cdStartInstance.start();// Sound 3
		UpdateStartCounter("3");
        yield return new WaitForSeconds(1);
		UpdateStartCounter("2");
        _cdStartInstance.start();// Sound 2
        yield return new WaitForSeconds(1);
		UpdateStartCounter("1");
        _cdStartInstance.start();// Sound 1
        yield return new WaitForSeconds(1);
        _goStartInstance.start();//Sound GO
        UpdateStartCounter("GOOOOOO");
		_player1._raceHasStarted = true;
		if (_player2 != null)
		{
			_player2._raceHasStarted = true;
		}
        _ostInstance.start();// Start the OST
		yield return new WaitForSeconds(1);
		UpdateStartCounter("");
	}

	private void CheckPositions()
	{
		if (CheckLap())
		{
			if (CheckWayPoints())
			{
				CheckDistance();
			}
		}
	}

	private bool CheckLap()
	{
		if (_player1._currentLap > _player2._currentLap)
		{
			UpdatePosition(1);
			return false;
		}
		else if (_player2._currentLap > _player1._currentLap)
		{
			UpdatePosition(2);
			return false;
		}
		else
		{
			return true;
		}
	}

	private bool CheckWayPoints()
	{
		if (_player1._currentWayPoint > _player2._currentWayPoint)
		{
			UpdatePosition(1);
			return false;
		}

		else if (_player2._currentWayPoint > _player1._currentWayPoint)
		{
			UpdatePosition(2);
			return false;
		}

		else
		{
			return true;
		}
	}

	private void CheckDistance()
	{
		if (_player1._distanceFromWayPoints < _player2._distanceFromWayPoints)
		{
			UpdatePosition(1);
		}
		
		else if (_player2._distanceFromWayPoints < _player1._distanceFromWayPoints)
		{
			UpdatePosition(2);
		}
	}

	private void UpdatePosition (int player)
	{
		if (player == 1)
		{
			_player1._positionInRace = 1;
			_player2._positionInRace = 2;
			_player1._uim.UpdatePosition(_player1._positionInRace);
			_player2._uim.UpdatePosition(_player2._positionInRace);
		}
		else if (player == 2)
		{
			_player1._positionInRace = 2;
			_player2._positionInRace = 1;
			_player1._uim.UpdatePosition(_player1._positionInRace);
			_player2._uim.UpdatePosition(_player2._positionInRace);
		}
	}

	private void UpdateStartCounter(string text)
	{
		_startCounter.text = text;
	}

	public void PauseGame(int playerInd)
	{
		_playerInd = playerInd;
		//_pauseScreen.gameObject.SetActive(true);
		_pauseCanvas.SetActive(true);
        _ostInstance.setPaused(true);// Stop OST
        //_player1._carEngineInstance.setPaused(true);

        for (int i = 0, length = _player1._soundInstances.Count; i < length; i++)
        {
            _player1._soundInstances[i].setPaused(true);
        }

        if (_player2 != null)
        {
            for (int i = 0, length = _player2._soundInstances.Count; i < length; i++)
            {
                _player2._soundInstances[i].setPaused(true);
            }
        }


        if (playerInd == 1)
		{
			_player1._im._start = false;
		}
		else if (playerInd == 2)
		{
			_player2._im._start = false;
		}
		Time.timeScale = 0;
	}

	public void DePauseGame(int playerInd)
	{
		
		if (playerInd == 1)
		{
			Debug.Log(_player1._im._start);
				Time.timeScale = 1;
				//_pauseScreen.gameObject.SetActive(false);
                _pauseCanvas.gameObject.SetActive(false);

                _ostInstance.setPaused(false);//Restart OST
                //_player1._carEngineInstance.setPaused(false);

                for (int i = 0, length = _player1._soundInstances.Count; i < length; i++)
                {
                    _player1._soundInstances[i].setPaused(false);
                }

                if (_player2 != null)
                {
                    for (int i = 0, length = _player2._soundInstances.Count; i < length; i++)
                    {
                        _player2._soundInstances[i].setPaused(false);
                    }
                }
		}
		else if (playerInd == 2)
		{

				Time.timeScale = 1;
				//_pauseScreen.gameObject.SetActive(false);
                _pauseCanvas.gameObject.SetActive(false);

                _ostInstance.setPaused(false);//Restart OST
                _player1._carEngineInstance.setPaused(false);
                if (_player2 != null)
                {
                    _player2._carEngineInstance.setPaused(false);
                }
		}
        
    }

	private IEnumerator EndGame()
	{
		if (_player1._positionInRace == 1)
		{
			_player1._uim._WIN.gameObject.SetActive(true);
			if (_player2 != null)
			{
				_player2._uim._LOOSE.gameObject.SetActive(true);
			}
		}
		else
		{
			_player1._uim._LOOSE.gameObject.SetActive(true);
			if (_player2 != null)
			{
				_player2._uim._WIN.gameObject.SetActive(true);
			}
		}

		yield return new WaitForSeconds(20);
		//SceneManager.LoadScene("LevelBlockOut", LoadSceneMode.Single);
	}
    public void QuitMenu()
    {
        KillAllSounds();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private void KillAllSounds()
    {
        _cdStartInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _cdStartInstance.release();
        _goStartInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _goStartInstance.release();
        _ostInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _ostInstance.release();
        _backGroundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _backGroundInstance.release();

        for (int i = 0, length = _player1._soundInstances.Count; i < length; i++)
        {
            _player1._soundInstances[i].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _player1._soundInstances[i].release();
        }

        if(_player2 != null)
        {
            for (int i = 0, length = _player2._soundInstances.Count; i < length; i++)
            {
                _player2._soundInstances[i].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                _player2._soundInstances[i].release();
            }
        }

    }
}
