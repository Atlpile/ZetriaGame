using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private MovePlatform platform;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _currentDistance;

    public GameObject pointA;
    public GameObject pointB;


    private void Awake()
    {

    }


    public bool MovePlatform(Vector2 direction)
    {
        if (Vector2.Distance(this.transform.position, pointA.transform.position) > 0.1f && Vector2.Distance(this.transform.position, pointB.transform.position) > 0.1f)
        {
            platform.transform.Translate(direction * _moveSpeed * Time.deltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GetDistance()
    {

    }

}
