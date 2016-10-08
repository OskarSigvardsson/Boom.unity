using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using csDelaunay;

namespace GK {
	public class BreakableWall : MonoBehaviour {

		public int NumberOfPoints = 5;
		public GameObject ShardPrefab;
		
		void Start() {
			var scale = transform.localScale;
			var left = -scale.x/2.0f;
			var right = scale.x/2.0f;
			var down = -scale.y/2.0f;
			var up = scale.y/2.0f;

			var bounds = new Rectf(left, down, right - left, up - down);
			//var bounds = new Rectf(-0.5f, -0.5f, 1.0f, 1.0f);

			var sites = new List<Vector2f>();

			//sites.Add(new Vector2f(0.0f, 0.25f));
			//sites.Add(new Vector2f(-0.25f, -0.25f));
			//sites.Add(new Vector2f(0.25f, -0.25f));


			for(int i = 0; i < NumberOfPoints; i++) {
				var x = Random.Range(left, right);
				var y = Random.Range(down, up);
				var point = new Vector2f(x, y);

				sites.Add(point);
			}

			var voronoi = new Voronoi(sites, bounds, 1);
			var regions = voronoi.Regions();

			foreach(var region in regions) {
				var len = region.Count;

				var mesh = new Mesh();


				var trisCount = 12 * (len - 1);
				//var trisCount = 6 * (len - 1);

				var tris = new int[trisCount];

				var verts = new Vector3[3 * len];
				var uvs = new Vector2[3 * len];

				for(int v = 0; v < len; v++) {
					var coord = region[v];
					verts[v] = new Vector3(coord.x, coord.y, scale.z/2.0f);
					verts[len + v] = new Vector3(coord.x, coord.y, -scale.z/2.0f);
					verts[2 * len + v] = new Vector3(coord.x, coord.y, -scale.z/2.0f);

					uvs[v] = new Vector2(-1, -1);
					uvs[2 * len + v] = new Vector2(-1, -1);
					uvs[len + v] = new Vector2(coord.x, coord.y);
				}

				var t = 0;
				for(int v = 1; v < len - 1; v++) {
					//tris[t++] = 0; 
					//tris[t++] = v; 
					//tris[t++] = v + 1; 

					tris[t++] = len + v + 1;
					tris[t++] = len + v;
					tris[t++] = len;

					tris[t++] = 2 * len;
					tris[t++] = 2 * len + v;
					tris[t++] = 2 * len + v + 1;
				}

				for(int v = 0; v < len; v++) {
					var n = v == (len-1) ? 0 : v+1;

					tris[t++] = v;
					tris[t++] = len + v;
					tris[t++] = len + n;

					tris[t++] = v;
					tris[t++] = len + n;
					tris[t++] = n;
				}

				mesh.vertices = verts;
				mesh.triangles = tris;
				mesh.uv = uvs;
				
				//mesh.RecalculateNormals();

				var shard = Instantiate(ShardPrefab);

				shard.transform.SetParent(transform, false);

				shard.transform.localPosition = Vector3.zero;
				shard.transform.localRotation = Quaternion.identity;
				shard.transform.localScale = Vector3.one;

				shard.GetComponent<MeshCollider>().sharedMesh = mesh;

				shard.GetComponentInChildren<MeshFilter>().sharedMesh = mesh;
				//shard.GetComponentInChildren<MeshRenderer>().sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;

				transform.localScale = Vector3.one;

				GetComponent<MeshRenderer>().enabled = false;
			}

		}

	}
}
