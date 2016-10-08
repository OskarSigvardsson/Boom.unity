using UnityEngine;
using System.Collections;

namespace GK {
	public class Shoot : MonoBehaviour {

		public LayerMask HitLayers;
		public LayerMask ShardLayers;
		public float ExplosionRadius;
		public float ExplosionForce;

		void Update() {
			if(Input.GetMouseButton(0)) {
				var cam = GetComponentInChildren<Camera>();

				RaycastHit hit;
				var ray = new Ray(cam.transform.position, cam.transform.forward);

				if(Physics.Raycast(ray, out hit, 100.0f, layerMask: HitLayers)) {
					Explode(hit.point);
				}
			}
		}

		void Explode(Vector3 position) {
			foreach(var shard in Physics.OverlapSphere(position, ExplosionRadius, ShardLayers)) {
				var rb = shard.GetComponent<Rigidbody>();

				rb.isKinematic = false;
				rb.AddExplosionForce(ExplosionForce, position, ExplosionRadius);

				shard.gameObject.AddComponent<DestroyIn>().Time = 3.0f;
			}
		}
	}
}
