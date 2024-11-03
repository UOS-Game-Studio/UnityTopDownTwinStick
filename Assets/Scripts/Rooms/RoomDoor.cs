using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rooms
{
    public enum DoorDirection
    {
        North,
        South,
        East,
        West
    }
    public class RoomDoor : MonoBehaviour
    {
        private SpawnPoint _spawnComponent;
        [SerializeField] private Transform spawnTransform;
        [SerializeField] private DoorDirection doorDirection;
        private bool _isLocked = true;
        [SerializeField] private bool spawnDoor = false;

        [SerializeField] private Mesh lockedMesh;
        [SerializeField] private Mesh openMesh;
        //[SerializeField] private Mesh closedMesh;
        
        private MeshFilter _meshFilter;
        public bool IsSpawnDoor => spawnDoor;

        private void Start()
        {
            _spawnComponent = GetComponent<SpawnPoint>();
            _meshFilter = GetComponent<MeshFilter>();
            Debug.Assert(_spawnComponent != null);
            Debug.Assert(_meshFilter != null);
            
            _spawnComponent.SetSpawnPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_isLocked) return;

            if (other.CompareTag("Player"))
            {
                Debug.Log("Transition outta here");
            }
        }

        public void MakeSpawnDoor()
        {
            spawnDoor = true;
            
            // spawn doors remain closed as the player cannot go "back" on themselves
            // so we show a "locked mesh".
            _meshFilter.mesh = lockedMesh;
        }
        
        public void Unlock()
        {
            _isLocked = false;

            _meshFilter.mesh = openMesh;
        }
    }

}