using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zetria
{
    public class PlayerGizmos : MonoBehaviour
    {
        public Vector2 groundCheckPosition;
        public float groundCheckRadius;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheckPosition, groundCheckRadius);
        }
    }
}

