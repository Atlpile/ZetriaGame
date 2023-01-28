using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    public E_PlatformButtonType type;
    public bool _canUse;
    public bool _notUse;

    private GameObject _buttonLight;
    private GameObject _buttonHighLight;
    private PlatformController controller;


    private void Awake()
    {
        _buttonLight = this.transform.GetChild(0).gameObject;
        _buttonHighLight = this.transform.GetChild(1).gameObject;
        controller = GetComponentInParent<PlatformController>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && _canUse)
        {
            switch (type)
            {
                case E_PlatformButtonType.Up:
                    _notUse = controller.MovePlatform(Vector2.up);
                    break;
                case E_PlatformButtonType.Down:
                    _notUse = controller.MovePlatform(Vector2.down);
                    break;
                case E_PlatformButtonType.Left:
                    break;
                case E_PlatformButtonType.Right:
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _canUse = true;
            _buttonHighLight.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _canUse = false;
            _buttonHighLight.SetActive(false);
        }
    }

    public void EnableButton()
    {
        _buttonLight.SetActive(true);
    }

    public void DisableButton()
    {
        _buttonLight.SetActive(false);
    }
}
