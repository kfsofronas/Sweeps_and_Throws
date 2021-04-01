using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float speed = 0.13f;
	public float MINIMUM_TILT = 0.1f;
	public int SWEEPSTARTUP = 25;
	public int SWEEPACTIVE = 17;
	public int THROWSTARTUP = 15;
	public int THROWACTIVE = 14;
	public float THROWTECHPUSHBACK = 0.2f;
	private Vector3 techPushbackVector;
	public float SWEEPPUSHBACK = 0.3f;
	private Vector3 sweepPushbackVector;
	public bool controllable = true;
	public bool forward = false;
	public bool backward = false;
	public bool still = false;
	public bool crouch = false;
	public bool sweep = false;
	public bool throwing = false;
	public bool being_thrown = false;
	public bool firstCrouch = true;
	public bool hit_by_sweep = false;
	public bool actually_hit_by_sweep = false;
	public bool throw_connect = false;
	public bool invulnerable = false;
	public bool block = false;
	public bool acutallyBlocking = false;
	public bool throwTech = false;
	public bool techPushback = false;
	public int techPushbackCount = 0;
	public bool sweepPushback = false;
	public int sweepPushbackCount = 0;
	public bool healthDone = false;
	public int health = 3;
	public bool gameOver = false;
	public bool win = false;
	public Sprite dead;

	private Transform trans;
	Vector3 movement_fwd;
	Vector3 movement_back;
	public Animator anim;
	public SpriteRenderer spriteRender;

	public bool sweepAnimFirst = true;
	public int sweepAnimCount = 0;
	public bool throwAnimFirst = true;
	public int throwAnimCount = 0;
	public bool blockAnimFirst = true;
	public int blockAnimCount = 0;

	public BoxCollider2D playerHurtboxCollider;
	public GameObject sweepHitbox;
	public GameObject throwHitbox;
	public BoxCollider2D sweepHitboxCollider;
	public BoxCollider2D throwHitboxCollider;
	public bool actuallyBlocking;

	public float throwAnim1 = -1.5f;
	public bool anim1done = false;
	public float throwAnim2 = 0.25f;
	public bool anim2done = false;
	public float throwAnim3 = 4.5f;
	public bool anim3done = false;
	public Transform player2Transform;

	public Vector3 newPos = new Vector3(0, 0, 0);


	// Start is called before the first frame update
	void Start()
	{
		trans = GetComponent<Transform>();
		anim = GetComponent<Animator>();
		spriteRender = GetComponent<SpriteRenderer>();
		still = true;
		movement_fwd = new Vector3(speed, 0, 0);
		movement_back = new Vector3(-1 * speed, 0, 0);
		playerHurtboxCollider = GetComponent<BoxCollider2D>();
		sweepHitbox = trans.Find("Sweep Hitbox").gameObject;
		throwHitbox = trans.Find("Throw Hitbox").gameObject;
		sweepHitboxCollider = sweepHitbox.GetComponent<BoxCollider2D>();
		throwHitboxCollider = throwHitbox.GetComponent<BoxCollider2D>();
		techPushbackVector = new Vector3(-1 * THROWTECHPUSHBACK, 0, 0);
		sweepPushbackVector = new Vector3(-1 * SWEEPPUSHBACK, 0, 0);
		player2Transform = GameObject.FindGameObjectWithTag("Player2").transform;
	}

	// Update is called once per frame
	void Update()
	{

		if(Input.GetAxisRaw("Reset Game") > MINIMUM_TILT) {
			print("reset game");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		trans.rotation = new Quaternion(0, 0, 0, 0);
		trans.position = new Vector3(trans.position.x, 0.4f, 0);

		if (!gameOver)
		{
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_ThrowHit") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_ThrowMiss") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Sweep") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_HitSweep") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_HitThrow") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_GetUp") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Crouch2Stand") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Stand2Crouch") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_CrouchBlock") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_ThrowTech"))
			{
				controllable = false;
			}
			else { controllable = true; }

			if (controllable)
			{
				if (Input.GetAxisRaw("P1_Horizontal") > MINIMUM_TILT) { forward = true; still = false; backward = false; }
				else if (Input.GetAxisRaw("P1_Horizontal") < (MINIMUM_TILT * -1)) { backward = true; still = false; forward = false; }
				else { still = true; forward = false; backward = false; }

				if (Input.GetAxisRaw("P1_Sweep") > MINIMUM_TILT) { sweep = true; } else { sweep = false; }
				if (Input.GetAxisRaw("P1_Throw") > MINIMUM_TILT) { throwing = true; } else { throwing = false; }
			}
			else
			{
				forward = false; backward = false; still = true;
			}

			if (Input.GetAxisRaw("P1_Vertical") < MINIMUM_TILT * -1) { crouch = true; }
			else { crouch = false; }

			if (spriteRender.sprite.name.Equals("Sweep4")) { sweepHitbox.SetActive(true); } else { sweepHitbox.SetActive(false); }
			if (spriteRender.sprite.name.Equals("grab1")) { throwHitbox.SetActive(true); } else { throwHitbox.SetActive(false); }
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_HitSweep") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_HitThrow") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_GetUp") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_CrouchBlock") || anim.GetCurrentAnimatorStateInfo(0).IsName("Player_ThrowTech"))
			{
				invulnerable = true; playerHurtboxCollider.enabled = false;

				actually_hit_by_sweep = false;
				hit_by_sweep = false;
				being_thrown = false;
				throwTech = false;
				anim1done = false;
				anim2done = false;
				anim3done = false;

			}
			else { invulnerable = false; playerHurtboxCollider.enabled = true; }

			if ((crouch && backward && controllable) || (crouch && backward && anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Stand2Crouch"))) { block = true; } else { block = false; }

			if (hit_by_sweep && block) { actuallyBlocking = true; } else { actuallyBlocking = false; }
			if (hit_by_sweep && !block) { actually_hit_by_sweep = true; } else { actually_hit_by_sweep = false; }

			//fixes infinite throw
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Player_ThrowMiss") || !anim.GetCurrentAnimatorStateInfo(0).IsName("Player_ThrowMiss")) { throw_connect = false; };

			//throw tech
			if (throwTech && spriteRender.sprite.name.Equals("grab1"))
			{
				being_thrown = false;
				trans.Translate(techPushbackVector);
			}

			if (spriteRender.sprite.name.Equals("crouchBlock"))
			{
				trans.Translate(sweepPushbackVector);
			}

			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_HitThrow")) { GetComponent<BoxCollider2D>().enabled = false; } else { GetComponent<BoxCollider2D>().enabled = true; }
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_HitThrow") && spriteRender.sprite.name.Equals("hit_throw1") && !anim1done) { newPos.x = player2Transform.position.x - throwAnim1; trans.position = newPos; anim1done = true; print("did anim1"); print("anim1done = " + anim1done.ToString()); }
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_HitThrow") && spriteRender.sprite.name.Equals("hit_throw2flipped") && !anim2done) { newPos.x = player2Transform.position.x - throwAnim2; trans.position = newPos; anim2done = true; }
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_HitThrow") && spriteRender.sprite.name.Equals("hit_sweep2") && !anim3done) { newPos.x = player2Transform.position.x - throwAnim3; trans.position = newPos; anim3done = true; }

			//health
			if (spriteRender.sprite.name.Equals("hit_sweep2") && !healthDone)
			{
				health--;
				healthDone = true;
			}

			if (!spriteRender.sprite.name.Equals("hit_sweep2")) { healthDone = false; }

			/*
			//enable sweep hibox at correct time
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Sweep"))
			{
				if (sweepAnimFirst) { sweepAnimCount = 0; sweepAnimFirst = false; }
				else { sweepAnimCount++;
					if (sweepAnimCount == SWEEPSTARTUP) { sweepHibox.SetActive(true); }
					else if (sweepAnimCount == SWEEPSTARTUP + SWEEPACTIVE) { sweepHibox.SetActive(false); }
				}
			} else { sweepAnimFirst = true; }
	
			
	
			//enable throw hitbox at correct time
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_ThrowMiss"))
			{
				if (throwAnimFirst) { throwAnimCount = 0; throwAnimFirst = false; }
				else
				{
					throwAnimCount++;
					if (throwAnimCount == THROWSTARTUP) { throwHibox.SetActive(true); }
					else if (throwAnimCount == THROWSTARTUP + THROWACTIVE) { throwHibox.SetActive(false); }
				}
			} else { throwHibox.SetActive(false); throwAnimFirst = true; }
	
			*/

			/*if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Player_ThrowHit") || !anim.GetCurrentAnimatorStateInfo(0).IsName("Player_ThrowMiss"))
			{
				throwing = false;
			}
			if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Sweep"))
			{
				sweep = false;
			}*/

			if (forward && !crouch) { trans.Translate(movement_fwd); }
			if (backward && !crouch) { trans.Translate(movement_back); }

			if (health == 0) { gameOver = true; win = false; }
			if (GameObject.FindGameObjectWithTag("Player2").GetComponent<Player2Controller>().health == 0) { gameOver = true; win = true; trans.parent.GetChild(0).gameObject.SetActive(true); }

		}

		Animate();

		if (gameOver) {
			controllable = false;
			sweepHitbox.SetActive(false);
			throwHitbox.SetActive(false);
			if(!win) { spriteRender.sprite = dead; }
		}

	}

	void FixedUpdate() { }

	void Animate() {

		anim.SetBool("walkf", forward);
		anim.SetBool("still", still);
		anim.SetBool("walkb", backward);
		anim.SetBool("throw", throwing);
		anim.SetBool("firstCrouch", firstCrouch);
		anim.SetBool("crouch", crouch);
		anim.SetBool("sweep", sweep);
		anim.SetBool("hitSweep", actually_hit_by_sweep);
		anim.SetBool("hitThrow", being_thrown);
		anim.SetBool("throwConnect", throw_connect);
		anim.SetBool("blocking", actuallyBlocking);
		anim.SetBool("techThrow", throwTech);
		GameObject.FindGameObjectWithTag("P1Health").GetComponent<Animator>().SetInteger("health", health);
		anim.SetBool("win", win);
	}
}