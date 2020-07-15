using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZombie : EntityEnemy
{
    [SerializeField] private Sprite _deathSprite;
    [SerializeField] private AudioClip[] _deathClips;
    [SerializeField] private float _deathColliderYHeight = 0.2f;
    [SerializeField] private float _deathColliderYOffset = 0.25f;
    private AudioSource _source;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _renderer = GetComponent<SpriteRenderer>();
        
        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 center = collider.center;

        collider.center = new Vector3(center.x, 0.5f, center.z);
    }

    public override void OnHit(int damage)
    {
        base.OnHit(damage);
    }

    public override void OnDeath()
    {
        _renderer.sprite = _deathSprite;
        _source.PlayOneShot(_deathClips[Random.Range(0, _deathClips.Length)]);
        
        //Change collider
        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 size = collider.size;
        Vector3 center = collider.center;

        collider.center = new Vector3(center.x, _deathColliderYOffset, center.z);
        collider.size = new Vector3(size.x, _deathColliderYHeight, size.z);
        collider.isTrigger = true;
    }
}
