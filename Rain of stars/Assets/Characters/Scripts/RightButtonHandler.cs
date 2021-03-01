using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	public static bool isRightButtonDown;

	public void OnPointerDown(PointerEventData eventData) {
		isRightButtonDown = true;
	}
	public void OnPointerUp(PointerEventData eventData) {
		isRightButtonDown = false;
	}

	// Update is called once per frame
	/*void Update()
    {
        
    }*/
}
