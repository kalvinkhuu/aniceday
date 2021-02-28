/******************************************************************************
 * Spine Runtimes License Agreement
 * Last updated January 1, 2020. Replaces all prior versions.
 *
 * Copyright (c) 2013-2020, Esoteric Software LLC
 *
 * Integration of the Spine Runtimes into software or otherwise creating
 * derivative works of the Spine Runtimes is permitted under the terms and
 * conditions of Section 2 of the Spine Editor License Agreement:
 * http://esotericsoftware.com/spine-editor-license
 *
 * Otherwise, it is permitted to integrate the Spine Runtimes into software
 * or otherwise create derivative works of the Spine Runtimes (collectively,
 * "Products"), provided that each user of the Products must obtain their own
 * Spine Editor license and redistribution of the Products in any form must
 * include this license and copyright notice.
 *
 * THE SPINE RUNTIMES ARE PROVIDED BY ESOTERIC SOFTWARE LLC "AS IS" AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL ESOTERIC SOFTWARE LLC BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES,
 * BUSINESS INTERRUPTION, OR LOSS OF USE, DATA, OR PROFITS) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THE SPINE RUNTIMES, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using Spine.Unity;
using UnityEngine.SceneManagement;

namespace Spine.Unity.Examples
{

    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {

        public Interactable focus;
        public LayerMask movementMask;
        

        public enum CharacterState
        {
            None,
            Idle,
            Walk,
            Run,
            Crouch,
            Rise,
            Fall,
            Attack
        }

        [Header("Components")]
        public CharacterController controller;

        [Header("Controls")]
        public string XAxis = "Horizontal";
        public string YAxis = "Vertical";
        public string JumpButton = "Jump";
        public string AttackButton = "Attack";
        public string InteractionButton = "Interaction";

        [Header("Moving")]
        public float walkSpeed = 1.5f;
        public float runSpeed = 7f;
        public float gravityScale = 6.6f;

        [Header("Jumping")]
        public float jumpSpeed = 25;
        public float minimumJumpDuration = 0.5f;
        public float jumpInterruptFactor = 0.5f;
        public float forceCrouchVelocity = 25;
        public float forceCrouchDuration = 0.5f;

        //Added the attack header and the interaction header
        [Header("Attack")]
        public float attackDamage = 10;

        [Header("Health")]
        

        [Header("Animation")]
        public SkeletonAnimationHandleExample animationHandle;

        // Events
        public event UnityAction OnJump, OnLand, OnHardLand;

        Vector2 input = default(Vector2);
        Vector3 velocity = default(Vector3);
        float minimumJumpEndTime = 0;
        float forceCrouchEndTime;
        bool wasGrounded = false;
        private PlayerWeapon playerWeapon = null;

        CharacterState previousState, currentState;

        // SCORE OF THE DEEDS
        public static int GoodDeeds = 0;
        public static int BadDeeds = 0;

        public static int murderSins = 0;
        public static int stealOrGiveMoney = 0;
        public static int stealOrGiveFood = 0;

        public static int helpingPeople = 0;

        public int amountMoney = 0;
        public int amountFood = 0; 

        private Interactable InteractingObject = null;

        private House houseEntered = null;
        private bool isInsideHouse = false;

        public int LifeTimerLimit = 180;
        public int currentHealth;
        public HealthBar healthBar;

        void Start()
        {
            currentHealth = LifeTimerLimit;
            healthBar.SetMaxHealth(LifeTimerLimit);
            playerWeapon = GetComponentInChildren<PlayerWeapon>();
            
        }

        void Update()
        {
            float dt = Time.deltaTime;
            bool isGrounded = controller.isGrounded;
            bool landed = !wasGrounded && isGrounded;

            // Dummy input.
            input.x = Input.GetAxis(XAxis);
            input.y = Input.GetAxis(YAxis);
            bool inputJumpStop = Input.GetButtonUp(JumpButton);
            bool inputJumpStart = Input.GetButtonDown(JumpButton);
            bool doCrouch = (isGrounded && input.y < -0.5f) || (forceCrouchEndTime > Time.time);
            bool doJumpInterrupt = false;
            bool doJump = false;
            bool hardLand = false;

            // Input for the attack 
            bool inputAttackStart = Input.GetButtonDown(AttackButton);
            bool inputAttackStop = Input.GetButtonUp(AttackButton);
            

            if (landed)
            {
                if (-velocity.y > forceCrouchVelocity)
                {
                    hardLand = true;
                    doCrouch = true;
                    forceCrouchEndTime = Time.time + forceCrouchDuration;
                }
            }

            if (!doCrouch)
            {
                if (isGrounded)
                {
                    if (inputJumpStart)
                    {
                        doJump = true;
                    }
                }
                else
                {
                    doJumpInterrupt = inputJumpStop && Time.time < minimumJumpEndTime;
                }
            }

            // Dummy physics and controller using UnityEngine.CharacterController.
            Vector3 gravityDeltaVelocity = Physics.gravity * gravityScale * dt;

            if (doJump)
            {
                velocity.y = jumpSpeed;
                minimumJumpEndTime = Time.time + minimumJumpDuration;
            }
            else if (doJumpInterrupt)
            {
                if (velocity.y > 0)
                    velocity.y *= jumpInterruptFactor;
            }

            velocity.x = 0;
            if (!doCrouch)
            {
                if (input.x != 0)
                {
                    velocity.x = Mathf.Abs(input.x) > 0.6f ? runSpeed : walkSpeed;
                    velocity.x *= Mathf.Sign(input.x);
                }
            }


            if (!isGrounded)
            {
                if (wasGrounded)
                {
                    if (velocity.y < 0)
                        velocity.y = 0;
                }
                else
                {
                    velocity += gravityDeltaVelocity;
                }
            }
            controller.Move(velocity * dt);
            wasGrounded = isGrounded;

            

            // Determine and store character state
            if (isGrounded)
            {
                if (inputAttackStart)
                {
                    playerWeapon.SetIsAttacking(true);
                }
                else if (inputAttackStop)
                {
                    playerWeapon.SetIsAttacking(false);
                }

                if (playerWeapon.GetIsAttacking())
                {
                    currentState = CharacterState.Attack;
                }
                else
                {
                    if (doCrouch)
                    {
                        currentState = CharacterState.Crouch;
                    }

                    else
                    {
                        if (input.x == 0)
                            currentState = CharacterState.Idle;
                        else
                            currentState = Mathf.Abs(input.x) > 0.6f ? CharacterState.Run : CharacterState.Walk;
                    }
                }



            }
            else
            {
                currentState = velocity.y > 0 ? CharacterState.Rise : CharacterState.Fall;
            }

            bool stateChanged = previousState != currentState;
            previousState = currentState;

            // Animation
            // Do not modify character parameters or state in this phase. Just read them.
            // Detect changes in state, and communicate with animation handle if it changes.
            if (stateChanged)
                HandleStateChanged();

            if (input.x != 0)
                animationHandle.SetFlip(input.x);

            // Fire events.
            if (doJump)
            {
                OnJump.Invoke();
            }
            if (landed)
            {
                if (hardLand)
                {
                    OnHardLand.Invoke();
                }
                else
                {
                    OnLand.Invoke();
                }
            }

            if (transform.position.y <= -5)
            {
                currentHealth = 0;
            }

            if (currentHealth <= 0)
            {
                SceneManager.LoadScene("TransitionScene");
            }

            if (Input.GetButtonDown("Interact"))
            {
                if (InteractingObject != null)
                {
                    InteractingObject.SetInteracting(true);
                }
            }

            if (Input.GetButtonDown("Exit"))
            {
                if (isInsideHouse && houseEntered != null)
                {
                    houseEntered.SetDoorOpened(false);
                    isInsideHouse = false;
                }
            }

            if (InteractingObject != null && InteractingObject.GetIsInteracting()) 
            {
                
                if (houseEntered != null) 
                {
                    houseEntered.SetDoorOpened(true);
                    isInsideHouse = true;
                }
                
            }

            currentHealth = LifeTimerLimit - (int)dt;
            

        }

        private void LateUpdate()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, isInsideHouse ? houseEntered.GetPlaceToStandInHouse() : 0.0f);
        }

        void HandleStateChanged()
        {
            // When the state changes, notify the animation handle of the new state.
            string stateName = null;
            switch (currentState)
            {
                case CharacterState.Idle:
                    stateName = "idle";
                    break;
                case CharacterState.Walk:
                    stateName = "walk";
                    break;
                case CharacterState.Run:
                    stateName = "run";
                    break;
                case CharacterState.Crouch:
                    stateName = "crouch";
                    break;
                case CharacterState.Rise:
                    stateName = "rise";
                    break;
                case CharacterState.Fall:
                    stateName = "fall";
                    break;
                case CharacterState.Attack:
                    stateName = "attack";
                    break;
                default:
                    break;
            }

            animationHandle.PlayAnimationForState(stateName, 0);
        }

        public void AddGoodDeed() 
        {
            GoodDeeds++;
        }

        public int GetGoodDeed() 
        {
            return GoodDeeds;
        }

        public void AddBadDeed()
        {
            BadDeeds++;
        }

        public int GetBadDeed()
        {
            return BadDeeds;
        }

        public void AddMurderDeed()
        {
            murderSins++;
        }

        public int GetMurderDeed()
        {
            return murderSins;
        }

        public void AddHelpingDeed()
        {
            helpingPeople++;
        }

        public int GetHelpingDeed()
        {
            return helpingPeople;
        }

        public void StealMoneyDeed()
        {
            stealOrGiveMoney--;
        }

        public void GiveMoneyDeed() 
        {
            stealOrGiveMoney++;
            stealOrGiveMoney++;

        }

        public int GetMoneyDeed()
        {
            return stealOrGiveMoney;
        }

        public void StealFoodDeed()
        {
            stealOrGiveFood--;
        }

        public void GiveFoodDeed()
        {
            stealOrGiveFood++;
            stealOrGiveFood++;

        }

        public int GetFoodDeed()
        {
            return stealOrGiveFood;
        }



        private void OnTriggerEnter(Collider other)
        {
            Interactable Interaction = other.GetComponent<Interactable>();
            if (Interaction != null)
            {
                InteractingObject = Interaction;
                houseEntered = InteractingObject.GetComponent<House>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Interactable Interaction = other.GetComponent<Interactable>();
            if (Interaction != null)
            {
                if (InteractingObject != null)
                {
                    InteractingObject.SetInteracting(false);
                }
                InteractingObject = null;
            }
        }

    }
}
