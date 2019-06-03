using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RepairKits : MonoBehaviour
{
    public GameObject _carController; //Pour récupérer la ref du nombre de kits disponibles
    public TextMeshProUGUI _kits;

    void Start()
    {
        _kits.text = _carController.kits;
    }

    public virtual void Updatekits(int kits)
    {
        _kits.text = kits;
    }
}
