﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LNStartingRoomController : RoomController {

    public GameObject[] LampLights;
    public GameObject[] LightsToDisable;
    public GameObject[] MovingFloors;
    public GameObject TrapTrigger;
    public GameObject[] WardrobeDoors;
    private const int GEMS_COUNT = 3;
    private int activatedGems;
    private bool trapActivated;
    private PhotonView view;

    // Use this for initialization
    void Start ()
    {
        activatedGems = 0;
        trapActivated = false;
        view = GetComponent<PhotonView>();
    }

    public void ActivateGem()
    {
        view.RPC("activateGem", PhotonTargets.All, true);
    }

    public void DeactivateGem()
    {
        view.RPC("activateGem", PhotonTargets.All, false);
    }

    public void DeactivateTrap()
    {
        if (!trapActivated) return;
        foreach (GameObject floor in MovingFloors)
        {
            MovingFloor mf = floor.GetComponent<MovingFloor>();
            mf.MoveDown();
        }
        TrapTrigger.SetActive(false);
    }

    public void EnableLampLights(bool enable)
    {
        view.RPC("setObjectsActive", PhotonTargets.All, enable, LampLights);
    }

    public void EnableOutsideLights(bool enable)
    {
        view.RPC("setObjectsActive", PhotonTargets.All, enable, LightsToDisable);
    }

    private void activateTrap()
    {
        foreach (GameObject floor in MovingFloors)
        {
            MovingFloor mf = floor.GetComponent<MovingFloor>();
            mf.MoveUp();
        }
        TrapTrigger.SetActive(true);
        trapActivated = true;
    }

    private void openWardrobeDoors()
    {
        foreach (GameObject door in WardrobeDoors)
        {
            UsableDoorController udc = door.GetComponent<UsableDoorController>();
            udc.Unlock();
            udc.Use();
        }
    }

    [PunRPC]
    private void setObjectsActive(bool active, GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(active);
        }
    }

    [PunRPC]
    private void activateGem(bool activate)
    {
        if (trapActivated) return;
        if (activate)
        {
            ++activatedGems;
            if (activatedGems >= GEMS_COUNT)
            {
                activateTrap();
                openWardrobeDoors();
            }
        }
        else
        {
            if (trapActivated) return;
            --activatedGems;
        }
    }
}
