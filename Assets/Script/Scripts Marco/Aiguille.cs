using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aiguille : MonoBehaviour
{
    public Image _clockHand;
    public Quaternion _larot;

    private void Start()
    {
        _larot = _clockHand.rectTransform.rotation;
        _larot.z = 100;
    }
}
