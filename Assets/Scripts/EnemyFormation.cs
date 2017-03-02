using UnityEngine;
using System.Collections;

public class EnemyFormation : MonoBehaviour {
	public float health = 150f;
	public GameObject projectile;
	public float projectileSpeed;
	public float enemyFirRate = 0.5f;
	
	private int deadEnemyCount;
	
	void Update () {
		float probability = enemyFirRate*Time.deltaTime;
		if (Random.value < probability){
			FireEnemyproj();
		}
	}

	
	
	void FireEnemyproj(){
		Vector3 safeDist = transform.position + new Vector3(0,-0.6f,0);
		GameObject enemyBeam = Instantiate(projectile, safeDist, Quaternion.identity) as GameObject; //Instatiate normally produces Objects and we want a GameObject
		enemyBeam.rigidbody2D.velocity = new Vector3(0, projectileSpeed, 0);
	}
	
	void OnTriggerEnter2D(Collider2D collidingProj){
		Projectile missile =  collidingProj.gameObject.GetComponent<Projectile>(); //creates new object within Enemy upon collision of gameObjects with Projectile components
		if(missile){ //If misille is true and exists because it has Projectile components
			health -= missile.GetDamage();
			missile.Hit();
			if(health <= 0){
				deadEnemyCount ++;
				Destroy (gameObject); 
			}
		}
	}
}
