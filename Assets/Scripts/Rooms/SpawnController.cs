using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rooms
{
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        private SpawnPoint[] _startPoints;
        private SpawnPoint[] _validDoors;
        private SpawnPoint[] _spawnPoints;
        private int _spawnCount = 0;
        public AnimationCurve spawnCurve;
        [SerializeField] private int enemiesToSpawnInRoom;
        [SerializeField] private int enemiesInRoomAtStart;
        [SerializeField] private float spawnRate = 2.0f;
        private WaitForSeconds _coroutineWait;
        
        
        private void Awake()
        {
            _spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.InstanceID);
            
            Debug.Assert(_spawnPoints.Length > 0, "No SpawnPoints found in room prefab.");
            
            if (_spawnPoints.Length < enemiesInRoomAtStart) enemiesInRoomAtStart = _spawnPoints.Length;
            
            /*
             * This uses LINQ methods to find the spawn points flagged as starting spawns
             * sorted by their unique ID and then only use the number of spawns set in enemiesInRoomAtStart
             * https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/
             */
            _startPoints = _spawnPoints.Where(point => point.IsStartingSpawn)
                .OrderByDescending(point => point.ID)
                .Take(enemiesInRoomAtStart)
                .ToArray();
            
            // find all the door spawn points to use during play
            _validDoors = _spawnPoints.Where(point => point.IsStartingSpawn == false)
                .Where(point => point.GetComponent<RoomDoor>().IsSpawnDoor == false)
                .ToArray();

            // adjust the curve keys to adjust for the maximum number of enemies to spawn
            spawnCurve.MoveKey(2, new Keyframe(enemiesToSpawnInRoom, 0.0f));
            spawnCurve.MoveKey(1, new Keyframe(enemiesToSpawnInRoom / 2.0f, 3.0f));
        }
        
        private void Start()
        {
            if(enemyPrefab == null) return;
            
            foreach (var spawn in _startPoints)
            {
                spawn.SpawnObject(enemyPrefab);
            }
            
            if(_startPoints.Length > 0)
                _spawnCount = enemiesInRoomAtStart;
            
            _coroutineWait = new WaitForSeconds(spawnRate);
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
                // in to evaluate; here our time is actually how many enemies have already spawned
                // the number we get back is how many enemies we should spawn.
                int numberToSpawn = (int)Mathf.Ceil(spawnCurve.Evaluate(_spawnCount));
                Debug.Log("number of enemies to spawn: " + numberToSpawn);
                int startDoor = Random.Range(0, _validDoors.Length);

                for (int i = 0; i < numberToSpawn; i++)
                {
                    SpawnPoint door = _validDoors[startDoor];
                    door.SpawnObject(enemyPrefab);
                    _spawnCount++;
                    startDoor++;
                    if (startDoor == _validDoors.Length)
                        startDoor = 0;
                }
                
            }
        }
    }
}
