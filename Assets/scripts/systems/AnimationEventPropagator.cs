using UnityEngine;
using UnityEngine.Events;

public class AnimationEventPropagator : MonoBehaviour {

	public UnityEvent[] events;

	public void Invoke(int elementID){
		if(events[elementID] != null)
			events[elementID].Invoke();
	}


}
