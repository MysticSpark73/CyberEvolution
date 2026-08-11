using UnityEngine;

namespace CyberEvolution.Entities
{
    public interface IDamageable
    {
        void TakeDamage(float damage, Vector2Int from);
    }
}