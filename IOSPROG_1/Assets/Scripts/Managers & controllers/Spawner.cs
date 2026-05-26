using System.Collections.Generic;
using UnityEngine;

public class Spawner : Singleton<Spawner>
{
    [Header("Enemy")]
    [SerializeField] private GameObject _enemyPrefab;

    [Header("Power Up")]
    [SerializeField] private GameObject _powerUpPrefab;

    [Header("Spawn Point")]
    [SerializeField] private GameObject _spawnLocation;

    private List<GameObject> _enemies = new List<GameObject>();
    private List<GameObject> _powerUps = new List<GameObject>();

    public Enemy CurrentEnemy;
    public Powerup CurrentPowerUp;

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(
            _enemyPrefab,
            _spawnLocation.transform.position,
            Quaternion.identity
        );

        Enemy enemyScript = enemy.GetComponent<Enemy>();

        enemyScript.Initalize();

        _enemies.Add(enemy);
    }

    public void SpawnPowerUp(Vector3 spawnPosition)
    {
        GameObject powerUp = Instantiate(
            _powerUpPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Powerup powerUpScript = powerUp.GetComponent<Powerup>();

        powerUpScript.InitializePowerUp();

        _powerUps.Add(powerUp);
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        _enemies.Remove(enemy.gameObject);

        if (CurrentEnemy == enemy)
        {
            CurrentEnemy = null;
        }
    }

    public void RemovePowerUpFromList(Powerup powerUp)
    {
        _powerUps.Remove(powerUp.gameObject);

        if (CurrentPowerUp == powerUp)
        {
            CurrentPowerUp = null;
        }
    }
}