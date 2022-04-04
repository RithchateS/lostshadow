using System;
using UnityEngine;

namespace Data
{
    public class AudioClipData: MonoBehaviour
    {
        [SerializeField]private AudioClip[] audioClips;


        public AudioClip GetAudioClip(int index)
        {
            return audioClips[index];
        }

        public AudioClip[] GetAudioClipGroup(int startIndex, int endIndex)
        {
            AudioClip[] aud = new AudioClip[endIndex - startIndex + 1];
            int x = 0;
            for (int i = startIndex; i <= endIndex; i++)
            {
                aud[x] = audioClips[i];
                x++;
            }
            return aud;
        }
    }
}
