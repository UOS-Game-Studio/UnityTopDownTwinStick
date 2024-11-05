using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rooms
{
    public class SpawnPoint : MonoBehaviour
    {
        public Mesh gizmoMesh;
        [SerializeField] private bool startingSpawn = true;
        private Vector3 _spawnPosition;
        private Quaternion _spawnRotation;
        private const float YOffset = 0.5f;
        
        // to allow these to be sorted, we get a GUID (Globally Unique ID) for each spawn point component
        // because we cannot guarantee the order of Awake or Start, we have to initialise it here.
        // Formally, we are using a property here to ensure the ID cannot be changed once initialised
        // we can't use const as Guid.NewGuid does not return a value at compile time.
        public Guid ID { get; } = Guid.NewGuid();
        public bool IsStartingSpawn => startingSpawn;

        private void Awake()
        {
            _spawnPosition = transform.position;
            _spawnRotation = transform.rotation;
        }

        /*
         * Allow for the spawn position and rotation to be overridden
         * Used by RoomDoor to shift the spawn point away from the door origin
         */
        public void SetSpawnPositionAndRotation(Vector3 pos, Quaternion rot)
        {
            _spawnPosition = pos;
            _spawnRotation = rot;
        }
        
        /*
         * SpawnPoints do not have a physical representation, which is what we want in the game
         * in the editor, we want to see where they are and interact with them, so we draw a mesh gizmo
         * to give us that representation.
         */
        private void OnDrawGizmos()
        {
            if (gizmoMesh == null) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireMesh(gizmoMesh, transform.position, transform.rotation);
        }

        public void SpawnObject(GameObject prefab)
        {
            Vector3 spawnPosition =
                new Vector3(_spawnPosition.x, _spawnPosition.y + YOffset, _spawnPosition.z);
            Instantiate(prefab, spawnPosition, _spawnRotation);
        }
    }
}