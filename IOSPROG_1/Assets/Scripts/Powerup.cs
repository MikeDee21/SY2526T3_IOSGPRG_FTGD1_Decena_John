using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _powerupSpeed; 

    private bool _canBeTouched;
    private Player _currentPlayer;

    private void Start()
    {
        _powerupSpeed = 2.5f;
    }

    private void Update()
    {
        transform.position += Vector3.down * _powerupSpeed * Time.deltaTime;
    }
    public void InitializePowerUp()
    {
        Debug.Log("PowerUp Initialized");
    }

    public void CollectPowerUp()
    {
        if (!_canBeTouched)
        {
            return;
        }

        if (_currentPlayer != null)
        {
            _currentPlayer.GainExtraLife(1);

            Debug.Log("Player gained extra life");
        }

        Spawner.Instance.RemovePowerUpFromList(this);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();

        if (player != null)
        {
            _canBeTouched = true;

            _currentPlayer = player;

            Spawner.Instance.CurrentPowerUp = this;

            CollectPowerUp();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();

        if (player != null)
        {
            _canBeTouched = false;

            _currentPlayer = null;

            if (Spawner.Instance != null &&
                Spawner.Instance.CurrentPowerUp == this)
            {
                Spawner.Instance.CurrentPowerUp = null;
            }
        }
    }
}