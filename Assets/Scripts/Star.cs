using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	public AudioClip collectSound;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			renderer.enabled = false;	
			collider2D.enabled = false;
			audio.PlayOneShot(collectSound);
			particleSystem.Emit(10);
			StartCoroutine("kill",2);
		}
	}


	IEnumerator kill(float seconds)
	{
		yield return new WaitForSeconds (seconds);
		Destroy (gameObject);
	}
}
