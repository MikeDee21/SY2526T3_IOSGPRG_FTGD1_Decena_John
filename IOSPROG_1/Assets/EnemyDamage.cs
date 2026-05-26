using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private bool _hasHitPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasHitPlayer)
        {
            return;
        }

        Player player = collision.GetComponentInParent<Player>();

        if (player == null)
        {
            return;
        }

        _hasHitPlayer = true;

        player.HitPlayer(1);

        Enemy enemy = GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            enemy.RemoveEnemy();
        }
    }
}