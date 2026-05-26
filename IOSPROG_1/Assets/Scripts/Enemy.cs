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

    [SerializeField] private Sprite _greenUp;
    [SerializeField] private Sprite _greenDown;
    [SerializeField] private Sprite _greenLeft;
    [SerializeField] private Sprite _greenRight;

    [SerializeField] private Sprite _redUp;
    [SerializeField] private Sprite _redDown;
    [SerializeField] private Sprite _redLeft;
    [SerializeField] private Sprite _redRight;

    [Header("Movement")]
    [SerializeField] private float _enemySpeed = 3f;

    private SwipeDirection _displayedDirection;
    private SwipeDirection _requiredSwipe;

    private bool _isRedArrow;
    private bool _canBeHit = true;

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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();

        if (player != null)
        {
            _canBeHit = true;
            Spawner.Instance.CurrentEnemy = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();

        if (player != null)
        {
            _canBeHit = false;

            if (Spawner.Instance != null &&
                Spawner.Instance.CurrentEnemy == this)
            {
                Spawner.Instance.CurrentEnemy = null;
            }
        }
    }


    public void CheckSwipe(SwipeDirection swipeDirection)
    {
        if (!_canBeHit)
            return;

        if (swipeDirection == _requiredSwipe)
        {
            Die();
        }
        else
        {
            PlayerDies();
        }
    }

    private void GenerateArrow()
    {
        _displayedDirection = (SwipeDirection)Random.Range(0, 4);

        _isRedArrow = Random.value > 0.5f;

        if (_isRedArrow)
        {
            _requiredSwipe = GetOppositeDirection(_displayedDirection);
        }
        else
        {
            _requiredSwipe = _displayedDirection;
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
        if (!_isRedArrow)
        {
            switch (_displayedDirection)
            {
                case SwipeDirection.Up:
                    _arrowRenderer.sprite = _greenUp;
                    break;

                case SwipeDirection.Down:
                    _arrowRenderer.sprite = _greenDown;
                    break;

                case SwipeDirection.Left:
                    _arrowRenderer.sprite = _greenLeft;
                    break;

                case SwipeDirection.Right:
                    _arrowRenderer.sprite = _greenRight;
                    break;
            }
        }
        else
        {
            switch (_displayedDirection)
            {
                case SwipeDirection.Up:
                    _arrowRenderer.sprite = _redUp;
                    break;

                case SwipeDirection.Down:
                    _arrowRenderer.sprite = _redDown;
                    break;

                case SwipeDirection.Left:
                    _arrowRenderer.sprite = _redLeft;
                    break;

                case SwipeDirection.Right:
                    _arrowRenderer.sprite = _redRight;
                    break;
            }
        }
    }

    
    private void Die()
    {
        Debug.Log("Enemy Killed");

        Spawner.Instance.RemoveEnemyFromList(this);

        Destroy(gameObject);
    }

    private void PlayerDies()
    {
        Debug.Log("Player Died");
    }

}
