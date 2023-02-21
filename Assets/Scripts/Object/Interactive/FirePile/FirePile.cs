using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePile : MonoBehaviour
{
    private AudioSource fireSource;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<PlayerController>().Hurt();
        }
    }


}
