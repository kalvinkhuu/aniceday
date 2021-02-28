using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    GameObject FrontOfHouse;
    GameObject DoorFrame;
    private Interactable interactable;

    [SerializeField]
    private float HouseZ = -2.0f;

    // Start is called before the first frame update
    void Start()
    {
        FrontOfHouse = GameObject.Find("FrontOfHouse");
        DoorFrame = GameObject.Find("DoorFrame");
    }

    public void SetDoorOpened(bool bOpened)
    {
        FrontOfHouse.SetActive(!bOpened);
        DoorFrame.SetActive(!bOpened);
    }

    public float GetPlaceToStandInHouse()
    {
        return HouseZ;
    }
}

