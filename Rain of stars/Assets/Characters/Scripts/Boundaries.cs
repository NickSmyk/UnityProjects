using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
	private Vector2 ScreenBounds;
	private float CharacterWidth;
	private float CharacterHeight;


    // Start is called before the first frame update
    void Start()
    {
		ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
		CharacterWidth = transform.GetComponent<BoxCollider2D>().bounds.size.x / 2;
		CharacterHeight = transform.GetComponent<BoxCollider2D>().bounds.size.y / 2;

    }

    // Update is called once per frame
    void LateUpdate()
    {
		Vector3 position = transform.position;
		position.x = Mathf.Clamp(position.x, ScreenBounds.x* -1 + CharacterWidth, (ScreenBounds.x ) - CharacterWidth);
		position.y = Mathf.Clamp(position.y, ScreenBounds.y * -1 + CharacterHeight, (ScreenBounds.y) - CharacterHeight);
		transform.position = position;
	}
}
