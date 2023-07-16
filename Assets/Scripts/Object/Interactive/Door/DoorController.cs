using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Door _door;
    private DoorButton _doorButton;
    private List<GameObject> _SignLights;

    private Transform signLight;

    private void Awake()
    {
        _door = this.GetComponentInChildren<Door>();
        _doorButton = this.GetComponentInChildren<DoorButton>();
        signLight = this.transform.GetChild(2);

        for (int i = 0; i < signLight.transform.childCount; i++)
        {
            _SignLights.Add(signLight.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        _doorButton.GetDoor(_door);
        _doorButton.GetSignLights(_SignLights);
    }

}
