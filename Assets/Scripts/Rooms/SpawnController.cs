using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Rooms
{
    /// <summary>
    /// SpawnController is attached to a room prefab, it owns how many enemies should spawn (really, the minimum number)
    /// and it maintains arrays of spawn points, both "start points" and "doors"
    /// Events: 
    ///   <c>spawnedEnemy</c> - invoked when any enemy is spawned.
    ///   <c>roomStarted</c> - invoked after spawning the initial enemies (as defined by enemiesInRoomAtStart) passing <c>enemiesToSpawnInRoom</c>.
    /// </summary>
    public class SpawnController : MonoBehaviour
    {
        // AnimationCurves are sort of fancy, they're so named because they are mostly used for animation purposes
        // but we can use them for all sorts of things that we want to base on a curve, XP or levelling up for instance
        // we just treat the "time" value differently.
        // https://docs.unity3d.com/ScriptReference/AnimationCurve.html
        public AnimationCurve spawnCurve;
        public GameObject enemyPrefab;
        public int enemiesToSpawnInRoom; //Review: rename to minimumEnemiesToSpawn?
        public int enemiesInRoomAtStart;
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
            StopCoroutine(SpawnEnemies());
        }

        private void Awake()
        {
            // any call to the "Find" functions is quite expensive, so we only want to do them in functions that don't happen
            // regularly; Start and Awake are ideal.
            // as this script is attached to the base GameObject of Room prefabs, we could do this in the editor by hand instead.
            SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.InstanceID);
            
            Debug.Assert(spawnPoints.Length > 0, "No SpawnPoints found in room prefab.");

            if (spawnPoints.Length == 0)
            {
                enabled = false;
                return;
            }
            
            // if there are fewer spawn points in the room than the number of enemies we want present
            // at the start of the room, we need to cap the value of enemiesInRoomAtStart
            if (spawnPoints.Length < enemiesInRoomAtStart) enemiesInRoomAtStart = spawnPoints.Length;
            
            /*
             * This uses LINQ methods to find the spawn points flagged as starting spawns
             * sorted by their unique ID and then only use the number of spawns set in enemiesInRoomAtStart
             * each spawn assigns itself a random ID, so we get a shuffle effect doing this.
             * https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/
             */
            _startPoints = spawnPoints.Where(point => point.IsStartingSpawn)
                .OrderByDescending(point => point.ID)
                .Take(enemiesInRoomAtStart)
                .ToArray();
            
            // find all the door spawn points to use during play
            // first find all the spawn points not flagged as "starting spawns"
            // from that list, find all doors that are not the door the PC entered from.
            // this is making the assumption that a spawn point with a false starting spawn is attached to a door!
            _validDoors = spawnPoints.Where(point => point.IsStartingSpawn == false)
                .Where(point => point.GetComponent<RoomDoor>().IsSpawnDoor == false)
                .ToArray();

            // just in case we have no doors (for some reason?!) - we'll still spawn monsters
            if (_validDoors.Length == 0)
                _validDoors = _startPoints;
            
            const float maxDoors = 3.0f;
            // adjust the curve keys to adjust for the maximum number of enemies to spawn
            // this enforces a max number of 3 at the top end, as that's how many doors aren't "locked"
            spawnCurve.MoveKey(2, new Keyframe(enemiesToSpawnInRoom, 0.0f));
            spawnCurve.MoveKey(1, new Keyframe(enemiesToSpawnInRoom / 2.0f, maxDoors));
        }

        private void Start()
        {
            _coroutineWait = new WaitForSeconds(spawnRate);
        }
        
        public void StartRoom()
        {
            // because Rooms are instantiated from prefabs, we need to do this in code.
            GameController gameController = GameObject.FindFirstObjectByType<GameController>();
            
            Debug.Assert(gameController, "No GameController instance found in the scene.");
            
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
                spawn.SpawnObject(enemyPrefab, transform);
                spawnedEnemy.Invoke();
            }
            
            if(_startPoints.Length > 0)
                _spawnCount = enemiesInRoomAtStart;

            roomStarted.Invoke(enemiesToSpawnInRoom);
        }

        public void StartSpawning()
        {
            StartCoroutine(SpawnEnemies());
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
                // we use Ceil to lock the returned value to a whole integer.
                int numberToSpawn = (int)Mathf.Ceil(spawnCurve.Evaluate(_spawnCount));
                
                // We spawn at doors in a round robin fashion, here we pick a random door to start at.
                int startDoor = Random.Range(0, _validDoors.Length);

                for (int i = 0; i < numberToSpawn; i++)
                {
                    SpawnPoint door = _validDoors[startDoor];
                    door.SpawnObject(enemyPrefab, transform);
                    spawnedEnemy.Invoke();
                    _spawnCount++;
                    
                    // step to the next door, if we've hit the end of the array, we go back to 0.
                    startDoor++;
                    if (startDoor == _validDoors.Length)
                        startDoor = 0;
                }
                
            }
        }
    }
}
