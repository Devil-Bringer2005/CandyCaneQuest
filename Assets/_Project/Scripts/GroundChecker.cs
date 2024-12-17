using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] float groundDistance = 0.08f;
        [SerializeField] LayerMask groundLayers;

        public bool IsGrounded {  get; private set; }
        // Update is called once per frame
        void Update()
        {
            IsGrounded = Physics.SphereCast(transform.position, groundDistance , Vector3.down,out _ , groundDistance, groundLayers);
        }
    }
}
