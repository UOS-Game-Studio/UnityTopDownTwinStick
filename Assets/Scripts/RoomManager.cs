using Rooms;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// RoomManager is responsible for instantiating and destroying instances of the room prefabs
/// it hooks into events from GameController to know when to trigger the swap
/// when a new room is instantiated, we need to know which door to flag as the entry point
/// </summary>
public class RoomManager : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public Transform playerTransform;
    private GameObject _currentRoom; // temporary serialisation for testing joys.

    private SpawnController _spawnController;

    private DoorDirection _exitDoor;
    
    private void Start()
    {
        GameController controller = FindAnyObjectByType<GameController>();
        Debug.Assert(controller, "No GameController found in RoomManager Start");
        
        controller.onRoomExit.AddListener(DoRoomSpawn);
        controller.onRoomBegin.AddListener(StartSpawnController);
        
        // trigger an initial room to spawn in at the start of gameplay!
        if (!_currentRoom)
        {
            DoRoomSpawn();
        }
    }

    private void StartSpawnController()
    {
        _spawnController.StartSpawning();
    }
    
    private void DoRoomSpawn()
    {
        if (roomPrefabs.Length == 0) return;

        // by setting the default exit position to the north, we'll always start in the south
        // if nothing else changes it.
        _exitDoor = DoorDirection.North;
        DoorController doorController;
        
        // we have an existing room, so we need to deal with that.
        if (_currentRoom)
        {
            doorController = _currentRoom.GetComponent<DoorController>();
            _exitDoor = doorController.GetExitDirection();
            Destroy(_currentRoom);
        }
        
        // pick a random prefab from the array of prefabs
        GameObject prefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];

        // instantiate it
        _currentRoom = Instantiate(prefab, Vector3.zero, quaternion.identity);
        
        doorController = _currentRoom.GetComponent<DoorController>();
        Transform doorTransform = doorController.SetEntryPoint(_exitDoor);
        
        // move the player character to the correct door location
        playerTransform.SetPositionAndRotation(doorTransform.position, doorTransform.rotation);

        // hook the new room into the game controller.
        _spawnController = _currentRoom.GetComponent<SpawnController>();
        
        _spawnController.StartRoom();
    }
}
