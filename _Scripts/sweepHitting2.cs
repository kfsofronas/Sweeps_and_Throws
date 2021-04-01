using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sweepHitting2 : MonoBehaviour
{

	public bool hit = false;

	void OnTriggerEnter2D(Collider2D col)
	{
		hit = true;
		if (col.gameObject.tag.Equals("Player2") || col.gameObject.tag.Equals("P2SweepHitbox")) { /*do nothing*/ }
		else if (col.gameObject.tag.Equals("Player1")) { col.gameObject.GetComponent<PlayerController>().hit_by_sweep = true; print("collided with player 1"); }
		else if (col.gameObject.tag.Equals("P1SweepHitbox")) { col.transform.parent.gameObject.GetComponent<Player2Controller>().hit_by_sweep = true; print("collided with player 1 sweep hitbox"); }
	}
}
