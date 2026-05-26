using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Enemy Spawn Settings")]
    [SerializeField] private float _spawnInterval = 5f;

    private void Start()
    {
        StartCoroutine(CO_SpawnEnemyLoop());
    }

    private IEnumerator CO_SpawnEnemyLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);

            Debug.Log("Spawning Enemy");

            Spawner.Instance.SpawnEnemy();
        }
    }
}