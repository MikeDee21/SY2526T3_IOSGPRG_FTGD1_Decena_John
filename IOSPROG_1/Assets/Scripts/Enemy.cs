using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

public class Enemy : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private SpriteRenderer _arrowRenderer;

    [Header("Arrow Sprites")]
    [SerializeField] private List<Sprite> _greenArrowSprites;
    [SerializeField] private List<Sprite> _redArrowSprites;

    [Header("Movement")]
    [SerializeField] private float _enemySpeed;

    [Header("Rotating Arrow Settings")]
    [SerializeField] private bool _isRotatingArrow;
    [SerializeField] private float _rotationInterval = 0.5f;

    private SwipeDirection _displayedDirection;
    private SwipeDirection _requiredSwipe;

    private bool _isRedArrow;
    private bool _canBeHit = true;

    private Player _currentPlayer; 
    private void Start()
    {
        _enemySpeed = Random.Range(3f, 5f);
    }
    public void Initalize()
    {
        GenerateArrow();
    }

    private void Update()
    {
        transform.position += Vector3.down * _enemySpeed * Time.deltaTime;

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
            Spawner.Instance.RemoveEnemyFromList(this);
        }
    }

    #region player detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();

        if (player != null)
        {
            _currentPlayer = player;

            _canBeHit = true;

            Spawner.Instance.CurrentEnemy = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();

        if (player != null)
        {
            _currentPlayer = null;

            _canBeHit = false;

            if (Spawner.Instance != null &&
                Spawner.Instance.CurrentEnemy == this)
            {
                Spawner.Instance.CurrentEnemy = null;
            }
        }
    }

    #endregion

    public void CheckSwipe(SwipeDirection swipeDirection)
    {
        if (!_canBeHit)
            return;

        if (_currentPlayer != null &&
    (_currentPlayer.IsDashActive ||
     swipeDirection == _requiredSwipe))
        {
            KilledByPlayer();
        }
        else
        {
            if (_currentPlayer != null)
            {
                _currentPlayer.HitPlayer(1);
            }
        }
    }

    //Remove enemy without incrementing score 
    public void RemoveEnemy()
    {
        Spawner.Instance.RemoveEnemyFromList(this);

        Destroy(gameObject);
    }


    #region private functions

    private void GenerateArrow()
    {
        _displayedDirection = (SwipeDirection)Random.Range(0, 4);

        _isRedArrow = Random.value > 0.5f;

        _isRotatingArrow = Random.value <= 0.2f;

        if (_isRedArrow)
        {
            _requiredSwipe = GetOppositeDirection(_displayedDirection);
        }
        else
        {
            _requiredSwipe = _displayedDirection;
        }
        if (_isRotatingArrow)
        {
            StartCoroutine(CO_RotateArrow());
        }

        SetArrowSprite();
    }

    private SwipeDirection GetOppositeDirection(SwipeDirection direction)
    {
        switch (direction)
        {
            case SwipeDirection.Up:
                return SwipeDirection.Down;

            case SwipeDirection.Down:
                return SwipeDirection.Up;

            case SwipeDirection.Left:
                return SwipeDirection.Right;

            default:
                return SwipeDirection.Left;
        }
    }

    private void SetArrowSprite()
    {
        int directionIndex = (int)_displayedDirection;

        if (_isRedArrow)
        {
            _arrowRenderer.sprite =
                _redArrowSprites[directionIndex];
        }
        else
        {
            _arrowRenderer.sprite =
                _greenArrowSprites[directionIndex];
        }
    }
    private void RotateArrowDirection()
    {
        int nextIndex =
            ((int)_displayedDirection + 1) % 4;

        _displayedDirection =
            (SwipeDirection)nextIndex;

        if (_isRedArrow)
        {
            _requiredSwipe =
                GetOppositeDirection(_displayedDirection);
        }
        else
        {
            _requiredSwipe =
                _displayedDirection;
        }

        SetArrowSprite();
    }

    #region death functions

    private void KilledByPlayer()
    {
        float dropChance = 0.03f;

        Debug.Log("Enemy Killed");

        GameManager.Instance.IncrementScore(1);

        Spawner.Instance.RemoveEnemyFromList(this);

        if(_currentPlayer != null)
        {
            _currentPlayer.AddDashCharge(1);
        }

        Destroy(gameObject);

        //3% chance to spawn powerup 
        if (Random.value <= dropChance)
        {
            Spawner.Instance.SpawnPowerUp(transform.position);

            Debug.Log("PowerUp Spawned");
        }
    }

    private IEnumerator CO_RotateArrow()
    {
        while (true)
        {
            yield return new WaitForSeconds(_rotationInterval);

            RotateArrowDirection();
        }
    }
    #endregion

    #endregion
}
