using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    public AudioSource audioSource;
    [SerializeField] private AudioClip birdAudio, diamondAudio;
    private void Awake()
    {
        instance = this;
    }

    public void PlayAudio(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void BirdAudio()
    {
        audioSource.clip = birdAudio;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void DiamondAudio()
    {
        audioSource.clip = diamondAudio;
        audioSource.volume = 0.15f;
        audioSource.Play();
    }
}
