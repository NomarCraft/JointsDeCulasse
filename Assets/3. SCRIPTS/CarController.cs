using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UIManager))]
public class CarController : MonoBehaviour
{

	//GameManagers
	[Header("Managers")]
	public InputManager _im;
	public UIManager _uim;
	[SerializeField] private GameObject _companion;

	//Components
	[Header("Components")]
	public Transform _cm;
	public Rigidbody _rb;
	public Camera _cam;
	[SerializeField] private List<Material> _materials;

	//Animations
	[Header("Animations & FXs")]
	public Animator _anim;
	public ParticleSystem[] _boostFX;
	public ParticleSystem[] _flameFX;
	public ParticleSystem _speed_Sand;
	public ParticleSystem[] _damageLights;
	public ParticleSystem[] _sparkRepair;
	

	//Race
	[Header ("Race Settings")]
	public int _playerIndex;
	[HideInInspector] public bool _raceHasStarted = false;
	[HideInInspector] public int _positionInRace;
	public int _currentLap = 0;
	public int _currentWayPoint = 1;
	public float _distanceFromWayPoints;

	//Wheels
	[Header("Physical Settings")]
	public List<WheelCollider> _throttleWheels;
	[SerializeField] private List<WheelCollider> _steeringWheels;
	private bool _isLeaning = false;
	private float _leanDir = 0;
	public bool _turning = false;
	public int _turningDir;
	public bool _isTheCompanionHelping = false;
	[SerializeField] private float _strenghtCoefficient = 10000f;
	[SerializeField] private float _brakeStrenght;
	[SerializeField] private float _maxTurnAngle = 20f;
	[SerializeField] private float _boostUseSpeed = 50f;
	[SerializeField] private float _boostRefillRate = 10f;
	[SerializeField] private float _boostStartAmount = 50f;
	[SerializeField] private float _boostAmount;
	[SerializeField] private float _boostMaxAmount = 200f;
	[HideInInspector] public bool _isBoosting = false;
	[HideInInspector] public float _currentSpeed;
	[SerializeField] private float _additionalGravity = 4000f;

	//Repair
	[Header("Repair Settings")]
	private int _carLife = 3;
	public int _repairToolCount = 6;
	public int _repairToolMaxCount = 6;
	[SerializeField] private Transform _grabSpot;
	private bool _grabingTools = false;
	private bool _isRepairing = false;
	private int _spotCurrentlyRepaired;
	[SerializeField] private Transform _spotLeft;
	 public int _spotLeftLife = 2;
	[SerializeField] private int _spotLeftMaxLife = 3;
	[SerializeField] private Transform _spotCenter;
	 public int _spotCenterLife = 3;
	[SerializeField] private int _spotCenterMaxLife = 3;
	[SerializeField] private Transform _spotRight;
	public int _spotRightLife = 3;
	[SerializeField] private int _spotRightMaxLife = 3;
	[SerializeField] private Transform _defaultSpot;
	private bool _miniGameFailing = false;
	private Coroutine _repair;
	private bool _repairFeedBack = false;
	private bool _hasJustBeenHit = false;

	//Minigame
	[Header("MiniGames")]
	[SerializeField] private bool _miniGame1IsOn = false;
	[SerializeField] private RectTransform _miniGame1;
	[SerializeField] private float _score1;
	[SerializeField] private float _objective1;
	[SerializeField] private float _timeLeft1;
	[SerializeField] private float _initialTime1 = 10f;

    // Sons
    [Header("Sounds")]

    [FMODUnity.EventRef]
    public string _carEngine = "";
    public FMOD.Studio.EventInstance _carEngineInstance;
    [FMODUnity.EventRef]
    public string _carKlaxon = "";
    public FMOD.Studio.EventInstance _carKlaxonInstance;
    [FMODUnity.EventRef]
    public string _carRespawn = "";
    public FMOD.Studio.EventInstance _carRespawnInstance;
    [FMODUnity.EventRef]
    public string _carDamage = "";
    public FMOD.Studio.EventInstance _carDamageInstance;
    [FMODUnity.EventRef]
    public string _carCollision = "";
    public FMOD.Studio.EventInstance _carCollisionInstance;
    [FMODUnity.EventRef]
    public string _carBoost = "";
    public FMOD.Studio.EventInstance _carBoostInstance;
    [FMODUnity.EventRef]
    public string _stopCarBoost = "";
    public FMOD.Studio.EventInstance _stopCarBoostInstance;
    [FMODUnity.EventRef]
    public string _repairFinished = "";
    public FMOD.Studio.EventInstance _repairFinishedInstance;
    [FMODUnity.EventRef]
    public string _steamLoop1 = "";
    public FMOD.Studio.EventInstance _steamLoop1Instance;
    [FMODUnity.EventRef]
    public string _steamLoop2 = "";
    public FMOD.Studio.EventInstance _steamLoop2Instance;
    [FMODUnity.EventRef]
    public string _steamLoop3 = "";
    public FMOD.Studio.EventInstance _steamLoop3Instance;
    [FMODUnity.EventRef]
    public string _reloadSound = "";
    public FMOD.Studio.EventInstance _reloadSoundInstance;
	[FMODUnity.EventRef]
	public string _hammerSound = "";
	public FMOD.Studio.EventInstance _hammerSoundInstance;

