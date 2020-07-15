using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : Weapon
{
    [SerializeField] private AudioClip _shootSound;
    private Animator _animator;
    private int _shootTriggerId;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _shootTriggerId = Animator.StringToHash("Shoot");
    }
    
    private void Update()
    {
        if ((Input.GetMouseButton(0) || Input.GetAxisRaw("RightTrigger") != 0) && !_isCoolingDown)
        {
            _isCoolingDown = true;
            AudioManager.PlaySound(_shootSound);
            _animator.SetTrigger(_shootTriggerId);
            Invoke(nameof(RestCooldown),cooldownTime);
        }
    }

    public void TryAttack() //Called by animation
    {
        OnShoot();
    }
    
    protected override void OnShoot()
    {
        RaycastHit hit = TryHit(30.0f);
        if (hit.collider != null)
        {
            EntityEnemy enemy = hit.collider.GetComponent<EntityEnemy>();
            if (enemy != null)
            {
                enemy.OnHit(100);
            }
        }
    }
    
    private void RestCooldown()
    {
        _isCoolingDown = false;
    }
}