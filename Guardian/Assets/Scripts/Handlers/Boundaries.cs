using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
	private Vector2 ScreenBounds;
	private float CharacterWidth;
	private float CharacterHeight;

	public Transform Portal;
	public Transform Core;


	// Start is called before the first frame update
	void Start() {
		ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
		CharacterWidth = transform.GetComponent<BoxCollider2D>().bounds.size.x / 2;
		CharacterHeight = transform.GetComponent<BoxCollider2D>().bounds.size.y / 2;

	}

	// Update is called once per frame
	void LateUpdate() {
		Vector3 position = transform.position;
		position.x = Mathf.Clamp(position.x, Portal.position.x  + CharacterWidth, (Core.position.x* -1) - CharacterWidth);
		position.y = Mathf.Clamp(position.y, Portal.position.y  + CharacterHeight, (Core.position.y* -1) - CharacterHeight);
		transform.position = position;
	}
}
