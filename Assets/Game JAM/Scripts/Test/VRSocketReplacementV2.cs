using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class VRSocketReplacementV2 : MonoBehaviour
{
    [Header("XR References")]
    [SerializeField] private XRSocketInteractor socketInteractor;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private bool removeParentObject = true;

    [Header("Particle Effect")]
    [SerializeField] private ParticleSystem particleEffect;
    [SerializeField] private bool playEffectOnSpawn = true;
    [SerializeField] private bool playEffectOnDestroy = true;

    [Header("Timings")]
    [SerializeField] private float destroyDelay = DEFAULT_DESTROY_DELAY;

    private const float DEFAULT_DESTROY_DELAY = 0.05f;

    private void OnEnable()
    {
        if (socketInteractor == null)
        {
            Debug.LogError("XRSocketInteractor is not assigned!", this);
            return;
        }
        socketInteractor.selectEntered.AddListener(OnObjectInserted);
    }

    private void OnDisable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnObjectInserted);
        }
    }

    private void OnObjectInserted(SelectEnterEventArgs args)
    {
        if (playEffectOnDestroy)
            PlayParticleEffect();

        if (args.interactableObject != null)
        {
            GameObject insertedObject = args.interactableObject.transform.gameObject;
            insertedObject.SetActive(false);
            Destroy(insertedObject, destroyDelay);
        }

        SpawnPrefab();

        gameObject.SetActive(false);
        Destroy(gameObject, destroyDelay);

        if (removeParentObject && transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
            Destroy(transform.parent.gameObject, destroyDelay);
        }
    }

    private void SpawnPrefab()
    {
        if (prefabToSpawn == null)
            return;

        GameObject spawned = Instantiate(prefabToSpawn, transform.position, transform.rotation);

        if (playEffectOnSpawn)
            PlayParticleEffect();
    }

    private void PlayParticleEffect()
    {
        if (particleEffect == null)
            return;

        ParticleSystem effectInstance = Instantiate(
            particleEffect,
            transform.position,
            transform.rotation
        );

        effectInstance.Play();
        
        float lifetime = effectInstance.main.duration + effectInstance.main.startLifetime.constantMax;
        Destroy(effectInstance.gameObject, lifetime);
    }
}
