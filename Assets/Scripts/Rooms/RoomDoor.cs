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
        private Transform _spawnTransform;
        public DoorDirection doorDirection;
        private bool _spawnDoor;
        public Mesh lockedMesh;
        public Mesh openMesh;
        
        private SpawnPoint _spawnComponent;
        private bool _isLocked = true;
        
        private MeshFilter _meshFilter;
        public bool IsSpawnDoor => _spawnDoor;

        private void Start()
        {
            _spawnComponent = GetComponent<SpawnPoint>();
            _meshFilter = GetComponent<MeshFilter>();
            Debug.Assert(_spawnComponent != null);
            Debug.Assert(_meshFilter != null);

            _spawnTransform = transform.Find("SpawnPoint");
            
            _spawnComponent.SetSpawnPositionAndRotation(_spawnTransform.position, _spawnTransform.rotation);

            if(_spawnDoor) MakeSpawnDoor();
            
            GameController gameController = GameObject.FindFirstObjectByType<GameController>();
            gameController.onRoomComplete.AddListener(Unlock);
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
            _spawnDoor = true;
            
            // spawn doors remain closed as the player cannot go "back" on themselves
            // so we show a "locked mesh".
            _meshFilter.mesh = lockedMesh;
        }
        
        private void Unlock()
        {
            if (_spawnDoor) return;
            
            _isLocked = false;
            _meshFilter.mesh = openMesh;
        }
    }

}