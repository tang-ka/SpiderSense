using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSH_PlayerHP : MonoBehaviour
{
    public static SSH_PlayerHP Instance;
    private void Awake()
    {
        Instance = this;
    }

    float hp = 100;
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp > 0)
            {
                healthState = HealthState.Alive;
            }
            else
            {
                healthState = HealthState.Dead;
            }
        }
    }

    public enum HealthState
    {
        Alive,
        Dead
    }
    public HealthState healthState = HealthState.Alive;

    SSH_PlayerMove playerMove;
    SSH_WebMove playerWebMove;
    SSH_CamPivotRotate playerCamRotate;

    // Start is called before the first frame update
    void Start()
    {
        HP = 100;
        playerMove = GetComponent<SSH_PlayerMove>();
        playerWebMove = GetComponent<SSH_WebMove>();
        playerCamRotate = GetComponentInChildren<SSH_CamPivotRotate>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (healthState)
        {
            case HealthState.Alive:
                Alive();
                break;

            case HealthState.Dead:
                Dead();
                break;
        }
    }

    private void Alive()
    {
        playerMove.enabled = true;
        playerWebMove.enabled = true;
        playerCamRotate.enabled = true;
    }

    private void Dead()
    {
        playerMove.enabled = false;
        playerWebMove.enabled = false;
        playerCamRotate.enabled = false;
     }
}
