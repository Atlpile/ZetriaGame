using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCard : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    private Vector3 _direction;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            GameManager.Instance.EventManager.EventTrigger(E_EventType.PickUpDoorCard);
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "collect");
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        _direction = new Vector2(0, Mathf.Sin(_moveSpeed * Time.deltaTime));
        transform.Translate(_direction);
    }
}
