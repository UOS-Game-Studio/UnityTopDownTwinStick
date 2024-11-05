using System;
using UnityEngine;
using UnityEngine.Events;
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
        public DoorDirection direction;
        private bool _isSpawnDoor;
        public Mesh lockedMesh;
        public Mesh openMesh;
        
        private SpawnPoint _spawnComponent;
        private bool _isLocked = true;

        private bool _isExitDoor;
        
        private MeshFilter _meshFilter;
        public bool IsSpawnDoor => _isSpawnDoor;
        public bool IsExitDoor => _isExitDoor;
        
        private GameController _gameController;

        private UnityEvent _onExitTriggered = new UnityEvent();

        private void Awake()
        {
            _spawnComponent = GetComponent<SpawnPoint>();
            _meshFilter = GetComponent<MeshFilter>();
            Debug.Assert(_spawnComponent != null);
            Debug.Assert(_meshFilter != null);
            
            _spawnTransform = transform.Find("SpawnPoint");
            
            _spawnComponent.SetSpawnPositionAndRotation(_spawnTransform.position, _spawnTransform.rotation);
        }

        private void Start()
        {
            if(_isSpawnDoor) MakeSpawnDoor();
            
            _gameController = GameObject.FindFirstObjectByType<GameController>();
            _gameController.onRoomComplete.AddListener(Unlock);
            _onExitTriggered.AddListener(_gameController.ExitDoorTriggered);
        }

        private void OnDestroy()
        {
            _gameController.onRoomComplete.RemoveListener(Unlock);
            _onExitTriggered.RemoveAllListeners();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isLocked) return;

            if (other.CompareTag("Player"))
            {
                _isExitDoor = true;
                _onExitTriggered.Invoke();
            }
        }

        public Transform GetSpawnTransform()
        {
            Debug.Log(name + " spawnTransform position: " + _spawnTransform.position);
            return _spawnTransform;
        }
        
        public void MakeSpawnDoor()
        {
            Debug.Log("Setting Spawn Door as " + name);
            _isSpawnDoor = true;
            
            // spawn doors remain closed as the player cannot go "back" on themselves
            // so we show a "locked mesh".
            _meshFilter.mesh = lockedMesh;
        }
        
        private void Unlock()
        {
            if (_isSpawnDoor) return;
            
            _isLocked = false;
            _meshFilter.mesh = openMesh;
        }
    }

}