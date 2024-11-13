using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// GameController tracks our progress through the current room.
/// It has a set of events that are invoked at specific points:
/// Events:
///     <c>onRoomComplete</c> - when all enemies are dead
///     <c>onGameOver</c> - on PC death
///     <c>onRoomBegin</c> - when a room has instantiated and is ready to spawn enemies
///     <c>onRoomExit</c> - when the PC hits a door trigger
/// </summary>
public class GameController : MonoBehaviour
{
    private int _spawnedEnemies;
    private int _maxEnemies;
    private int _killedEnemies;

    [SerializeField] private float roomStartWaitTime = 0.5f;
    private WaitForSeconds _startWaitTime;
    
    public UnityEvent onRoomComplete = new();
    public UnityEvent onGameOver = new();
    public UnityEvent onRoomBegin = new();
    public UnityEvent onRoomExit = new();

    private void Start()
    {
        _startWaitTime = new WaitForSeconds(roomStartWaitTime);
    }

    private IEnumerator StartRoom()
    {
        yield return _startWaitTime;
        onRoomBegin.Invoke();
    }
    
    // handler for the SpawnController.roomStarted event
    public void OnRoomStart(int maxEnemies)
    {
        _killedEnemies = 0;
        _maxEnemies = maxEnemies;
        StartCoroutine(StartRoom());
    }
    
    // handler for SpawnController.spawnedEnemy
    public void OnEnemySpawned()
    {
        _spawnedEnemies++;
    }

    // handler for RoomDoor.onExitTriggered
    public void ExitDoorTriggered()
    {
        onRoomExit.Invoke();
    }
    
    // handler for Health.onDeath
    public void OnCharacterKilled(Common.Health charHealth)
    {
        // it's an enemy if the associated gameobject has an enemycore component
        bool isEnemy = charHealth.GetComponent<AI.EnemyCore>() != null;
        bool isPlayer = !isEnemy; // if not, it must be a player (assumptions!)

        if (isEnemy)
        {
            _killedEnemies++;

            // due to the spawn curve, we sometimes get an extra enemy or two, so we have to account for that here!
            if (_killedEnemies >= _maxEnemies && _killedEnemies >= _spawnedEnemies)
            {
                _spawnedEnemies = 0;
                onRoomComplete.Invoke();
                return;
            }
        }

        if (isPlayer)
        {
            onGameOver.Invoke();
        }
    }
}
