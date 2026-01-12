using UnityEngine;

public class AttackSFX : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip swoosh1;
    public AudioClip swoosh2;

    public AudioClip punch1;
    public AudioClip punch2;

    public AudioClip death1;
    public AudioClip death2;

    public void PlayRandomSwoosh()
    {
        AudioClip clip = Random.value < 0.5f ? swoosh1 : swoosh2;
        audioSource.PlayOneShot(clip);
    }

    public void PlayRandomHit()
    {
        AudioClip clip = Random.value < 0.5f ? punch1 : punch2;
        audioSource.PlayOneShot(clip);
    }

    public void PlayRandomDeath()
    {
        AudioClip clip = Random.value < 0.5f ? death1 : death2;
        audioSource.PlayOneShot(clip);
    }
}