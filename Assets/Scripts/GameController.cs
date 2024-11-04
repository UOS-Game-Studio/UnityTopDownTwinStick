using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private int _livingEnemies;
    private int _maxEnemies;
    private int _killedEnemies;

    [SerializeField] private float roomStartWaitTime = 0.5f;
    private WaitForSeconds _startWaitTime;
    
    public UnityEvent onRoomComplete = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();
    public UnityEvent onRoomBegin = new UnityEvent();

    private void Start()
    {
        _startWaitTime = new WaitForSeconds(roomStartWaitTime);
        StartCoroutine(StartRoom());
    }

    private IEnumerator StartRoom()
    {
        yield return _startWaitTime;
        
        onRoomBegin.Invoke();
    }
    
    public void OnRoomStart(int maxEnemies)
    {
        _maxEnemies = maxEnemies;
    }
    
    public void OnEnemySpawned()
    {
        _livingEnemies++;
    }

    public void OnCharacterKilled(Common.Health charHealth)
    {
        bool isEnemy = charHealth.GetComponent<AI.EnemyCore>() != null;
        bool isPlayer = !isEnemy;

        if (isEnemy)
        {
            _livingEnemies--;
            _killedEnemies++;

            if (_killedEnemies >= _maxEnemies)
            {
                Debug.Log("Room complete");
                onRoomComplete.Invoke();
                return;
            }
            
        }

        if (isPlayer)
        {
            Debug.Log("Handle the game over state here");
            onGameOver.Invoke();
        }
    }
}
