using UnityEngine;

public class SealCollider : MonoBehaviour
{
    private SealMob seal;

    private void Awake()
    {
        seal = GetComponentInParent<SealMob>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Let parent handle everything automatically
        // NO METHOD CALL NEEDED
    }

    private void OnCollisionStay(Collision collision)
    {
        // Parent SealMob will receive collision callbacks
    }
}
