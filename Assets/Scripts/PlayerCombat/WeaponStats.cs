using UnityEngine;

namespace PlayerCombat
{
    [CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
    public class WeaponStats : ScriptableObject
    {
        [Range(0.01f, 5.0f)]
        public float fireDelay = 0.5f;
        public float baseDamage = 1.0f;
        public float adjustedValue;
        public float moveDamageModifier = 1.0f;
        public float shootDamageModifier = 1.0f;
        public float rollDamageModifier = 1.0f;
    }
}