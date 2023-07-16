using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCardController : MonoBehaviour
{
    private Door _door;
    private DoorCardMachine _doorCardMachine;
    private List<GameObject> _SignLights;

    private Transform signLight;

    private void Awake()
    {
        _door = this.GetComponentInChildren<Door>();
        _doorCardMachine = this.GetComponentInChildren<DoorCardMachine>();
        signLight = this.transform.GetChild(0);

        for (int i = 0; i < signLight.transform.childCount; i++)
        {
            _SignLights.Add(signLight.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        _doorCardMachine.GetDoor(_door);
        _doorCardMachine.GetSignLights(_SignLights);
    }
}
