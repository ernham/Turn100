using UnityEngine;
using System.Collections;

[ExecuteInEditMode] //실시간으로 코드 반영
[RequireComponent(typeof(MeshFilter))] //생성된 오브젝트에 메시필터 콤포넌트 적용
[RequireComponent(typeof(MeshRenderer))] //생성된 오브젝트에 메시렌더러 콤포넌트 적용
[RequireComponent(typeof(MeshCollider))] //생성된 오브젝트에 메시콜라이더 콤포넌트 적용

public class Dmanager : MonoBehaviour {
	// 전체 맵의 가로 세로 크기를 담는 변수
	public int size_x = 50;
	public int size_z = 25;
	public float tileSize = 1.0f;

	public Texture2D terrainTiles;
	public int tileResolution;

	void Start () {
	// 빌드메시 함수를 실행
		BuildMesh ();
	}

	Color[][] ChopUpTiles() {
		int numTilesPerRow = terrainTiles.width / tileResolution;
		int numRows = terrainTiles.height / tileResolution;

		Color[][] tiles = new Color[numTilesPerRow * numRows][];

		for(int y=0; y<numRows; y++){
			for(int x=0; x<numTilesPerRow; x++){
				tiles[y*numTilesPerRow + x] = terrainTiles.GetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution );
			}
		}
		return tiles;
	}
				    

	void BuildTexture () {


		int texWidth = size_x * tileResolution;
		int texHeight = size_z * tileResolution;
		Texture2D texture = new Texture2D (texWidth, texHeight);

		Color[][] tiles = ChopUpTiles ();

		for (int y=0; y < size_z; y++) {
			for (int x=0; x < size_x; x++) {
				Color[] p = tiles[ Random.Range (1,4)];
				texture.SetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
			}
		}
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply ();

		MeshRenderer mesh_renderer = GetComponent<MeshRenderer> ();
		mesh_renderer.sharedMaterials [0].mainTexture = texture;

		Debug.Log ("텍스쳐 생성!");


	}


	// 빌드메시 함수 코드
	public void BuildMesh () {

		int numTiles = size_x * size_z; // 전체 타일의 수량 
		int numTris = numTiles * 2; //전체 폴리곤의 수량 1타일당 2개의 폴리곤으로 구성
		int vsize_x = size_x + 1; //가로 버텍스의 수량 저장 기본 2개에서 타일 1개당 1개씩 추가,인접해 있으므로
		int vsize_z = size_z + 1; //세로 버텍스의 수량 저장
		int numVerts = vsize_x * vsize_z; //총 버텍스의 수량

		// 메시 데이터 생성
		Vector3[] vertices = new Vector3[numVerts]; // Vector3 타입의 vertices 변수에 총 버텍스 수량을 저장
		Vector3[] normals = new Vector3[numVerts]; // Vector3 타입의 normals 변수에 총 버텍스 수량을 저장
		Vector2[] uv = new Vector2[numVerts]; // Vector2 타입의 normals 변수에 총 버텍스 수량을 저장

		int[] triangles = new int[ numTris * 3]; // int 타입의 변수에 총 폴리곤*3 만큼의 값을 저

		//타일배치 
		int x, z;

		for (z=0; z <vsize_z; z++) {
			for (x=0; x <vsize_x; x++) {
				vertices[ z * vsize_x + x] = new Vector3( x*tileSize, 0, z*tileSize);
				normals[ z * vsize_x + x] = Vector3.up;
				uv[ z * vsize_x + x] = new Vector2((float)x / vsize_x, (float)z / vsize_z);

				// x=0, uv.x = 0
				// x=101, uv.x = 1
				// uv.x = (float)x / vsize_x

			}
		}

		Debug.Log ("버텍스 생성!!");

		for (z=0; z <size_z; z++) {
			for (x=0; x <size_x; x++) {
				int squareIndex = z * size_x + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = z * vsize_x + x + 0;
				triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
				triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;


				triangles[triOffset + 3] = z * vsize_x + x + 0;
				triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
				triangles[triOffset + 5] = z * vsize_x + x + 1;

			}
		}

		Debug.Log ("폴리곤 생성!!");

		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		MeshFilter mesh_filter = GetComponent<MeshFilter> ();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer> ();
		MeshCollider mesh_collider = GetComponent<MeshCollider> ();


		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		Debug.Log ("메시 생성");

		BuildTexture ();
	
	}
}
