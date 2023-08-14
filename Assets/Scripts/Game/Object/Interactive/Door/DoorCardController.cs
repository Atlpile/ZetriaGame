using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCardController : MonoBehaviour
{
    private Door _door;
    private DoorCardMachine _doorCardMachine;
    private Transform _signLight;
    private readonly List<GameObject> _SignLights = new();

    private void Awake()
    {
        _door = this.GetComponentInChildren<Door>();
        _doorCardMachine = this.GetComponentInChildren<DoorCardMachine>();
        _signLight = this.transform.GetChild(0);

    }

    private void Start()
    {
        for (int i = 0; i < _signLight.transform.childCount; i++)
        {
            _SignLights.Add(_signLight.GetChild(i).gameObject);
        }

        _doorCardMachine.GetDoor(_door);
        _doorCardMachine.GetSignLights(_SignLights);
    }
}
