using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player" || other.gameObject.name == "SleepWomen")
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "Player" || other.gameObject.name == "SleepWomen")
        {
            other.transform.SetParent(null);
        }
    }
}
