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
        [SerializeField] private SpawnPoint[] spawnPoints;
        private int _spawnCount = 0;
        public AnimationCurve spawnCurve;
        [SerializeField] private int enemiesToSpawnInRoom;
        [SerializeField] private int enemiesInRoomAtStart;
        [SerializeField] private float spawnRate = 0.7f;
        private WaitForSeconds _coroutineWait;
        
        private void Awake()
        {
            spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.InstanceID);
            
            Debug.Assert(spawnPoints.Length > 0, "No SpawnPoints found in room prefab.");
            
            if (spawnPoints.Length < enemiesInRoomAtStart) enemiesInRoomAtStart = spawnPoints.Length;
        }
        
        private void Start()
        {
            
            
            _coroutineWait = new WaitForSeconds(spawnRate);
            StartCoroutine(SpawnEnemies());
        }

        private void OnEnable()
        {
            if(enemyPrefab == null) return;
            
            // select from the total spawn points and instantiate beasties.
            SpawnPoint[] toSpawn = spawnPoints.OrderByDescending(point => point.ID)
                                                .Take(enemiesInRoomAtStart)
                                                .ToArray();

            Debug.Log("toSpawn size:" + toSpawn.Length);
            
            foreach (var spawn in toSpawn)
            {
                spawn.SpawnObject(enemyPrefab);
            }
            
            _spawnCount = enemiesInRoomAtStart;
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
                
                
            }
        }
    }
}
