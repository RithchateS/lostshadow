using UnityEngine;

public class AudioClipData: MonoBehaviour
{
    [SerializeField]private AudioClip[] _audioClips;


    public AudioClip GetAudioClip(int index)
    {
        return _audioClips[index];
    }
}
