using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    public bool canUse;
    public bool isMax;
    public Transform targetPoint;

    private GameObject _buttonLight;
    private GameObject _highLight;
    private PlatformController controller2;

    private void Awake()
    {
        _buttonLight = this.transform.GetChild(0).gameObject;
        _highLight = this.transform.GetChild(1).gameObject;

        controller2 = this.transform.GetComponentInParent<PlatformController>();
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
            isMax = Vector2.Distance(controller2.platform.transform.position, targetPoint.position) < 0.1f;
            UpdateButtonLight();

            if (Input.GetKey(KeyCode.E) && canUse)
            {
                controller2.MovePlatformToward(targetPoint);
            }
        }
    }

    private void UpdateButtonLight()
    {
        if (isMax && !_buttonLight.activeInHierarchy)
            _buttonLight.gameObject.SetActive(true);
        else if (!isMax && _buttonLight.activeInHierarchy)
            _buttonLight.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _highLight.SetActive(true);
            canUse = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _highLight.SetActive(false);
            canUse = false;
        }
    }


}
