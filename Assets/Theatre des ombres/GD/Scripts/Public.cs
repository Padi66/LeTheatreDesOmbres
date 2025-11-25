using System.Collections;
using UnityEngine;

public class Public : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.1f;
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private float frameRate = 60f;
    [SerializeField] private MusicManager _musicManager;

    private Transform[] crowdMembers;
    private float[] offsets;
    private float[] speeds;
    private Vector3[] startPositions;
    private bool isAnimating = true;

    void Start()
    {
        _musicManager = GetComponent<MusicManager>();
        _musicManager.PlayPublicSound();
        crowdMembers = GetComponentsInChildren<Transform>();
        int count = crowdMembers.Length;
        offsets = new float[count];
        speeds = new float[count];
        startPositions = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            startPositions[i] = crowdMembers[i].localPosition;
            offsets[i] = Random.Range(1.9f, Mathf.PI * 1.8f);
            speeds[i] = baseSpeed * Random.Range(5f, 10f);
        }
    }
    
    public void PublicReaction(bool reaction)
    {
        if (reaction == true)
        {
           _musicManager.PlayBravoSound();
           StartCoroutine(AnimateCrowd());
        }
        else
        {
            {
                isAnimating = false;
                _musicManager.PlayBouhSound();
            }
        }
    }

    IEnumerator AnimateCrowd()
    {
        while (isAnimating)
        {
            float time = Time.time;
            for (int i = 0; i < crowdMembers.Length; i++)
            {
                float newY = startPositions[i].y + Mathf.Sin(time * speeds[i] + offsets[i]) * amplitude;
                crowdMembers[i].localPosition = new Vector3(startPositions[i].x, newY, startPositions[i].z);
            }

            yield return new WaitForSeconds(1f / frameRate); // limite la fréquence
        }
    }
}
