using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform Target;
	public float SmoothSpeed = 0.125f;
	public Vector3 offset = new Vector3(0,0,-3);

	private void LateUpdate() {
		if (Target == null)
			return;
		Vector3 desiredPostion = new Vector3(Target.position.x, GameObject.FindGameObjectWithTag("MainCamera").transform.position.y, Target.position.z) + offset;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPostion, SmoothSpeed);
		transform.position = smoothedPosition;
	}


}
