using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private static PlayerWeaponController instance;

    [SerializeField] private Transform _weaponBase;
    private List<WeaponDrop> _weapons = new List<WeaponDrop>();
    private WeaponDrop _actualWeapon;
    private GameObject _actualFPSObject;
    private int _currentIndex;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (_weapons.Count != 0)
        {
            if ((Input.GetAxis("ScrollWheel") > 0 || Input.GetButtonDown("BumperRight")) && _currentIndex < _weapons.Count - 1)
            {
                _currentIndex++;
                _currentIndex = Mathf.Clamp(_currentIndex, 0, _weapons.Count - 1);
                UpdateWeapon(_weapons[_currentIndex]);
            }
            if ((Input.GetAxis("ScrollWheel") < 0 ||  Input.GetButtonDown("BumperLeft")) && _currentIndex > 0)
            {
                _currentIndex--;
                _currentIndex = Mathf.Clamp(_currentIndex, 0, _weapons.Count - 1);
                UpdateWeapon(_weapons[_currentIndex]);
            }
        }
    }

    public static void PickupWeapon(WeaponDrop drop)
    {
        instance.Pickup(drop);
    }

    protected void Pickup(WeaponDrop drop)
    {
        _weapons.Add(drop);
        _currentIndex = _weapons.Count - 1;
        _actualWeapon = _weapons[_currentIndex];
        UpdateWeapon(_actualWeapon);
    }

    private void UpdateWeapon(WeaponDrop weapon)
    {
        if (_actualFPSObject != null)
        {
            Destroy(_actualFPSObject);
        }
        
        _actualFPSObject = Instantiate(weapon.GetPrefab(), _weaponBase.transform);
    }
}
