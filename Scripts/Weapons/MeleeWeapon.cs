using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private AudioClip _swingSound;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private float _hitDistance = 2.75f;
    private Animator _animator;
    private int _attackTriggerId;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _attackTriggerId = Animator.StringToHash("Attack");
    }

    private void Update()
    {
        if ((Input.GetMouseButton(0) || Input.GetAxisRaw("RightTrigger") != 0) && !_isCoolingDown)
        {
            _isCoolingDown = true;
            AudioManager.PlaySound(_swingSound);
            _animator.SetTrigger(_attackTriggerId);
            Invoke(nameof(RestCooldown),cooldownTime);
        }
    }

    public void TryAttack() //Called by animation
    {
        OnShoot();
    }
    
    protected override void OnShoot()
    {
        RaycastHit hit = TryHit(_hitDistance);
        if (hit.collider != null)
        {
            AudioManager.PlaySound(_hitSound);
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
