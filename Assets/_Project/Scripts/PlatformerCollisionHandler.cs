using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlatformerCollisionHandler : MonoBehaviour
    {
        Transform platform; // The platform , if any , we are on top of 

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlatform"))
            {
                //if the contact normal is pointing up ,we have collided with top of platform
                ContactPoint contact = other.GetContact(0);
                if (contact.normal.y < 0.5f) return;

                platform = other.transform;
                transform.SetParent(platform);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlatform"))
            {
                transform.SetParent(null);
                platform = null;
            } 
        }






    }
}
