using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource PublicSource;
    [SerializeField] private AudioSource PlayerSource;

    [SerializeField] private AudioClip PublicSound;

    [SerializeField] private AudioClip BravoSound;

    [SerializeField] private AudioClip BouhSound;
    [SerializeField] private AudioClip MusicSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPublicSound()
    {
        PublicSource.Stop();
        PublicSource.clip = PublicSound;
        PublicSource.Play();
        PublicSource.volume = 0.5f;
    }

    public void PlayBravoSound()
    {
        PublicSource.Stop();
        PublicSource.clip = BravoSound;
        PublicSource.Play();
        PublicSource.volume = 1f;
    }

    public void PlayBouhSound()
    {
        PublicSource.Stop();
        PublicSource.clip = BouhSound;
        PublicSource.Play();
        PublicSource.volume = 0.5f;
    }

    public void PlayMusicSound()
    {
        PlayerSource.Stop();
        PlayerSource.clip = MusicSound;
        PlayerSource.Play();
    }


}
