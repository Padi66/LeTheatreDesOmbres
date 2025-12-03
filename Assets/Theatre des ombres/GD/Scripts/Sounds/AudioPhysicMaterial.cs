using UnityEngine;

public class PlaySoundOnMaterial : MonoBehaviour
{
    public AudioSource audioSource;


    public PhysicsMaterial targetMaterial;

    void OnCollisionEnter(Collision collision)
    {
        Collider otherCollider = collision.collider;

        if (otherCollider.sharedMaterial == targetMaterial)
        {
            audioSource.Play();
        }
    }
}
