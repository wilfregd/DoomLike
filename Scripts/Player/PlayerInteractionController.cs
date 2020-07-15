using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private float _distance = 2.5f;
    private Transform _camera;

    private void Awake()
    {
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interaction"))
        {
            RaycastHit hit;
            Ray ray = new Ray(_camera.position, _camera.forward);
            if (Physics.Raycast(ray, out hit, _distance, _mask))
            {
                IInteractive interactive = hit.collider.GetComponent<IInteractive>();
                if (interactive != null)
                {
                    interactive.OnInteract();
                }
            }
        }
    }
}
