using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlatformColorChanger : MonoBehaviour
    {
        [SerializeField] private Material originalColor;
        [SerializeField] private Material changeColor;
        [SerializeField] private AudioClip collisionAudioClip; // The audio to play when collided
        private AudioManager audioManager;

        private void Start()
        {
            // Find the AudioManager in the scene
            audioManager = FindObjectOfType<AudioManager>();

            if (audioManager == null)
            {
                Debug.LogError("AudioManager not found in the scene. Make sure it's added.");
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Change color
                gameObject.GetComponent<MeshRenderer>().material = changeColor;

                // Play the assigned audio clip
                if (audioManager != null && collisionAudioClip != null)
                {
                    audioManager.PlaySFX(collisionAudioClip);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Revert color
                gameObject.GetComponent<MeshRenderer>().material = originalColor;
            }
        }
    }
}
