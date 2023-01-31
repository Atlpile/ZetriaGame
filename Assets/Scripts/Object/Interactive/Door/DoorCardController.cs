using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCardController : MonoBehaviour
{
    private bool _canUse;
    private bool _isPlayer;
    private GameObject _highLight;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isPlayer)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _isPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _isPlayer = false;
        }
    }
}
