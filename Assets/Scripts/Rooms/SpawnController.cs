using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Rooms
{
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] public AnimationCurve spawnCurve;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int enemiesToSpawnInRoom;
        [SerializeField] private int enemiesInRoomAtStart;
        [SerializeField] private float spawnRate = 2.0f;

        public UnityEvent spawnedEnemy = new UnityEvent();
        public UnityEvent<int> roomStarted = new UnityEvent<int>();
        
        private SpawnPoint[] _startPoints;
        private SpawnPoint[] _validDoors;
        private int _spawnCount = 0;
        
        private WaitForSeconds _coroutineWait;

        private void OnDestroy()
        {
            spawnedEnemy.RemoveAllListeners();
            roomStarted.RemoveAllListeners();
        }

        private void Awake()
        {
            SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.InstanceID);
            
            Debug.Assert(spawnPoints.Length > 0, "No SpawnPoints found in room prefab.");

            if (spawnPoints.Length == 0)
            {
                enabled = false;
                return;
            }
            
            if (spawnPoints.Length < enemiesInRoomAtStart) enemiesInRoomAtStart = spawnPoints.Length;
            
            /*
             * This uses LINQ methods to find the spawn points flagged as starting spawns
             * sorted by their unique ID and then only use the number of spawns set in enemiesInRoomAtStart
             * https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/
             */
            _startPoints = spawnPoints.Where(point => point.IsStartingSpawn)
                .OrderByDescending(point => point.ID)
                .Take(enemiesInRoomAtStart)
                .ToArray();
            
            // find all the door spawn points to use during play
            // first find all the spawn points not flagged as "starting spawns"
            // from that list, find all doors that are not the door the PC entered from.
            _validDoors = spawnPoints.Where(point => point.IsStartingSpawn == false)
                .Where(point => point.GetComponent<RoomDoor>().IsSpawnDoor == false)
                .ToArray();

            // just in case we have no doors (for some reason?!) - we'll still spawn monsters
            if (_validDoors.Length == 0)
                _validDoors = _startPoints;
            
            // adjust the curve keys to adjust for the maximum number of enemies to spawn
            spawnCurve.MoveKey(2, new Keyframe(enemiesToSpawnInRoom, 0.0f));
            spawnCurve.MoveKey(1, new Keyframe(enemiesToSpawnInRoom / 2.0f, 3.0f));
        }
        
        private void Start()
        {
            // we could do this same setup in the Editor rather than via code.
            GameController gameController = GameObject.FindFirstObjectByType<GameController>();

            if (gameController)
            {
                roomStarted.AddListener(gameController.OnRoomStart);
                spawnedEnemy.AddListener(gameController.OnEnemySpawned);
            }

            if (enemyPrefab == null)
            {
                Debug.LogError("no prefab set in SpawnController");
                return;
            }

            foreach (var spawn in _startPoints)
            {
                spawn.SpawnObject(enemyPrefab);
                spawnedEnemy.Invoke();
            }
            
            if(_startPoints.Length > 0)
                _spawnCount = enemiesInRoomAtStart;
            
            _coroutineWait = new WaitForSeconds(spawnRate);
            
            roomStarted.Invoke(enemiesToSpawnInRoom);
        }

        public void StartSpawning()
        {
            Debug.Log("Starting SpawnEnemies coroutine");
            StartCoroutine(SpawnEnemies());
        }
        
        private void OnDisable()
        {
            StopCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                yield return _coroutineWait;

                if (_spawnCount >= enemiesToSpawnInRoom) break;

                // the animation curve gives us a float value based on the "time" we pass
                // in to evaluate; here our time is actually how many enemies have already spawned.
                // The number we get back is how many enemies should spawn.
                int numberToSpawn = (int)Mathf.Ceil(spawnCurve.Evaluate(_spawnCount));
                int startDoor = Random.Range(0, _validDoors.Length);

                for (int i = 0; i < numberToSpawn; i++)
                {
                    SpawnPoint door = _validDoors[startDoor];
                    door.SpawnObject(enemyPrefab);
                    spawnedEnemy.Invoke();
                    _spawnCount++;
                    startDoor++;
                    if (startDoor == _validDoors.Length)
                        startDoor = 0;
                }
                
            }
        }
    }
}
