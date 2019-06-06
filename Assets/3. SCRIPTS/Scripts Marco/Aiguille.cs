using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aiguille : MonoBehaviour
{
    public Image _clockHand; 
    public RectTransform _larot; // component rotation de l'image de l'aiguille

    private float _speed; // a rempalcer par une ref à la voiture pour recup la vraie speed

    private void Start()
    {
        _larot.rotation = Quaternion.Euler(_larot.rotation.x, _larot.rotation.y, 80);
    }

    private void Update()
    {

    }
}
