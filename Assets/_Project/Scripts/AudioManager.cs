using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class AudioManager : MonoBehaviour
    {
        [Header("----------AudioSource----------")]
        [SerializeField] AudioSource musicSource;
        [SerializeField] AudioSource SFXSource;

        [Header("----AudioClip------")]
        public AudioClip background;
        public AudioClip DO;
        public AudioClip RE;
        public AudioClip ME;
        public AudioClip FA;
        public AudioClip SO;
        public AudioClip LA;
        public AudioClip SI;
        public AudioClip DO_Octave;
        private void Start()
        {
            musicSource.clip = background;
            musicSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {

            SFXSource.PlayOneShot(clip);
        }
    }
}
