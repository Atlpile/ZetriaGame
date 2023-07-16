using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Door door;
    public DoorButton doorButton;
    public List<GameObject> signLights;

    private Transform signLight;

    private void Awake()
    {
        door = this.GetComponentInChildren<Door>();
        doorButton = this.GetComponentInChildren<DoorButton>();
        signLight = this.transform.GetChild(2);

        for (int i = 0; i < signLight.transform.childCount; i++)
        {
            signLights.Add(signLight.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        doorButton.GetDoor(door);
        doorButton.GetSignLights(signLights);
    }

}
