using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPath : ScriptableObject {

	[SerializeField] Transform m_target;
	[SerializeField] Vector3 targetCoordinates;
	[SerializeField] int m_stepsToTarget;
	[SerializeField] List<Vector3> path;

	public List<Vector3> Path {
		get{
			return path;
		}
		set{
			return;
		}
	}
	bool lerping = false;

	int PosNeg {
		get { return (int) Mathf.Sign(Random.Range(-1,1)); }
	}

	public void CreatePath(Transform transform, Transform target, int steps, Color startColor, LayerMask environmentCheck, float debugRayDuration){
		path = new List<Vector3>();
		
		//member allocation
		m_stepsToTarget = steps;
		m_target = target;
		//m_transform = transform;
		targetCoordinates = target.position;
		targetCoordinates.y = 0;
		
		//color juggling
		float H,S,V;
		Color.RGBToHSV(startColor, out H, out S, out V);
		Color[] colors = new Color[3];
		for(int i = 0; i < colors.Length; i++){
			colors[i] = Color.HSVToRGB(H, S, V);
			H = Mathf.Repeat(H + 0.165f, 1);
		}

		//internal attribute declaration and initialization
		Vector3 currentPosition = transform.position, sidePosition, forwardPosition, targetVector, sideStep, forwardStep;
		float sideStepLength, forwardStepLength;
		float reductionRate =  1f / (float) m_stepsToTarget;
		float targetDistance = Vector3.Distance(currentPosition, targetCoordinates);
		float targetDistanceReductionRate = targetDistance / m_stepsToTarget;
		int randomSign = PosNeg;
		//targetVector = targetCoordinates - currentPosition;
		//targetVector.y = 0f;

		path.Add(currentPosition);

		for(int i = m_stepsToTarget; i > 0; i--){

			targetVector = targetCoordinates - currentPosition;
			targetVector.y = 0f;

			//diminish the step lengths
			sideStepLength = (Random.Range(0.8f, 0.95f) * i * targetDistanceReductionRate);
			forwardStepLength = (Random.Range(0.7f, 0.95f) * i * targetDistanceReductionRate);
			//one in four chance of increased step length
			sideStepLength *= (Random.value < 0.25f) ? Random.value + 1 : 1;
			forwardStepLength *= (Random.value < 0.25f) ? Random.value + 1 : 1;

			sideStep = Vector3.Cross(transform.up * randomSign, targetVector).normalized * sideStepLength;
			forwardStep = Vector3.Cross(transform.up * -randomSign, sideStep).normalized * forwardStepLength;
			
			int a = 0;
			RaycastHit hit;
			//Physics.Raycast(currentPosition + sideStep, forwardStep, forwardStep.magnitude + 1, environmentCheck, QueryTriggerInteraction.Collide)
			//Physics.SphereCast(currentPosition + sideStep, 1f, forwardStep, out hit, forwardStep.magnitude + 1000, environmentCheck, QueryTriggerInteraction.Collide)
			Debug.DrawRay(currentPosition + sideStep, forwardStep, Color.white, 10f, false);
			while(Physics.Raycast(currentPosition + sideStep, forwardStep, out hit, forwardStep.magnitude + 1, environmentCheck, QueryTriggerInteraction.Collide)){
				//Debug.DrawRay(currentPosition + sideStep, forwardStep, Color.white, 10f, false);
				if(a > 50) {
					//Debug.Log("First loop break");
					break;
				}
				a++;
				//Debug.Log("Spherecast hit something " + a + " " + hit.collider.transform.root.name);
				sideStep *= 1.33f;
				//forwardStep = Vector3.Cross(transform.up * -randomSign, sideStep).normalized * forwardStepLength;
			}
			
			sidePosition = currentPosition + sideStep;
			path.Add(sidePosition);
			Debug.DrawLine(currentPosition, sidePosition, colors[0], debugRayDuration, false);
			forwardPosition = sidePosition + forwardStep;

			targetVector = targetCoordinates - forwardPosition;
			//targetVector.y = 1;
			
			a = 0;
			while(targetVector.magnitude < i * targetDistanceReductionRate || targetVector.magnitude > i * targetDistanceReductionRate * 2){
				if(a > 50) {
					//Debug.Log("Second loop break");
					break;
				}
				a++;
				//Debug.Log("Second loop step " + a);
				if(targetVector.magnitude < i * targetDistanceReductionRate){
					forwardPosition -= targetVector.normalized * ((i * targetDistanceReductionRate) - targetVector.magnitude);
					targetVector = targetCoordinates - forwardPosition;
				} else if(targetVector.magnitude > i * targetDistanceReductionRate * 2){
					forwardPosition += targetVector.normalized * ((i * targetDistanceReductionRate) + targetVector.magnitude);
					targetVector = targetCoordinates - forwardPosition;
				}
			}

			Debug.DrawLine(sidePosition, forwardPosition, colors[1], debugRayDuration, false);

			currentPosition = forwardPosition;

			path.Add(currentPosition);

			randomSign = (i % 2 == 0) ? PosNeg : -randomSign;
			
		}

		Debug.DrawLine(currentPosition, targetCoordinates, colors[2], debugRayDuration, false);

		path.Add(targetCoordinates);

		DrawFinishedPath(debugRayDuration);

	}

	[ExecuteInEditMode]
	public IEnumerator CreatePathCoroutine(MonoBehaviour runner, Transform transform, Transform target, int steps, Color startColor, LayerMask environmentCheck, float debugRayDuration){
		path = new List<Vector3>();
		
		//member allocation
		m_stepsToTarget = steps;
		m_target = target;
		//m_transform = transform;
		targetCoordinates = target.position;
		targetCoordinates.y = 0;
		
		//color juggling
		float H,S,V;
		Color.RGBToHSV(startColor, out H, out S, out V);
		Color[] colors = new Color[3];
		for(int i = 0; i < colors.Length; i++){
			colors[i] = Color.HSVToRGB(H, S, V);
			H = Mathf.Repeat(H + 0.165f, 1);
		}

		GameObject computePos = GameObject.CreatePrimitive(PrimitiveType.Cube);
		computePos.transform.localScale = new Vector3(0.5f, 3f, 0.5f);
		Material mat = computePos.GetComponent<MeshRenderer>().material;

		//internal attribute declaration and initialization
		Vector3 currentPosition = transform.position, sidePosition, forwardPosition, targetVector, sideStep, forwardStep;
		float sideStepLength, forwardStepLength;
		float reductionRate =  1f / (float) m_stepsToTarget;
		float targetDistance = Vector3.Distance(currentPosition, targetCoordinates);
		float targetDistanceReductionRate = targetDistance / m_stepsToTarget;
		int randomSign = PosNeg;
		//targetVector = targetCoordinates - currentPosition;
		//targetVector.y = 0f;

		path.Add(currentPosition);

		for(int i = m_stepsToTarget; i > 0; i--){
			//Debug.Log("_________" + i + "___________ ");
			runner.StartCoroutine(LerpComputePos(computePos, currentPosition, 0.25f));
			yield return new WaitUntil(() => lerping == false);
			//computePos.transform.position = currentPosition;
			//yield return new WaitForSecondsRealtime(1f);

			targetVector = targetCoordinates - currentPosition;
			targetVector.y = 0f;

			//diminish the step lengths
			sideStepLength = (Random.Range(0.8f, 0.95f) * i * targetDistanceReductionRate);
			forwardStepLength = (Random.Range(0.7f, 0.95f) * i * targetDistanceReductionRate);
			//one in four chance of increased step length
			sideStepLength *= (Random.value < 0.25f) ? Random.value + 1 : 1;
			forwardStepLength *= (Random.value < 0.25f) ? Random.value + 1 : 1;

			sideStep = Vector3.Cross(transform.up * randomSign, targetVector).normalized * sideStepLength;
			forwardStep = Vector3.Cross(transform.up * -randomSign, sideStep).normalized * forwardStepLength;
			
			sidePosition = currentPosition + sideStep;

			//computePos.transform.position = sidePosition;
			mat.color = colors[0];
			//Debug.Log("First sidePosition try " + i);
			runner.StartCoroutine(LerpComputePos(computePos, sidePosition, 3f));
			yield return new WaitUntil(() => lerping == false);
			//yield return new WaitForSecondsRealtime(3f);

			int a = 0;
			RaycastHit hit;
			//Physics.Raycast(currentPosition + sideStep, forwardStep, forwardStep.magnitude + 1, environmentCheck, QueryTriggerInteraction.Collide)
			//Physics.SphereCast(currentPosition + sideStep, 1f, forwardStep, out hit, forwardStep.magnitude + 1000, environmentCheck, QueryTriggerInteraction.Collide)
			//Debug.DrawRay(currentPosition + sideStep, forwardStep, Color.white, 10f, false);
			while(Physics.Raycast(sidePosition, forwardStep, out hit, forwardStep.magnitude + 10, environmentCheck, QueryTriggerInteraction.Collide)){
				//Debug.DrawRay(currentPosition + sideStep, forwardStep, Color.white, 10f, false);
				if(a > 50) {
					//Debug.Log("First loop break");
					break;
				}
				a++;
				//Debug.Log("Spherecast hit something " + a + " " + hit.collider.transform.root.name);
				sidePosition += sideStep * 0.33f;
				computePos.transform.position = sidePosition;
				//Debug.Log("Sideposition changed " + a);
				yield return new WaitForSecondsRealtime(0.5f);
				//forwardStep = Vector3.Cross(transform.up * -randomSign, sideStep).normalized * forwardStepLength;
			}

			Debug.DrawLine(currentPosition, sidePosition, colors[0], debugRayDuration, false);
			
			path.Add(sidePosition);

			//yield return new WaitForSecondsRealtime(0.5f);
			
			forwardPosition = sidePosition + forwardStep;

			//computePos.transform.position = forwardPosition;
			mat.color = colors[1];
			//Debug.Log("First forwardPosition try " + i);
			runner.StartCoroutine(LerpComputePos(computePos, forwardPosition, 3f));
			yield return new WaitUntil(() => lerping == false);
			//yield return new WaitForSecondsRealtime(3f);

			targetVector = targetCoordinates - forwardPosition;
			//targetVector.y = 1;

			a = 0;
			while(targetVector.magnitude < i * targetDistanceReductionRate || targetVector.magnitude > i * targetDistanceReductionRate + ((m_stepsToTarget - i) + 1)){
				//Debug.Log("");
				if(a > 50) {
					//Debug.Log("Second loop break");
					break;
				}

				a++;

				//Debug.Log("Upper tolerance : " + (i * targetDistanceReductionRate + ((m_stepsToTarget - i) + 1)));
				//Debug.Log("Distance from target: " + targetVector.magnitude);
				//Debug.Log("Lower tolerance: " + (i * targetDistanceReductionRate));
				
				if(targetVector.magnitude < i * targetDistanceReductionRate){
					forwardPosition -= targetVector.normalized * ((i * targetDistanceReductionRate + 1) - targetVector.magnitude);
					targetVector = targetCoordinates - forwardPosition;
					//Debug.Log("Forwardposition increased " + a);
				} else if(targetVector.magnitude > i * targetDistanceReductionRate + ((m_stepsToTarget - i) + 1)){
					forwardPosition += targetVector.normalized * (targetVector.magnitude - (i * targetDistanceReductionRate)) / 2;
					targetVector = targetCoordinates - forwardPosition;
					//Debug.Log("Forwardposition decreased " + a);
				}
				computePos.transform.position = forwardPosition;
				//Debug.Log("New Distance from target: " + targetVector.magnitude);
				yield return new WaitForSecondsRealtime(0.5f);
			}

			

			Debug.DrawLine(sidePosition, forwardPosition, colors[1], debugRayDuration, false);

			currentPosition = forwardPosition;

			path.Add(currentPosition);

			randomSign = (i % 2 == 0) ? PosNeg : -randomSign;

			//yield return new WaitForSecondsRealtime(0.5f);
		}
		

		Debug.DrawLine(currentPosition, targetCoordinates, colors[2], debugRayDuration, false);
		path.Add(targetCoordinates);

		DrawFinishedPath(debugRayDuration);

	}

	void DrawFinishedPath(float debugRayDuration){
		for(int i = 0; i + 1 < path.Count; i++){
			Vector3 pos = path[i], nextPos = path[i+1];
			pos.y = nextPos.y = 1f;
			Debug.DrawLine(pos, nextPos, Color.white, debugRayDuration, false);
		}
	}

	IEnumerator LerpComputePos(GameObject computePos, Vector3 toPos, float time){
		lerping = true;
		Vector3 startPos = computePos.transform.position;
		for(float t = 0; t <= time; t = t + Time.deltaTime*2){
			computePos.transform.position = Vector3.Lerp(startPos, toPos, t / time);
			yield return null;
		}
		computePos.transform.position = toPos;
		lerping = false;
	}

}
