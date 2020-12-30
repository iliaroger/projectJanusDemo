using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    private bool _menuActive = false;
    [SerializeField]
    private GameObject _menuBox;

    void Start()
    {
        _menuBox.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            // triggers the menu
            _menuActive = !_menuActive;
            if (_menuActive)
            {
                _menuBox.SetActive(true);
            }
            else _menuBox.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        
    }
}
