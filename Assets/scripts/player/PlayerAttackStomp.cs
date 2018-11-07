using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerArcLine), typeof(TargetAreaCircle), typeof(TargetAreaCircle))]
public class PlayerAttackStomp : PlayerAttack
{
    private PlayerArcLine line;
    private TargetAreaCircle areaCircle, targetCircle;
    private Vector3 tempTargetPos;

    protected override void Start(){
        base.Start();
        line = GetComponent<PlayerArcLine>();
        TargetAreaCircle[] targetAreaCircles = GetComponents<TargetAreaCircle>();
        areaCircle = targetAreaCircles[0];
        targetCircle = targetAreaCircles[1];
    }

    public override void StartDrawing()
    {
        base.StartDrawing();
        line.Toggle(true);
        line.useTargetPos = true;
        areaCircle.visible = true;
        targetCircle.visible = true;
    }

    public override void EndDrawing()
    {
        base.EndDrawing();
        line.Toggle(false);
        areaCircle.visible = false;
        targetCircle.visible = false;
    }

    public override IEnumerator DrawVisuals()
    {
        Vector3 targetCircleHeight = Vector3.up * 0.1f;
        while(PlayerInput.pointerDown){
            tempTargetPos = ExtendedStandaloneInputModule.instance.GetPointerEventData().pointerCurrentRaycast.worldPosition;
            line.targetPos = tempTargetPos;
            if(areaCircle.Contains(Vector3.Distance(transform.position, tempTargetPos))){
                line.RenderArc(true);
                targetCircle.visible = true;
                targetCircle.RelocateOrigin(tempTargetPos + targetCircleHeight);
            } else {
                line.RenderArc(false);
                targetCircle.visible = false;
            }
            yield return null;
        }
    }

    protected override void OnDestroy(){
        base.OnDestroy();
    }
}
