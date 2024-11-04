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
        [SerializeField] private Transform spawnTransform;
        [SerializeField] private DoorDirection doorDirection;
        [SerializeField] private bool spawnDoor = false;
        [SerializeField] private Mesh lockedMesh;
        [SerializeField] private Mesh openMesh;
        
        private SpawnPoint _spawnComponent;
        private bool _isLocked = true;

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

            if(spawnDoor) MakeSpawnDoor();
            
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
            spawnDoor = true;
            
            // spawn doors remain closed as the player cannot go "back" on themselves
            // so we show a "locked mesh".
            _meshFilter.mesh = lockedMesh;
        }
        
        public void Unlock()
        {
            if (spawnDoor) return;
            
            _isLocked = false;
            _meshFilter.mesh = openMesh;
        }
    }

}