	public List<FMOD.Studio.EventInstance> _soundInstances = new List<FMOD.Studio.EventInstance>();


    private void InstanceSounds()
    {
        _carEngineInstance = FMODUnity.RuntimeManager.CreateInstance(_carEngine);
        _soundInstances.Add(_carEngineInstance);
        _carKlaxonInstance = FMODUnity.RuntimeManager.CreateInstance(_carKlaxon);
        _soundInstances.Add(_carKlaxonInstance);
        _carRespawnInstance = FMODUnity.RuntimeManager.CreateInstance(_carRespawn);
        _soundInstances.Add(_carRespawnInstance);
        _carDamageInstance = FMODUnity.RuntimeManager.CreateInstance(_carDamage);
        _soundInstances.Add(_carDamageInstance);
        _carCollisionInstance = FMODUnity.RuntimeManager.CreateInstance(_carCollision);
        _soundInstances.Add(_carCollisionInstance);
        _carBoostInstance = FMODUnity.RuntimeManager.CreateInstance(_carBoost);
        _soundInstances.Add(_carBoostInstance);
        _stopCarBoostInstance = FMODUnity.RuntimeManager.CreateInstance(_stopCarBoost);
        _soundInstances.Add(_stopCarBoostInstance);
        _repairFinishedInstance = FMODUnity.RuntimeManager.CreateInstance(_repairFinished);
        _soundInstances.Add(_repairFinishedInstance);
        _steamLoop1Instance = FMODUnity.RuntimeManager.CreateInstance(_steamLoop1);
        _soundInstances.Add(_steamLoop1Instance);
        _steamLoop2Instance = FMODUnity.RuntimeManager.CreateInstance(_steamLoop2);
        _soundInstances.Add(_steamLoop2Instance);
        _steamLoop3Instance = FMODUnity.RuntimeManager.CreateInstance(_steamLoop3);
        _soundInstances.Add(_steamLoop3Instance);
        _reloadSoundInstance = FMODUnity.RuntimeManager.CreateInstance(_reloadSound);
        _soundInstances.Add(_reloadSoundInstance);
		_hammerSoundInstance = FMODUnity.RuntimeManager.CreateInstance(_hammerSound);
		_soundInstances.Add(_hammerSoundInstance);
		Debug.Log("les sons du caca");
    }

    private void Start()
	{
        InstanceSounds(); //Instantiate Sounds
        _carEngineInstance.start();

        _im = GetComponent<InputManager>();
		_rb = GetComponent<Rigidbody>();
		_uim = GetComponent<UIManager>();

		_boostAmount = _boostStartAmount;

		//CenterOfMass(Depreciated)
		/*if (_cm)
		{
			_rb.centerOfMass = _cm.position;
		}*/

		foreach (WheelCollider wheel in _throttleWheels)
		{
			wheel.ConfigureVehicleSubsteps(300, 24, 24);
		}

		CheckDamage();

		foreach (ParticleSystem light in _damageLights)
		{
			ParticleSystem.MainModule settings = light.main;
			settings.startColor = Color.green;
			light.GetComponentInChildren<Light>().color = Color.green;
		}

		InvokeRepeating("DisplayDamage", 2.0f, 0.5f);
		
	}

	private void Update()
	{
        //Pitch du son moteur
        _carEngineInstance.getParameter("Velocity", out FMOD.Studio.ParameterInstance velocity);
        if (!CheckGround(_throttleWheels))
        {
            velocity.setValue(_currentSpeed + 30);
        }
        else
        {
            velocity.setValue(_currentSpeed);
        }


        CalculateDistanceToWayPoint();
		//UIUpdate
		_uim.changeSpeed(transform.InverseTransformVector(_rb.velocity).z);

		if (_im._start && Time.timeScale != 0)
		{
			GameManager.Instance.PauseGame(_playerIndex);
		}

		if (_im._klaxon && Time.timeScale != 0)
		{
            _carKlaxonInstance.start(); //Joue le son du klaxon

        }

		if (_miniGame1IsOn && Time.timeScale != 0)
		{
			MiniGame1();
		}

		_uim.UpdateToolsCount(_repairToolCount);
	}

