using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int ammoCount;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float cooldownTime = 1.0f;
    [SerializeField] protected LayerMask _collisionLayers;
    protected bool _isCoolingDown;

    protected abstract void OnShoot();

    protected RaycastHit TryHit(float distance)
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.5f);
        if (Physics.Raycast(ray, out hit, distance, _collisionLayers))
        {
            return hit;
        }

        return new RaycastHit();
    }
}
