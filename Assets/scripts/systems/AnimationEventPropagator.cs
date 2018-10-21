using UnityEngine;
using UnityEngine.Events;

public class AnimationEventPropagator : MonoBehaviour {

	public UnityEvent[] events;

	public void Invoke(int elementID){
		Debug.Log("Event " + elementID + "invoked on " + gameObject.name, gameObject);
		if(events[elementID] != null)
			events[elementID].Invoke();
	}


}
