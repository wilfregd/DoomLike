using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    [SerializeField] private float _timer;

    private void Start()
    {
        Invoke(nameof(DestroyObject), _timer);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
