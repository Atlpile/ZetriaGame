using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorCard : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 0.005f;
    [SerializeField] private float _moveRate = 3f;
    private float _moveTimer;

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
        _moveTimer += Time.deltaTime * _moveRate;
        float y = Mathf.Sin(_moveTimer) * _moveSpeed;
        transform.Translate(new Vector2(0f, y));
    }
}
