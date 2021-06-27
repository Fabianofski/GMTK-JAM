using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] GameObject DoorClosed;
    [SerializeField] GameObject DoorOpened;
    public void Toggle()
    {
        DoorClosed.SetActive(!DoorClosed.activeSelf);
        DoorOpened.SetActive(!DoorOpened.activeSelf);
    }
}