	private void FixedUpdate()
	{
		_currentSpeed = Mathf.Round(transform.InverseTransformVector(_rb.velocity).z * 3.6f);

		if (_raceHasStarted == true)
		{
			FXCheck();
			Gravity();
			Companion();
			Accelerate();
			BoostAmount();
			Steer();

			if (_im._respawn)
			{
				foreach (WheelCollider wheel in _throttleWheels)
				{
					wheel.brakeTorque = 200000;
				}
				Respawn();
			}
		}
	}

   

	private void CalculateDistanceToWayPoint()
	{
		_distanceFromWayPoints = Vector3.Distance(transform.position, GameManager.Instance._wayPoints[_currentWayPoint].position);
	}

	private void FXCheck()
	{
		foreach (WheelCollider wheel in _throttleWheels)
		{
			WheelHit hit;
			if (wheel.GetGroundHit(out hit))
			{
				if (hit.collider.tag == "Sand" && _speed_Sand.isStopped)
				{
					_speed_Sand.Play();
				}

				else if (hit.collider.tag != "Sand" && _speed_Sand.isPlaying)
				{
					_speed_Sand.Stop();
				}
			}

		}
	}

	private void Respawn()
	{
		Transform respawn = GameManager.Instance._wayPoints[_currentWayPoint - 1].transform.GetComponentInChildren<RespawnPoints>().transform;
		_rb.velocity = Vector3.zero;
		_rb.angularVelocity = Vector3.zero;
		transform.SetPositionAndRotation(respawn.position , respawn.localRotation);
		_cam.transform.SetPositionAndRotation(respawn.position - new Vector3(0 , 1.5f, 3), respawn.localRotation);
        _carRespawnInstance.start(); // Play Sound Respawn

    }

	private void Gravity()
	{
		if (CheckGround(_throttleWheels))
		{
			_rb.AddForce(Vector3.down * _additionalGravity, ForceMode.Force);
		}
	}

	private void Companion()
	{
		if (!_grabingTools)
		{
			Repair();
			Lean();
		}
		GrabingTools();
	}

	private void TakeDamage(int amount)  //OK
	{
		if (_spotLeftLife > 0 || _spotCenterLife > 0 || _spotRightLife > 0)
		{
			int rand = Random.Range(0, 3);
			//Debug.Log(rand);
			switch (rand)
			{
				case 2:
					if (_spotLeftLife > 0)
					{
						_spotLeftLife -= amount;
						if (_spotLeftLife == 0)
						{
							DamageSound(0);
							ParticleSystem.MainModule settings = _damageLights[0].main;
							settings.startColor = Color.red;
							_damageLights[0].GetComponentInChildren<Light>().color = Color.red;
							_spotLeft.GetComponentInChildren<ParticleSystem>().Play();
							_carLife -= 1;
						}
					}
					else
					{
						TakeDamage(amount);
					}
					break;
				case 1:
					if (_spotCenterLife > 0)
					{
						_spotCenterLife -= amount;
						if (_spotCenterLife == 0)
						{
							DamageSound(1);
							ParticleSystem.MainModule settings = _damageLights[1].main;
							settings.startColor = Color.red;
							_damageLights[1].GetComponentInChildren<Light>().color = Color.red;
							_spotCenter.GetComponentInChildren<ParticleSystem>().Play();
							_carLife -= 1;
						}
					}
					else
					{
						TakeDamage(amount);
					}
					break;
				case 0:
					if (_spotRightLife > 0)
					{
						
						_spotRightLife -= amount;
						if (_spotRightLife == 0)
						{
							DamageSound(2);
							ParticleSystem.MainModule settings = _damageLights[2].main;
							settings.startColor = Color.red;
							_damageLights[2].GetComponentInChildren<Light>().color = Color.red;
							_spotRight.GetComponentInChildren<ParticleSystem>().Play();
							_carLife -= 1;
						}
					}
					else
					{
						TakeDamage(amount);
					}
					break;
				default:
					break;
			}
		}

		else
		{
			return;
		}

	}

