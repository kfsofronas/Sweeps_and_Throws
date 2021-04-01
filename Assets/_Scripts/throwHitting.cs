using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwHitting : MonoBehaviour
{

	public bool hit = false;

	void OnTriggerEnter2D(Collider2D col)
	{
		hit = true;
		if (col.gameObject.tag.Equals("Player1") || col.gameObject.tag.Equals("P1ThrowHitbox")) { /*do nothing*/ }
		else if (col.gameObject.tag.Equals("Player2") && !col.gameObject.GetComponent<Player2Controller>().invulnerable) { col.gameObject.GetComponent<Player2Controller>().being_thrown = true; print("collided with player 2"); /*this doesnt work somehow*/ gameObject.GetComponentInParent<PlayerController>().throw_connect = true; }
		else if (col.gameObject.tag.Equals("P2ThrowHitbox")) { /*throw tech*/ gameObject.GetComponentInParent<PlayerController>().throwTech = true; print("TECHED THE THROW!1"); }
	}
}
