using System;
using UnityEngine;
using UnityEngine.Events;

namespace Rooms
{
    public enum DoorDirection
    {
        North,
        South,
        East,
        West
    }
    
    /// <summary>
    /// RoomDoor is the logical representation of the doors that exist in each room.
    /// they are responsible for spawning enemies via an associated <c>SpawnPoint</c> when told to by <c>SpawnController</c>
    /// Events:
    ///     <c>_onExitTriggered</c> - invoked when the player enters the door trigger volume when the door is unlocked.
    /// </summary>
    public class RoomDoor : MonoBehaviour
    {
        public DoorDirection direction;
        public Mesh lockedMesh;
        public Mesh openMesh;

        public GameObject closedDoor;
        public GameObject openDoor;
        
        // public properties such as these are not serialised by the Inspector, as they are not variables in and of themselves
        // as they can have logic associated with them.
        // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/using-properties
        // another example in a different format is seen in WeaponProjectilePool for the Pool property.
        // to see these values in the inspector, we either add the [SerializeField] attribute to the associated variables
        // or we make the variables public and remove these properties.
        public bool IsSpawnDoor => _isSpawnDoor;
        public bool IsExitDoor => _isExitDoor;

        private bool _isLocked = true;
        private bool _isSpawnDoor;
        private bool _isExitDoor;
        
        private SpawnPoint _spawnComponent;
        private Transform _spawnTransform;
        private GameController _gameController;
        private UnityEvent _onExitTriggered = new UnityEvent();

        private void Awake()
        {
            _spawnComponent = GetComponent<SpawnPoint>();
            Debug.Assert(_spawnComponent != null);
            
            // enforce correct active status for open / closed children.
            openDoor.SetActive(false);
            closedDoor.SetActive(true);
            
            // transform.Find searches within the transforms hierarchy, not across the entire scene.
            // https://docs.unity3d.com/ScriptReference/Transform.Find.html
            _spawnTransform = transform.Find("SpawnPoint");
            _spawnComponent.SetSpawnPositionAndRotation(_spawnTransform.position, _spawnTransform.rotation);
        }

        private void Start()
        {
            _gameController = GameObject.FindFirstObjectByType<GameController>();
            _gameController.onRoomComplete.AddListener(Unlock);
            _onExitTriggered.AddListener(_gameController.ExitDoorTriggered);
        }

        private void OnDestroy()
        {
            _gameController.onRoomComplete.RemoveListener(Unlock);
            openDoor.SetActive(true);
            closedDoor.SetActive(false);
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
            return _spawnTransform;
        }
        
        public void MakeSpawnDoor()
        {
            _isSpawnDoor = true;
            
            // spawn doors remain closed as the player cannot go "back" on themselves
            // so we show a "locked mesh".
            openDoor.SetActive(false);
            closedDoor.SetActive(true);
        }
        
        private void Unlock()
        {
            if (_isSpawnDoor) return;
            
            _isLocked = false;
            openDoor.SetActive(true);
            closedDoor.SetActive(false);
        }
    }

}