	private void DamageSound(int ind)
	{
        _carDamageInstance.start(); //Play Damage Sound
        switch (ind)
        {
            case 0:
                _steamLoop1Instance.start();// Play Steam 1
                break;
            case 1:
                _steamLoop2Instance.start();// Play Steam 2
                break;
            case 2:
                _steamLoop3Instance.start();// Play Steam 3
                break;
            default:
                break;
        }
    }

	private void CheckDamage() // OK
	{
		CheckLife(_spotLeftLife, _spotLeft);
		CheckLife(_spotCenterLife, _spotCenter);
		CheckLife(_spotRightLife, _spotRight);
		UpdateLife();
	}

	private void DisplayDamage()
	{
		if (!_repairFeedBack && !_miniGame1IsOn)
		{
			if (_spotLeftLife <= 0)
			{
				_uim._buttonX.gameObject.SetActive(true);
				Vector3 pos;
				Canvas rect = _uim._buttonX.GetComponentInParent<Canvas>();
				Vector2 localPoint;
				pos = _cam.WorldToScreenPoint(_spotLeft.position);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.GetComponent<RectTransform>(), pos, rect.worldCamera, out localPoint);
				_uim._buttonX.rectTransform.localPosition = localPoint;
			}
			else
			{
				_uim._buttonX.gameObject.SetActive(false);
			}

			if (_spotCenterLife <= 0)
			{
				_uim._buttonA.gameObject.SetActive(true);
				Vector3 pos;
				Canvas rect = _uim._buttonA.GetComponentInParent<Canvas>();
				Vector2 localPoint;
				pos = _cam.WorldToScreenPoint(_spotCenter.position);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.GetComponent<RectTransform>(), pos, rect.worldCamera, out localPoint);
				_uim._buttonA.rectTransform.localPosition = localPoint;
			}
			else
			{
				_uim._buttonA.gameObject.SetActive(false);
			}

