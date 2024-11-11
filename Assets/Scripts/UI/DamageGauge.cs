using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace UI
{
/*
 * Basic overheating system - basic UI that increases or decreases in value based on damage modifiers
 * or from doing nothing. Upon overheat, reset the value back to zero
 */
    [RequireComponent(typeof(Slider))]
    public class DamageGauge : MonoBehaviour
    {
        public PlayerCombat.WeaponStats weaponStats;
        public AnimationCurve adjustedDamageCurve;

        private Slider damageGauge;
        private float damageReduction = -1f;
        private bool _startedMoving = false;
        private bool _startedShooting = false;

        private const float MINSLIDERVALUE = 0.0f;
        private const float MAXSLIDERVALUE = 100.0f;

        private InputAction _MoveAction;
        private InputAction _AttackAction;
        private InputAction _RollAction;

        #region Unity

        /*private void Awake()
        {
            // Example damage curve. At the mid point, damage is equal to 3 and then ramps up towards 10
            adjustedDamageCurve.MoveKey(2, new Keyframe(MAXSLIDERVALUE, 10f));
            adjustedDamageCurve.MoveKey(1, new Keyframe(MAXSLIDERVALUE/2.0f, 3f));
        }*/

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            damageGauge = GetComponent<Slider>();
            damageGauge.maxValue = MAXSLIDERVALUE;
            damageGauge.minValue = MINSLIDERVALUE;

            // This section decouples the slider from the other scripts by finding the relevant input actions,
            // caching them, and then calling the relevant methods as needed.
            _MoveAction = InputSystem.actions.FindAction("Move", true);
            _AttackAction = InputSystem.actions.FindAction("Attack", true);
            _RollAction = InputSystem.actions.FindAction("Roll", true);

            _MoveAction.started += IA_OnMoveStarted;
            _MoveAction.canceled += IA_OnMoveEnded;
            _AttackAction.started += IA_OnAttackStarted;
            _AttackAction.canceled += IA_OnAttackEnded;
            _RollAction.performed += IA_OnRoll;
        }

        // Update is called once per frame. In here we are checking if the player is not performing a particular
        // action, in which case we decrease the slider value, which decreases their overall damage. Then we check
        // if actions are being performed, and to scale damage accordingly. Finally, we check if the player has
        // performed too many actions, in which case the damage gauge overheats, and resets to 0.
        void Update()
        {
            // This "true" input value will need to be swapped out on implementation of the active reload mechanic
            DecreaseDamageIfApplicable(true);
            IncreaseDamageBasedOnModifiers();
            CheckIfOverheated();
        }

        #endregion

        // A series of "input action" methods which lets us keep track of when the player has begun or cancelled
        // a particular action. In the case of rolling, it's simply whether the player has performed the roll.

        public void IA_OnMoveStarted(InputAction.CallbackContext obj)
        {
            _startedMoving = true;
        }

        public void IA_OnMoveEnded(InputAction.CallbackContext obj)
        {
            _startedMoving = false;
        }

        public void IA_OnAttackStarted(InputAction.CallbackContext obj)
        {
            _startedShooting = true;
        }

        public void IA_OnAttackEnded(InputAction.CallbackContext obj)
        {
            _startedShooting = false;
        }

        public void IA_OnRoll(InputAction.CallbackContext obj)
        {
            AdjustDamageGaugeValue(weaponStats.baseDamage + weaponStats.rollDamageModifier);
        }

        /// <summary>
        /// Input should come from damage modifier sources as positive or negative values
        /// Probably some event based system
        /// </summary>
        /// <param name="valueToAdjustBy"></param>
        public void AdjustDamageGaugeValue(float valueToAdjustBy)
        {
            damageGauge.value += valueToAdjustBy * Time.deltaTime;
            weaponStats.adjustedValue = adjustedDamageCurve.Evaluate(damageGauge.value);
            if(weaponStats.adjustedValue < 0)
            {
                weaponStats.adjustedValue = 0;
            }
        }

        // To help the scriptable object reset properly, we force the adjusted value back to 0 on destroy
        private void OnDestroy()
        {
            weaponStats.adjustedValue = 0f;
        }

        /// <summary>
        /// If we're moving or shooting, increase damage appropriately
        /// </summary>
        void IncreaseDamageBasedOnModifiers()
        {
            if(_startedMoving)
            {
                AdjustDamageGaugeValue(weaponStats.moveDamageModifier);
            }
            if(_startedShooting)
            {
                AdjustDamageGaugeValue(weaponStats.shootDamageModifier);
            }
        }

        /// <summary>
        /// This checks whether we're close enough to 100 in order to overheat. As we're using floats, we cater for
        /// the edge case of having 99.99999999 by subtracting the current value from the total and checking against
        /// some threshold (wiggle room)
        /// </summary>
        void CheckIfOverheated()
        {
            if (Math.Abs(damageGauge.value - MAXSLIDERVALUE) < 0.01f)
            {
                damageGauge.value = 0;
            }
        }

        /// <summary>
        /// Always decrease damage if applicable. Applicability depends on the active reload mechanic
        /// </summary>
        void DecreaseDamageIfApplicable(bool ifApplicable)
        {
            if(!ifApplicable)
            {
                return;
            }
            AdjustDamageGaugeValue(damageReduction);
        }
    }
}