using UnityEngine;

namespace PlayerCombat
{
    [CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
    public class WeaponStats : ScriptableObject
    {
        [Range(0.01f, 5.0f)]
        public float fireDelay = 0.5f;
        public float baseDamage = 1.0f;
    }
}