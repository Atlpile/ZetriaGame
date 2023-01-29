using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    public Transform targetPoint;

    private bool _canUse;
    private bool _isMax;
    private GameObject _buttonLight;
    private GameObject _highLight;
    private PlatformController _controller;


    private void Awake()
    {
        _buttonLight = this.transform.GetChild(0).gameObject;
        _highLight = this.transform.GetChild(1).gameObject;

        _controller = this.transform.GetComponentInParent<PlatformController>();
    }

    private void Start()
    {
        if (targetPoint == null)
            Debug.LogWarning(this.gameObject.name + "没有挂载目标点,请检查该物体目标点是否挂载");
    }

    private void Update()
    {
        if (targetPoint != null)
        {
            _isMax = Vector2.Distance(_controller.platform.transform.position, targetPoint.position) < 0.1f;
            UpdateButtonLight();

            if (Input.GetKey(KeyCode.E) && _canUse)
                _controller.PlatformMove(targetPoint);

        }
    }


    private void UpdateButtonLight()
    {
        if (_isMax && !_buttonLight.activeInHierarchy)
            _buttonLight.gameObject.SetActive(true);
        else if (!_isMax && _buttonLight.activeInHierarchy)
            _buttonLight.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _highLight.SetActive(true);
            _canUse = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _highLight.SetActive(false);
            _canUse = false;
        }
    }


}
