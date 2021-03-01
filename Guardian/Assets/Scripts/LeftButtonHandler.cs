using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeftButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	public static bool isLeftButtonDown;

	public void OnPointerDown(PointerEventData eventData) {
		isLeftButtonDown = true;
	}
	public void OnPointerUp(PointerEventData eventData) {
		isLeftButtonDown = false;
	}
}
