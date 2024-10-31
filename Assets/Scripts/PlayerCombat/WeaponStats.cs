using UnityEngine;

namespace PlayerCombat
{
    [CreateAssetMenu]
    public class WeaponStats : ScriptableObject
    {
        public float fireDelay = 0.5f;
        public float baseDamage = 1.0f;
    }
}