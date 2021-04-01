using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sweepHitting : MonoBehaviour
{

	public bool hit = false;

	void OnTriggerEnter2D(Collider2D col)
	{
		hit = true;
		if (col.gameObject.tag.Equals("Player1") || col.gameObject.tag.Equals("P1SweepHitbox")) { /*do nothing*/ }
		else if (col.gameObject.tag.Equals("Player2")) { col.gameObject.GetComponent<Player2Controller>().hit_by_sweep = true; print("collided with player 2"); }
		else if (col.gameObject.tag.Equals("P2SweepHitbox")) { col.transform.parent.gameObject.GetComponent<PlayerController>().hit_by_sweep = true; print("collided with player 2 sweep hitbox"); }
	}
}
