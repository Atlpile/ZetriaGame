using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDouble : MonoBehaviour
{
    private bool isPlayer;
    private PlatformEffector2D platformEffector2D;

    private void Awake()
    {
        platformEffector2D = this.GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && isPlayer)
        {
            platformEffector2D.rotationalOffset = 180;
        }
        else if (platformEffector2D.rotationalOffset != 0 && !isPlayer)
        {
            platformEffector2D.rotationalOffset = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            isPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            isPlayer = false;
        }
    }
}
