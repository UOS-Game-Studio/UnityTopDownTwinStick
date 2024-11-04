using UnityEngine;
using UnityEngine.UI;

namespace UI
{
/*
 * Basic overheating system - basic UI that increases or decreases in value based on damage modifiers
 * or from doing nothing. Upon overheat, reset the value back to zero
 */
    [RequireComponent(typeof(Slider))]
    public class DamageGauge : MonoBehaviour
    {
        Slider damageGauge;
        float damageReduction;

        public delegate void OnMoveDashShoot();

        public static event OnMoveDashShoot onMoveDashShoot;

        // Handles slider value changes. Functions are currently self contained within this script
        // move functionality outside of script if/when necessary to link with the player-character
        public static void RaiseOnMoveDashShoot()
        {
            if (onMoveDashShoot != null)
            {
                onMoveDashShoot();
            }
        }

        /*
         * Basic overheating system - basic UI that increases or decreases in value based on damage
         * modifiers or from doing nothing. Upon overheat, reset the value back to zero
         */
        // check if we're not moving, shooting or dashing

        #region Unity

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            damageGauge = GetComponent<Slider>();
            damageReduction = -1f;
            DebugStates(false); // remove later
            onMoveDashShoot += IncreaseDamageBasedOnModifiers;
            onMoveDashShoot += DecreaseDamageIfApplicable;
            onMoveDashShoot += CheckIfOverheated;
        }

        // Update is called once per frame
        void Update()
        {
            DebugOnButtonPress(); // remove later
            RaiseOnMoveDashShoot();
        }

        #endregion

        #region debugging

        /// <summary>
        /// 1 turns all states off, 2, 3, 4 turns them on
        /// </summary>
        void DebugOnButtonPress()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DebugStates(false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                DebugWalkState(true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                DebugShootingState(true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                DebugDashState(true);
            }
        }

        void DebugStates(bool debugValue)
        {
            StaticStateValues.isWalking = StaticStateValues.isShooting = StaticStateValues.isDashing = debugValue;
        }

        void DebugWalkState(bool debugValue)
        {
            StaticStateValues.isWalking = debugValue;
        }

        void DebugShootingState(bool debugValue)
        {
            StaticStateValues.isShooting = debugValue;
        }

        void DebugDashState(bool debugValue)
        {
            StaticStateValues.isDashing = debugValue;
        }

        #endregion

        /// <summary>
        /// Input should come from damage modifier sources as positive or negative values
        /// Probably some event based system
        /// </summary>
        /// <param name="valueToAdjustBy"></param>
        public void AdjustDamageGaugeValue(float valueToAdjustBy)
        {
            damageGauge.value += valueToAdjustBy * Time.deltaTime;
        }

        void IncreaseDamageBasedOnModifiers()
        {
            if (StaticStateValues.isWalking)
            {
                AdjustDamageGaugeValue(1f);
            }

            if (StaticStateValues.isShooting)
            {
                AdjustDamageGaugeValue(1f);
            }

            if (StaticStateValues.isDashing)
            {
                AdjustDamageGaugeValue(1f);
            }
        }

        void CheckIfOverheated()
        {
            if (damageGauge.value == 100)
            {
                damageGauge.value = 0;
            }
        }

        void DecreaseDamageIfApplicable()
        {
            if (!StaticStateValues.isWalking && !StaticStateValues.isShooting && !StaticStateValues.isDashing)
            {
                AdjustDamageGaugeValue(damageReduction);
            }
        }
    }
}