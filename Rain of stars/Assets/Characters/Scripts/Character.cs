using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	private Rigidbody2D rb;
	private Animator anim;
	private float moveSpeed;
	private float dirX;
	private bool facingRight = true;
	private Vector3 localScale;
    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		localScale = transform.localScale;
		moveSpeed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
		if (GameControlScript.health == 0)
			return;
		//dirX = CrossPlatformInputManager.GetAxisRaw("Horizontal") * moveSpeed;
		dirX = RightButtonHandler.isRightButtonDown && LeftButtonHandler.isLeftButtonDown? 0f:
			(RightButtonHandler.isRightButtonDown? 1f : (LeftButtonHandler.isLeftButtonDown? -1f : 0));
		transform.position += new Vector3(dirX, 0, 0) * Time.deltaTime * moveSpeed;


		/*if (CrossPlatformInputManager.GetButtonDown("Jump"))
			rb.AddForce(Vector2.up * 300f);*/

		//anim.SetBool("isRunning", false);
		if (Mathf.Abs(dirX) > 0) {
			anim.SetBool("isRunning", true);
			FindObjectOfType<AudioManager>().Play("SnailMovement");
		} else {
			anim.SetBool("isRunning", false);
			FindObjectOfType<AudioManager>().Stop("SnailMovement");
		}
			

    }

	private void FixedUpdate() {
		rb.velocity = new Vector2(dirX, rb.velocity.y);
	}
	private void LateUpdate() {
		if (Time.timeScale == 0f) {
			return;
		}
		if (dirX > 0)
			facingRight = true;
		else if (dirX < 0)
			facingRight = false;
		if ((facingRight && localScale.x < 0) || (!facingRight && localScale.x > 0))
			localScale.x *= -1;

		transform.localScale = localScale;
	}
}
