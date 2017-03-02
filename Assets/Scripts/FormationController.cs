using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width = 10f;
	public float height = 5f;
	public float speed = 5f;
	public float spawnDelay = 0.5f;
	
	private bool movingRight = true;
	private float xMax;
	private float xMin;
	
	// Use this for initialization
	void Start () {
		
		//Calculate the edge of gamescreen
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 rightBoundry = Camera.main.ViewportToWorldPoint(new Vector3(1,0, distanceToCamera));
		Vector3 leftBoundry = Camera.main.ViewportToWorldPoint(new Vector3(0,0, distanceToCamera));
		xMax = rightBoundry.x;
		xMin = leftBoundry.x;
		EnemySpawn();
	}
	
	public void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
	}
	
	// Update is called once per frame
	void Update () {
		MoveFormation();
		if(AllMembersDead()){
			Debug.Log("Empty Formation");
			SpawnUntilFull();
		}
	}
	
	void EnemySpawn(){
		foreach(Transform child in transform){
			// Below creates a clone of a designated prefab as a GameObject...not an object as Instantiate normally produces.
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child; //child is the EnemyFormation's position transform and keeps clone organization tidy.
			}
	}
	
	void SpawnUntilFull(){
		Transform freePosition = NextFreePosition();
		if(freePosition){
			GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition; 
		}
		if(NextFreePosition()){
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}
	
	Transform NextFreePosition(){
		foreach(Transform childpositionGameObject in transform){
			if (childpositionGameObject.childCount == 0){
				return childpositionGameObject;
			}
		}
		return null;
	}
	
	bool AllMembersDead(){
		foreach(Transform childpositionGameObject in transform){
			if (childpositionGameObject.childCount > 0){
				return false;
			}
		}
		return true;
	}
	
	void MoveFormation(){
		if(movingRight){
			transform.position += Vector3.right * speed * Time.deltaTime;
		}else{
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		
		float rightEdgeOfFormation = transform.position.x + (0.5f*width);
		float leftEdgeOfFormation = transform.position.x - (0.5f*width);
		// || is same as a logical "or" and reverses direction
		if(leftEdgeOfFormation < xMin){
			movingRight = true;
		}
		if(rightEdgeOfFormation > xMax){
			movingRight = false;
		}
	}
}
