using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class ExtendedStandaloneInputModule : StandaloneInputModule {

	public static ExtendedStandaloneInputModule instance;

	public PointerEventData GetPointerEventData(int pointerID = -1) {
		PointerEventData data;
		instance.GetPointerData(pointerID, out data, true);
		return data;

	}

	protected override void Awake(){
		base.Awake();
		instance = this;
			
	}

}
