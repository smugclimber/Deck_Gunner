using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

	public bool autoPlay = false;
	public float shipSpeed = 15.0f;
	public float padding = 1f;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate = 0.2f;	
	public float health = 250f;
	
	private float xMin, xMax;
	
	// Use this for initialization
	void Start () {
		// Below we use the Camera to obtain the gamespaces left and most right deminsions
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftMost.x + padding;
		xMax = rightMost.x - padding;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){ //turns on firing rate when shooting
			InvokeRepeating ("FireProjectile", 0.00001f, firingRate);
		}
		if (Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke("FireProjectile");
		}
		if (!autoPlay){ //aka autoPlay == false
			MoveWithKeyboard();
		}else{
			AutoPlay();

		}
	}
	
	void MoveWithKeyboard (){
		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.position += Vector3.left * shipSpeed * Time.deltaTime;
		} else if (Input.GetKey(KeyCode.RightArrow)){
			transform.position += Vector3.right * shipSpeed * Time.deltaTime;
		}
		// restict player to the gamespace
		float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
		transform.position = new Vector3(newX, transform.position.y, 0f);
	}
	
	void FireProjectile(){
		Vector3 safeDist = transform.position + new Vector3(0,0.6f,0);
		GameObject beam = Instantiate(projectile, safeDist, Quaternion.identity) as GameObject; //Instatiate normally produces Objects and we want a GameObject
		beam.rigidbody2D.velocity = new Vector3(0, projectileSpeed, 0);
	}
	void OnTriggerEnter2D(Collider2D collidingProj){
		Projectile missile =  collidingProj.gameObject.GetComponent<Projectile>(); //creates new object within Enemy upon collision of gameObjects with Projectile components
		if(missile){ //If misille is true and exists because it has Projectile components
			Debug.Log("Player collided with missile");
			health -= missile.GetDamage();
			missile.Hit();
			if(health <= 0){
				Destroy (gameObject); 
			}
		}
	}
	
	void AutoPlay(){

	}
		
}
