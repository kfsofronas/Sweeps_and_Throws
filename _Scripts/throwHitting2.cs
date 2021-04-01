using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwHitting2 : MonoBehaviour
{

	public bool hit = false;

	void OnTriggerEnter2D(Collider2D col)
	{
		hit = true;
		if (col.gameObject.tag.Equals("Player2") || col.gameObject.tag.Equals("P2ThrowHitbox")) { /*do nothing*/ }
		else if (col.gameObject.tag.Equals("Player1") && !col.gameObject.GetComponent<PlayerController>().invulnerable) { col.gameObject.GetComponent<PlayerController>().being_thrown = true; print("collided with player 2"); /*this doesnt work somehow*/ gameObject.GetComponentInParent<Player2Controller>().throw_connect = true; }
		else if (col.gameObject.tag.Equals("P1ThrowHitbox")) { gameObject.GetComponentInParent<Player2Controller>().throwTech = true; print("TECHED THE THROW!2"); }
	}
}
