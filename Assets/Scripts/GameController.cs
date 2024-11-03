using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private int _livingEnemies;
    private int _maxEnemies;
    private int _killedEnemies;

    public UnityEvent onRoomComplete = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();
    
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

            if (_maxEnemies == _killedEnemies)
            {
                Debug.Log("Room complete");
                onRoomComplete.Invoke();
            }
            
        }

        if (isPlayer)
        {
            Debug.Log("Handle the game over state here");
            onGameOver.Invoke();
        }
    }
}
