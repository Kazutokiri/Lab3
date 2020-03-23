using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {

	public int xSize, ySize;

	private Mesh mesh;
	private Vector3[] vertices;

	private void Awake () {
		StartCoroutine(Generate());
	}

	//private void Generate () { 
    private IEnumerator Generate () {
        
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Grid";
        GetComponent<MeshCollider>().sharedMesh = mesh;
       
		vertices = new Vector3[(xSize + 1) * (ySize + 1)];
		Vector2[] uv = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        for (int i = 0, y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++, i++) {
                //x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
                //y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;
                float percentage = ((float)x / (float)xSize);
                float halfpipeX = Mathf.Cos(Mathf.Deg2Rad * (percentage * 180f)) * 5;
                float halfpipeY = Mathf.Sin(Mathf.Deg2Rad * (percentage * 180f)) * 5;
                vertices[i] = new Vector3(halfpipeX, -halfpipeY, (-ySize/2) + y);
                //values of x's but turns them to floats
				uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
				tangents[i] = tangent;
                yield return wait;
            }
		}
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.tangents = tangents;


        //dont directly put verticies into the mesh
		int[] triangles = new int[xSize * ySize * 6];
		for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
			for (int x = 0; x < xSize; x++, ti += 6, vi++) {
                /*triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
                */
                //revise triangles to draw with reverse normals.
                triangles[ti] = vi;
				triangles[ti + 4] = triangles[ti + 1] = vi + 1;
				triangles[ti + 3] = triangles[ti + 2] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
                
                mesh.triangles = triangles;
                yield return wait;
            }
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
        MeshCollider old_m = GetComponent<MeshCollider>();
        Destroy(old_m);
        MeshCollider m = gameObject.AddComponent<MeshCollider>();
        m.sharedMesh = mesh;
	}

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}