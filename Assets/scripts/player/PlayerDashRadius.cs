using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script controls the LineRenderer drawing the player dash radius circle
public class PlayerDashRadius : MonoBehaviour {

private PlayerDashChaining playerDashChaining;
private LineRenderer line;
public float dashRadius;
int resolution = 60;

public void Start () {
    playerDashChaining = GetComponentInParent<PlayerDashChaining>();
	dashRadius = playerDashChaining.dashRadius;
	line = GetComponent<LineRenderer>();
}

void Update () {
    line.positionCount = resolution + 1;

    for (var i = 0; i < line.positionCount; i++){
        var angle = (360/line.positionCount+1) * i;
        line.SetPosition(i, transform.position + dashRadius * new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle)));
    }
}

}
