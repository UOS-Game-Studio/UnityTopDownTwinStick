using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rooms
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Mesh gizmoMesh;
        private const float YOffset = 0.5f;

        public Guid ID { get; } = Guid.NewGuid();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireMesh(gizmoMesh, transform.position, transform.rotation);
        }

        public void SpawnObject(GameObject prefab)
        {
            Vector3 spawnPosition =
                new Vector3(transform.position.x, transform.position.y + YOffset, transform.position.z);
            Instantiate(prefab, spawnPosition, transform.rotation);
        }
    }
}