using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveIndependent : MonoBehaviour
{

    public float followSpeed, rotationSpeed;
    public PlayerState playerState;

    private Vector3 nextPos;
    private Quaternion lookRotation;
    private float currentAngle, timeStep, currentRadius;
    [SerializeField] private float timeToCircle, radiusTolerance;

    void Start()
    {
        playerState.canDash = true;
        playerState.isDashing = false;
        timeStep = (2 * Mathf.PI) / ((timeToCircle == 0) ? 1 : timeToCircle);
        // set all serialized fields if 0
        timeToCircle = (timeToCircle == 0) ? 6f : timeToCircle;
        radiusTolerance = (radiusTolerance == 0) ? 0.1f : radiusTolerance;
        currentRadius = (currentRadius == 0) ? 5f : currentRadius;

        GameCenterPatrolCircle.pRadChange += SetNewRadius;
    }

    void Update()
    {
        timeStep = (2 * Mathf.PI) / timeToCircle;

        bool onCircle = IsPlayerOnCircle();

        if(onCircle){
            IncrementAngle();
        } else {
            currentAngle = Vector3.SignedAngle(Vector3.forward, transform.position, Vector3.up);
        }

        if(!playerState.isDashing){
            MoveAndRotatePlayer(onCircle);  
        }
    }

    public void SetNewRadius(float newRadius){
        currentRadius = newRadius;
    }

    public void MoveAndRotatePlayer(bool onCircle){
        nextPos = currentRadius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * currentAngle), 0, Mathf.Cos(Mathf.Deg2Rad * currentAngle));
        lookRotation = Quaternion.LookRotation((nextPos - transform.position), Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.position = Vector3.MoveTowards(transform.position, nextPos, Time.deltaTime * followSpeed);
    }

    public bool IsPlayerOnCircle(){
		return 	transform.position.magnitude >= currentRadius - radiusTolerance && 
				transform.position.magnitude <= currentRadius + radiusTolerance;
	}

    public float IncrementAngle(){
        currentAngle = (currentAngle + timeStep) % 360;
        return currentAngle;
    }
}
