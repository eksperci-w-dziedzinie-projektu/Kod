using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public float moveSpeed;
	public float jumpHeight;

	public Transform groundCheck;												//Transform - rotacja, pozycja
	public float groundCheckRadius;
	public LayerMask WhatIsGround;
	private bool OnTheGround;												//określenie, czy postać jest na ziemi, czy w powietrzu
	private bool doubleJump;

	private Animator anim;

	private const float minimumHeldDuration = 0.05f;
	private float pressTime = 0;
	private bool buttonHeld = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
   	 {
		OnTheGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, WhatIsGround);			//zablokowanie możliwości wielokrotnego skoku

        	if(Input.GetKeyDown(KeyCode.W) && OnTheGround){	//GetKey - przytrzymanie, GetKeyDown - wciśnięcie		//poruszanie się
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpHeight); 					//Vector2 - wektor dwuwymiarowy (x,y)
		}

		if(Input.GetKeyDown(KeyCode.D)){
			pressTime = Time.timeSinceLevelLoad;
			buttonHeld = false;		
		} else if(Input.GetKeyUp(KeyCode.D)){										//gracz jedynie wcisnął przycisk
			if(!buttonHeld) {
				transform.localScale = new Vector3(1f,1f,1f);
			}
		}
		
		if(Input.GetKey(KeyCode.D)){											//gracz przytrzymał przycisk ponad 0,07s
			if(Time.timeSinceLevelLoad - pressTime > minimumHeldDuration) {
				GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
				buttonHeld = true;
			}
		}

		if(Input.GetKeyDown(KeyCode.A)){
			pressTime = Time.timeSinceLevelLoad;
			buttonHeld = false;		
		} else if(Input.GetKeyUp(KeyCode.A)){
			if(!buttonHeld) {
				transform.localScale = new Vector3(-1f,1f,1f);
			}
		}
		
		if(Input.GetKey(KeyCode.A)){
			if(Time.timeSinceLevelLoad - pressTime > minimumHeldDuration) {
				GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
				buttonHeld = true;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////////////////		//podwójny skok
		if(OnTheGround){
			doubleJump = false;
		}
//		anim.SetBool("OnTheGround", OnTheGround);	//BRAK ANIMACJI

		if(Input.GetKeyDown(KeyCode.W) && !OnTheGround && !doubleJump){							//GetKey - przytrzymanie, GetKeyDown - wciśnięcie
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpHeight);
			doubleJump = true;
		}
		/////////////////////////////////////////////////////////////////////////////////////////////////
		anim.SetFloat("Speed",Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));					//ilość liczb między 0 i konkretną liczbą

		if(GetComponent<Rigidbody2D>().velocity.x > 0){									//obrót postaci podczas animacji
			transform.localScale = new Vector3(1f,1f,1f);
		}
		else if(GetComponent<Rigidbody2D>().velocity.x < 0) {
			transform.localScale = new Vector3(-1f,1f,1f);
		}
    	}
}
