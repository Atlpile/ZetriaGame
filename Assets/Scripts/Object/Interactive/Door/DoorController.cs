using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Door _door;
    private DoorButton _doorButton;
    private readonly List<GameObject> _SignLights = new();
    private Transform _signLight;

    private void Awake()
    {
        _door = this.GetComponentInChildren<Door>();
        _doorButton = this.GetComponentInChildren<DoorButton>();
        _signLight = this.transform.GetChild(2);


    }

    private void Start()
    {
        for (int i = 0; i < _signLight.transform.childCount; i++)
        {
            Debug.Log("第" + i + "个物体");
            _SignLights.Add(_signLight.GetChild(i).gameObject);
        }


        _doorButton.GetDoor(_door);
        _doorButton.GetSignLights(_SignLights);
    }

}
