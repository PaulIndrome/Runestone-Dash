using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{

    public float followSpeed, rotationSpeed;
    public Transform carrotStick;
    public PlayerState playerState;

    private Vector3 nextPos;
    private Quaternion lookRotation;

    void Start()
    {
        playerState.canDash = true;
    }

    void Update()
    {
        if (!playerState.isDashing)
        {
			nextPos = Vector3.MoveTowards(transform.position, carrotStick.position, Time.deltaTime * followSpeed);
			lookRotation = Quaternion.LookRotation((nextPos - transform.position), Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
			transform.position = nextPos;
        }
    }
}
