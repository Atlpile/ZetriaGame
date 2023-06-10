using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private AudioSource platformMoveSource;

    private void Awake()
    {
        platformMoveSource = this.GetComponent<AudioSource>();
    }

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

    public void Move(Transform target, float moveSpeed)
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    public void UpdateAudio(bool isActive)
    {
        platformMoveSource.enabled = isActive;
    }
}
