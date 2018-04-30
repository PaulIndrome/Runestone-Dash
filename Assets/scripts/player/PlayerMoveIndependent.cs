using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveIndependent : MonoBehaviour
{
    public float followSpeed, rotationSpeed;
    Player player;
    Vector3 nextPos;
    Quaternion lookRotation;
    float currentAngle, timeStep, currentRadius;
    [SerializeField] float timeToCircle, radiusTolerance;

    void Start()
    {
        player = GetComponent<Player>();
        timeStep = (2 * Mathf.PI) / ((timeToCircle == 0) ? 1 : timeToCircle);
        // set all serialized fields if 0
        timeToCircle = (timeToCircle == 0) ? 6f : timeToCircle;
        radiusTolerance = (radiusTolerance == 0) ? 0.1f : radiusTolerance;
        currentRadius = (currentRadius == 0) ? 5f : currentRadius;

        GameCenterPatrolCircle.pRadChange += SetNewRadius;
    }

    void Update()
    {
        bool onCircle = IsPlayerOnCircle();

        //the angle gets incremented step by step if the player is on the circle
        if(onCircle){
            IncrementAngle();
        } else {
            //if not, the angle is set to point towards the player from position 0,0,0
            currentAngle = Vector3.SignedAngle(Vector3.forward, transform.position, Vector3.up);
        }

        //if possible, move the player toward the currently set angle position on the circle
        if(!player.playerState.isDashing && player.playerState.canMove){
            MoveAndRotatePlayer();  
        }
    }

    public void SetNewRadius(float newRadius){
        currentRadius = newRadius;
        timeStep = (2 * Mathf.PI) / timeToCircle;
    }

    public void MoveAndRotatePlayer(){
        nextPos = currentRadius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * currentAngle), 0, Mathf.Cos(Mathf.Deg2Rad * currentAngle)) ;
        lookRotation = Quaternion.LookRotation((nextPos - transform.position), Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.position = Vector3.MoveTowards(transform.position, nextPos, Time.deltaTime * followSpeed);
    }

    //utility function to check if the player is situated on the patrol circle within
    //a certain margin of error (which prevents stuttering or endless approaches)
    public bool IsPlayerOnCircle(){
		return 	transform.position.magnitude >= currentRadius - radiusTolerance && 
				transform.position.magnitude <= currentRadius + radiusTolerance;
	}

    public float IncrementAngle(){
        currentAngle = (currentAngle + timeStep*(1-Time.deltaTime)) % 360;
        return currentAngle;
    }
}
