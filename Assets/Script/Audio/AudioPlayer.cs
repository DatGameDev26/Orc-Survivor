using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private List<AudioSource> allSFXSources = new List<AudioSource>();
    public void playSFX(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
        allSFXSources.Add(sfxSource);

        sfxSource.clip = clip;
        sfxSource.Play();

        StartCoroutine(DestroySFXSource(sfxSource));
    }

    private IEnumerator DestroySFXSource(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        allSFXSources.Remove(source);
        Destroy(source);
    }

    public void playRandomSFXinList(AudioClip[] audioClips)
    {
        if (audioClips == null) return;
        playSFX(audioClips[Random.Range(0, audioClips.Length)]);
    }
}
