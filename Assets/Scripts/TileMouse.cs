using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Dmanager))]

public class TileMouse : MonoBehaviour {
		
	Dmanager _dmanager;

	Vector3 currentTileCoord;

	public Transform highLightTile;


	void Start() {
		_dmanager = GetComponent<Dmanager> ();

	}


	void Update () {
	
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;

		if ( GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity)) {
			int x = Mathf.FloorToInt ( hitInfo.point.x/ _dmanager.tileSize);
			int z = Mathf.FloorToInt ( hitInfo.point.z/ _dmanager.tileSize);
			Debug.Log ("Tile: "+ x +", " + z);

			currentTileCoord.x = x;
			currentTileCoord.z = z;

			highLightTile.GetComponent<Transform>().position = currentTileCoord*1.1f;

		} 
		else {

		}
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Click");
		}
	}
}