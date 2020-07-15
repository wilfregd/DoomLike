using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityEnemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem _bloodParticles;
    [SerializeField] private Vector3 _particleLocationOffset;
    public int health = 100;
    protected bool _dead;

    public virtual void OnHit(int damage)
    {
        Instantiate(_bloodParticles, transform.position + _particleLocationOffset, Quaternion.identity);
        
        if (_dead) return;
        
        health -= damage;

        if (health <= 0)
        {
            _dead = true;
            OnDeath();
        }
    }
    
    public virtual void OnDeath() {}
}
