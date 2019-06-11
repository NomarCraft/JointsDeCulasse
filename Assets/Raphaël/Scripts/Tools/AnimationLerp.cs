using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//copi right RAFA a elle er Téo
// cc Raf, jtm
public class AnimationLerp : MonoBehaviour
{
    // t'as compris ça
    public void GoToEndPosition()
    {
        _isOnStartPosition = true;
        StartCoroutine(GoToPosition(_startingPosition, _endingPosition));
    }

    // ça aussi hein
    public void GoToStartPosition()
    {
        _isOnStartPosition = false;
        StartCoroutine(GoToPosition(_endingPosition, _startingPosition));
    }

    // inverse l'état actuel, càd 
    // si t'es à la position de fin, tu va vers le début
    // et inversement
    public void Reverse()
    {
        _isOnStartPosition = !_isOnStartPosition;

        if (_isOnStartPosition)
        {
            GoToEndPosition();
        }
        else
        {
            GoToStartPosition();
        }
    }

    #region PAS POUR TOI RAPHAEL
    #region J'AI DIS, IL FAUT PAS TOUCHER!
    public readonly static float MIN_DISTANCE = 0.1f; // nom tré nul

    #region Fields
    [SerializeField] private float _speed = 3f;
    [SerializeField] private Vector3 _localTarget;

    private Vector3 _startingPosition;
    private Vector3 _endingPosition;

    private bool _isOnStartPosition = false; // nom un peu moins nul, mais toujours..
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _startingPosition = transform.position;
        _endingPosition = transform.position + _localTarget;
    }
    #endregion

    IEnumerator GoToPosition(Vector3 startPosition, Vector3 wantedPosition)
    {
        while (Vector3.Distance(transform.position, wantedPosition) >= MIN_DISTANCE)
        {
            transform.position = Vector3.MoveTowards(transform.position, wantedPosition, _speed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        Debug.Log("<color=red>On est arrivé esti</color>");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_localTarget + transform.position, 30f);
    }
    #endregion
    #endregion
}
