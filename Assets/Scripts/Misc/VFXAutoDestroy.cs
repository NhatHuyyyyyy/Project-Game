using UnityEngine;

public class VFXAutoDestroy : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
