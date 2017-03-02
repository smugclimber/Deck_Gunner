using UnityEngine;
using System.Collections;

public class EnemyPos : MonoBehaviour {

	void OnDrawGizmos(){
		Gizmos.DrawWireSphere(transform.position, 1);
	}
}
