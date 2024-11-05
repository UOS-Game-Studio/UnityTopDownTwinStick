using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private int _spawnedEnemies;
    private int _maxEnemies;
    private int _killedEnemies;

    [SerializeField] private float roomStartWaitTime = 0.5f;
    private WaitForSeconds _startWaitTime;
    
    public UnityEvent onRoomComplete = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();
    public UnityEvent onRoomBegin = new UnityEvent();
    public UnityEvent onRoomExit = new UnityEvent();
    
    
    private void Start()
    {
        _startWaitTime = new WaitForSeconds(roomStartWaitTime);
    }

    private IEnumerator StartRoom()
    {
        yield return _startWaitTime;
        onRoomBegin.Invoke();
    }
    
    public void OnRoomStart(int maxEnemies)
    {
        _killedEnemies = 0;
        _maxEnemies = maxEnemies;
        StartCoroutine(StartRoom());
    }
    
    public void OnEnemySpawned()
    {
        _spawnedEnemies++;
    }

    public void ExitDoorTriggered()
    {
        onRoomExit.Invoke();
    }
    
    public void OnCharacterKilled(Common.Health charHealth)
    {
        bool isEnemy = charHealth.GetComponent<AI.EnemyCore>() != null;
        bool isPlayer = !isEnemy;

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