			if (_spotRightLife <= 0)
			{
				_uim._buttonB.gameObject.SetActive(true);
				Vector3 pos;
				Canvas rect = _uim._buttonB.GetComponentInParent<Canvas>();
				Vector2 localPoint;
				pos = _cam.WorldToScreenPoint(_spotRight.position);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.GetComponent<RectTransform>(), pos, rect.worldCamera, out localPoint);
				_uim._buttonB.rectTransform.localPosition = localPoint;
			}
			else
			{
				_uim._buttonB.gameObject.SetActive(false);
			}
		}

		else if (_repairFeedBack && !_miniGame1IsOn)
		{
			if (_spotLeftLife <= 0)
			{
				_uim._buttonX.gameObject.SetActive(false);
			}

			if (_spotCenterLife <= 0)
			{
				_uim._buttonA.gameObject.SetActive(false);
			}

			if (_spotRightLife <= 0)
			{
				_uim._buttonB.gameObject.SetActive(false);
			}
		}

		_repairFeedBack = !_repairFeedBack;
	}

	private void Repair() // OK
	{
		if (!_isRepairing && _repairToolCount > 0 /*&& _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_Center")*/)
		{
			
			if (_im._spotLeft && _spotLeftLife < _spotLeftMaxLife)
			{
				_companion.transform.position = _spotLeft.position;
				_anim.SetBool("Start_Repair_Left", true);
				_anim.SetBool("Stop_Repair_Left", false);
				_repair = StartCoroutine(Repairing(0));
				_isRepairing = true;
			}
			if (_im._spotCentral && _spotCenterLife < _spotCenterMaxLife)
			{
				_companion.transform.position = _spotCenter.position;
				_anim.SetBool("Start_Repair_Center", true);
				_anim.SetBool("Stop_Repair_Center", false);
				_repair = StartCoroutine(Repairing(1));
				_isRepairing = true;
			}
			if (_im._spotRight && _spotRightLife < _spotRightMaxLife)
			{
				_companion.transform.position = _spotRight.position;
				_anim.SetBool("Start_Repair_Right", true);
				_anim.SetBool("Stop_Repair_Right", false);
				_repair = StartCoroutine(Repairing(2));
				_isRepairing = true;
			}
		}
		else if (_isRepairing)
		{
			if (Input.GetButtonUp("Spot1") || Input.GetButtonUp("Spot2") || Input.GetButtonUp("Spot3"))
			{
				//Stop All Repair fonctions
				if (_miniGame1IsOn)
				{
					StopMiniGame1();
				}
				_isRepairing = false;
				CheckDamage();
				StopCoroutine(_repair);
			}
		}
	}

	private IEnumerator Repairing(int spot)  // OK
	{
		yield return new WaitForSeconds(1f);
		switch (spot)
		{
			case 0:
				StartMiniGame1(0);
				_uim._buttonX.gameObject.SetActive(false);
				break;
			case 1:
				StartMiniGame1(1);
				_uim._buttonA.gameObject.SetActive(false);
				break;
			case 2:
				StartMiniGame1(2);
				_uim._buttonB.gameObject.SetActive(false);
				break;
			default:
				break;
		}
		yield return new WaitForSeconds(1);
	} 

	private void StartMiniGame1(int ind)
	{
		_miniGame1.gameObject.SetActive(true);
		Vector3 pos;
		Canvas rect = _miniGame1.GetComponentInParent<Canvas>();
		Vector2 localPoint;
		switch (ind)
		{
			case 0:
				pos = _cam.WorldToScreenPoint(_spotLeft.position);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.GetComponent<RectTransform>(), pos, rect.worldCamera, out localPoint);
				_miniGame1.localPosition = localPoint;
				break;
			case 1:
				pos = _cam.WorldToScreenPoint(_spotCenter.position);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.GetComponent<RectTransform>(), pos, rect.worldCamera, out localPoint);
				_miniGame1.localPosition = localPoint;
				break;
			case 2:
				pos = _cam.WorldToScreenPoint(_spotRight.position);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.GetComponent<RectTransform>(), pos, rect.worldCamera, out localPoint);
				_miniGame1.localPosition = localPoint;
				break;
			default:
				break;
		}



		_timeLeft1 = 0;
		_score1 = 0;
		_spotCurrentlyRepaired = ind;
		_miniGame1IsOn = true;
	}

	private void MiniGame1()
	{
		if (_im._leftTrigger != 0)
		{
			if (_im._leftTriggerIsInUse == false)
			{
				_score1 += 1;
				_im._leftTriggerIsInUse = true;
				_uim._LT.gameObject.SetActive(false);
			}
			
		}
		if (_im._leftTrigger == 0 && _im._rightTriggerIsInUse)
		{
			_im._leftTriggerIsInUse = false;
			_uim._LT.gameObject.SetActive(true);
		}

		if (_im._rightTrigger != 0)
		{
			if (_im._rightTriggerIsInUse == false)
			{
				_score1 += 1;
				_im._rightTriggerIsInUse = true;
				_uim._RT.gameObject.SetActive(false);
			}

		}
		if (_im._rightTrigger == 0 && _im._leftTriggerIsInUse)
		{
			_im._rightTriggerIsInUse = false;
			_uim._RT.gameObject.SetActive(true);
		}

		if (_timeLeft1 == 40)
		{
			_score1 -= 1 ;
			_timeLeft1 = 0;
		}

		_timeLeft1++;
		_uim.UpdateMiniGame1(_score1, _objective1);

		if (_score1 >= _objective1)
		{
			_repairToolCount -= 1;
			_carLife += 1;
			CheckDamage();
            _repairFinishedInstance.start();// Play Repair Finished Sound


            switch (_spotCurrentlyRepaired)
			{
				case 0:
					if (_score1 >= _objective1)
					{
						_spotLeftLife = _spotLeftMaxLife;
						ParticleSystem.MainModule settings = _damageLights[0].main;
						settings.startColor = Color.green;
						_damageLights[0].GetComponentInChildren<Light>().color = Color.green;
						_spotLeft.GetComponentInChildren<ParticleSystem>().Stop();
						_steamLoop1Instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);// Stop Steam 1
                    }
                    StopMiniGame1();
					break;
				case 1:
					if (_score1 >= _objective1)
					{
						_spotCenterLife = _spotCenterMaxLife;
						ParticleSystem.MainModule settings = _damageLights[1].main;
						settings.startColor = Color.green;
						_damageLights[1].GetComponentInChildren<Light>().color = Color.green;
						_spotCenter.GetComponentInChildren<ParticleSystem>().Stop();
						_steamLoop2Instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);// Stop Steam 2
                    }
                    StopMiniGame1();
					break;
				case 2:
					if (_score1 >= _objective1)
					{
						ParticleSystem.MainModule settings = _damageLights[2].main;
						settings.startColor = Color.green;
						_damageLights[2].GetComponentInChildren<Light>().color = Color.green;
						_spotRightLife = _spotRightMaxLife;
						_spotRight.GetComponentInChildren<ParticleSystem>().Stop();
                        _steamLoop3Instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);// Stop Steam 3
                    }
                    StopMiniGame1();
					break;
				default:
					break;
			}
		}
	}

	private void StopMiniGame1()
	{
		_companion.transform.position = _defaultSpot.position;
		_companion.transform.localRotation = Quaternion.Euler(0, 0, 0);
		_anim.SetBool("Stop_Repair_Left", true);
		_anim.SetBool("Start_Repair_Left", false);
		_anim.SetBool("Start_Repair_Right", false);
		_anim.SetBool("Stop_Repair_Right", true);
		_anim.SetBool("Start_Repair_Center", false);
		_anim.SetBool("Stop_Repair_Center", true);
		_uim._RT.gameObject.SetActive(true);
		_uim._LT.gameObject.SetActive(true);
		_miniGame1.gameObject.SetActive(false);
		_miniGame1IsOn = false;
	}

	private void UpdateLife()
	{
		switch (_carLife)
		{
			case 0:
                _uim._fillAmountBoost.color = Color.red;
				_uim._power1.color = Color.red;
				_uim._power2.color = Color.red;
				_uim._power3.color = Color.red;
				break;
			case 1:
                _uim._fillAmountBoost.color = Color.red;
                _uim._power1.color = Color.green;
				_uim._power2.color = Color.red;
				_uim._power3.color = Color.red;
				break;
			case 2:
                _uim._fillAmountBoost.color = Color.red;
                _uim._power1.color = Color.green;
				_uim._power2.color = Color.green;
				_uim._power3.color = Color.red;
				break;
			case 3:
                _uim._fillAmountBoost.color = Color.green;
                _uim._power1.color = Color.green;
				_uim._power2.color = Color.green;
				_uim._power3.color = Color.green;
				break;
			default:
				break;
		}
	}

	private void Lean()  // OK
	{
		{
			if (!_isRepairing)
			{
				if (_im._vertical > -0.2f && _im._vertical < 0.2f && _im._horizontal > -0.2f && _im._horizontal < 0.2f)
				{
					_anim.SetBool("Start_Lean_Left", false);
					_anim.SetBool("Start_Lean_Right", false);
					_anim.SetBool("Stop_Lean_Left", true);
					_anim.SetBool("Stop_Lean_Right", true);
					_anim.SetBool("From_Left_To_Right", false);
					_anim.SetBool("From_Right_To_Left", false);
					_companion.transform.position = _defaultSpot.position;
					_companion.transform.localRotation = Quaternion.Euler(0, 0, 0);
					_isLeaning = false;
					_leanDir = 0;
				}
				//else if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_Center"))
				//{
					else if (_im._vertical < -0.2f)
					{
						if (CanLean(4))
						{
							_companion.transform.localRotation = Quaternion.Euler(-30, 0, 0);
						}
					}
					else if (_im._vertical > 0.2f)
					{
						if (CanLean(3))
						{
							_companion.transform.localRotation = Quaternion.Euler(30, 0, 0);
						}
					}
					else if (_im._horizontal > 0.2f)
					{
						if (CanLean(2))
						{
							if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_Left") || _anim.GetCurrentAnimatorStateInfo(0).IsName("Stop_Lean_Left"))
							{
								_anim.SetBool("Stop_Lean_Left", true);
								_anim.SetBool("From_Left_To_Right", true);
						}
							else
							{
								_anim.SetBool("Start_Lean_Right", true);
								_anim.SetBool("Stop_Lean_Right", false);
								_companion.transform.localRotation = Quaternion.Euler(0, 0, -30);
							}

						}
					}
					else if (_im._horizontal < -0.2f)
					{
						if (CanLean(1))
							{
							if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_Right") || _anim.GetCurrentAnimatorStateInfo(0).IsName("Stop_Lean_Right"))
							{
								_anim.SetBool("Stop_Lean_Right", true);
								_anim.SetBool("From_Right_To_Left", true);
							}
							else
							{
								_anim.SetBool("Start_Lean_Left", true);
								_anim.SetBool("Stop_Lean_Left", false);
								_companion.transform.localRotation = Quaternion.Euler(0, 0, 30);
							}
						}
					}
				//}
			}
		}
	}

	private void GrabingTools()  //OK
	{
		if (!_isRepairing && !_isLeaning && _im._grabTools)
		{

			_anim.SetBool("Start_Grab", true);
			_anim.SetBool("Stop_Grab", false);
			if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Grab"))
			{
				Debug.Log("grab");
				_grabingTools = true;
				_companion.transform.position = _grabSpot.position;
			}
		}
		else
		{
			_anim.SetBool("Start_Grab", false);
			_anim.SetBool("Stop_Grab", true);
			_grabingTools = false;
			_companion.transform.position = _defaultSpot.position;
		}
	}

	private void StartBoost()
	{
		if (_boostFX[0].isStopped)
		{
			foreach (ParticleSystem boost in _boostFX)
			{
				boost.gameObject.SetActive(true);
				boost.Play();
			}
			foreach (ParticleSystem flame in _flameFX)
			{
				flame.gameObject.SetActive(false);
				flame.Stop();
			}

			_carBoostInstance.start(); // Play Sound
        }
	}

	private void StopBoost()
	{
		if (_boostFX[0].isPlaying)
		{
			foreach (ParticleSystem boost in _boostFX)
			{
				boost.gameObject.SetActive(false);
				boost.Stop();
			}
			foreach (ParticleSystem flame in _flameFX)
			{
				flame.gameObject.SetActive(true);
				flame.Play();
			}
            _carBoostInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);//Stop Sound
            _stopCarBoostInstance.start();
        }
	}

	private void Accelerate() // OK
	{
		foreach (WheelCollider wheel in _throttleWheels)
		{

			if (_im._brake >= 0.2f)
			{
				_isBoosting = false;
				if (transform.InverseTransformVector(_rb.velocity).z <= 0.5f )
				{
					wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * -_im._brake) * 2;
					wheel.brakeTorque = 0;
				}
				else
				{
					wheel.motorTorque = 0;
					wheel.brakeTorque = (_im._brake * _brakeStrenght * Time.deltaTime) * 3;
				}
				
			}

			else
			{
				if (_im._boost == true && _im._boostComp == true && _boostAmount > 50f && _carLife >= 3)
				{
					_isBoosting = true;
					StartBoost();
					wheel.brakeTorque = 0;
					wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime) * 4f;
				}
				else
				{
					_isBoosting = false;
					StopBoost();

					if (_currentSpeed < 100)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 2.75f;
						wheel.brakeTorque = 0;
					}
					else if (_currentSpeed > 100 && _currentSpeed < 170 && _carLife > 0)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 2.25f;
						wheel.brakeTorque = 0;
					}
					else if (_currentSpeed > 170 && _currentSpeed < 225 && _carLife > 1)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 2.00f;
						wheel.brakeTorque = 0;
					}
					else if (_currentSpeed > 225 && _currentSpeed < 300 && _carLife > 1)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 1.75f;
						wheel.brakeTorque = 0;
					}
					else if (_currentSpeed > 300 && _carLife > 2)
					{
						wheel.motorTorque = (_strenghtCoefficient * Time.deltaTime * _im._throttle) * 1.00f;
						wheel.brakeTorque = 0;
					}
				}                                                     
			}
			ClockRotation(_currentSpeed);

            switch (_carLife)
            {
                case 0 : 
                    foreach (WheelCollider _wheel in _throttleWheels)
                    {
                        _wheel.wheelDampingRate = 20;
                        
                    }
                    break;
                case 1:
                    foreach (WheelCollider _wheel in _throttleWheels)
                    {
                        _wheel.wheelDampingRate = 13;

                    }
                    break;
                case 2:
                    foreach (WheelCollider _wheel in _throttleWheels)
                    {
                        _wheel.wheelDampingRate = 8;

                    }
                    break;
                case 3:
                    foreach (WheelCollider _wheel in _throttleWheels)
                    {
                        _wheel.wheelDampingRate = 5;

                    }
                    break;
                default:
                    break;
            }
		}
	}

	private void ClockRotation(float rotationValue)
	{
		_uim._clock.rectTransform.localRotation = Quaternion.Euler(0, 0,( -145 * rotationValue / 300) + 78);
	}

	private void BoostAmount() // OK
	{
		bool grounded = CheckGround(_throttleWheels);

		if (grounded && _im._boost && _im._boostComp)
		{
			_boostAmount = Mathf.Clamp(_boostAmount - (_boostUseSpeed * Time.deltaTime), 0f, _boostMaxAmount);
		}
		else
		{
			_boostAmount = Mathf.Clamp(_boostAmount + (_boostRefillRate * Time.deltaTime), 0f, _boostMaxAmount);
		}

		_uim._fillAmountBoost.fillAmount = _boostAmount / 200f;
		
	}

	private void Steer () // OK
	{
		float steerDamping = _currentSpeed / 120;

		foreach (WheelCollider wheel in _steeringWheels)
		{
			if (CheckGround(_throttleWheels))
			{
				if (_currentSpeed < -0.5f)
				{
					wheel.steerAngle = _maxTurnAngle * _im._steer * 4;
				}
				else if (_currentSpeed > 0 && _currentSpeed < 10)
				{
					wheel.steerAngle = 0;
				}
				else
				{
					if (/*_leanDir == 1 && */(_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_Left") || _anim.GetCurrentAnimatorStateInfo(0).IsName("Lean_Left")))
					{
						if (_im._steer < -0.1f)
						{
							wheel.steerAngle = (_maxTurnAngle * _im._steer) / steerDamping;
						}
					}
					else if (/*_leanDir == 2 && */(_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_Right") || _anim.GetCurrentAnimatorStateInfo(0).IsName("Lean_Right")))
					{
						if (_im._steer > 0.1f)
						{
							wheel.steerAngle = (_maxTurnAngle * _im._steer) / steerDamping;
						}
					}
					else
					{
						wheel.steerAngle = (_maxTurnAngle * _im._steer / 2.5f) / steerDamping;
					}
				}
			}
			else
			{
				_rb.angularVelocity = Vector3.zero;
				/*if (_im._brake > 0.25f)
				{
					transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - .25f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
				}
				else if (_im._throttle > 0.25f)
				{
					transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + .25f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
				}
				else if (_im._steer < -0.25f)
				{
					transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y -.25f, transform.rotation.eulerAngles.z);
				}
				else if (_im._steer > 0.25f)
				{
					transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + .25f, transform.rotation.eulerAngles.z);
				}

				if (transform.rotation.eulerAngles.z > 0f && transform.rotation.eulerAngles.z < 180f)
				{
					transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 1f);
				}

				else if (transform.rotation.eulerAngles.z > -180f && transform.rotation.eulerAngles.z > 0f)
				{
					transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 1f);
				}*/
			}
		}
	}

	private bool CanLean(int index)  // OK
	{
		if( _isLeaning == false)
		{
			_leanDir = index;
			_isLeaning = true;
			return true;
		}
		else if (_isLeaning == true)
		{
			if (index == _leanDir)
			{
				return false;
			}
			else
			{
				_companion.transform.localRotation = Quaternion.Euler(0, 0, 0);
				_isLeaning = false;
				return false;
			}
			
		}

		else
		{
			return false;
		}
		

	}

	public void StartTurning (int dir) // OK
	{
		if (dir == 5)
		{
			_turning = false;
			_turningDir = 0;
			//_uim.changeDir(_turningDir);
		}
		else
		{
			_turning = true;
			_turningDir = dir;
			//_uim.changeDir(_turningDir);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Obstacles" && !_hasJustBeenHit)
		{
			//_rb.AddForce(-transform.forward * 20000);
			TakeDamage(1);
			CheckDamage();
			Debug.Log("hit");
			_hasJustBeenHit = true;
            _carCollisionInstance.start();// Play Sound Col

            StartCoroutine(JustBeenHit());
		}
	}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tools")
        {
            if (_repairToolCount < _repairToolMaxCount && _grabingTools && !other.gameObject.GetComponent<Tools>()._hasBeenUsed)
            {
                _reloadSoundInstance.start();//Play sound reload
                _repairToolCount += other.gameObject.GetComponent<Tools>()._toolsAmount;
                StartCoroutine(other.gameObject.GetComponent<Tools>().RegenTime());
                Debug.Log(_repairToolCount);
				_reloadSoundInstance.start();
            }
        }
    }

    //Generic Functions

    public bool CheckGround(List<WheelCollider> targets)
	{
		WheelHit hit = new WheelHit();

		foreach (WheelCollider target in targets)
		{
			bool grounded = target.GetGroundHit(out hit);
			if (grounded)
			{
				return true;
			}
		}

		return false;
	}

	private void CheckLife (int life, Transform target)
	{
		switch (life)
		{
			case int n when n >= 2:
				//target.GetComponentInChildren<MeshRenderer>().material = _materials[0];
				break;
			case int n when n > 0 && n < 2:
				//target.GetComponentInChildren<MeshRenderer>().material = _materials[1];
				break;
			case 0:
				//target.GetComponentInChildren<MeshRenderer>().material = _materials[2];
				break;
			default:
				break;
		}
	}

	private IEnumerator JustBeenHit()
	{
		yield return new WaitForSeconds(1);
		_hasJustBeenHit = false;
	}

	public void PlayRepairFX (int pos)
	{
		switch (pos)
		{
			case 0:
				Debug.Log("0");
				_sparkRepair[0].Play();
				break;
			case 1:
				Debug.Log("1");
				_sparkRepair[1].Play();
				break;
			case 2:
				Debug.Log("2");
				_sparkRepair[2].Play();
				break;
			default:
				break;
		}

		_hammerSoundInstance.start();
	}

}

