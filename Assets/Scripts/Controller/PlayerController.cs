using Assets.Scripts.Entities;
using Assets.Scripts.Game;
using Assets.Scripts.Math;
using Assets.Scripts.UI;
using BehaviorTree;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;


namespace Assets.Scripts.Controller
{
    public class PlayerController : SpaceShipController
    {
        public LayerMask targetIgnore;
        public HUD hud;
        //[SerializeField]
        //private RectTransform _uiTargetEnemySelected;
        //[SerializeField]
        //private RectTransform _uiTargetToFollow;
        //[SerializeField]
        //private RectTransform _uiTargetIndicator;

        public float crosshairDistance = 900f;
        //public Texture2D cursor, cursorEnemy, cursorFriendly;


        private InputAction _turnAction;
        private InputAction _speedAction;
        private InputAction _inventoryAction;
        private InputAction _interactAction;
        private Vector2 _cursorMid;
        private Ship _selectedShip;
        private Ray ray;


        private InputActionMap _interact;

        private Vector3 mousePos;
        private bool _isLookingMouse = true;

        private Vector3 aimPoint;

        private void Awake()
        {
            base.Awake();
            hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>();


        }

        private void Start()
        {
            

            ray = new Ray();
            _cursorMid = new Vector2(((float)HUD.Instance.crosshair.width) * 0.5f, ((float)HUD.Instance.crosshair.height) * 0.5f);
            Cursor.SetCursor(HUD.Instance.crosshair, _cursorMid, CursorMode.Auto);

            _turnAction = InputSystem.actions.FindAction("Move");
            _speedAction = InputSystem.actions.FindAction("ScrollWheel");
            _inventoryAction = InputSystem.actions.FindAction("Inventory");
            _interactAction = InputSystem.actions.FindAction("Interact");

            InventoryUI.Instance.SetUpUI(Entity);


            UnselectTarget();

            HUD.Instance.targetHP = HUD.Instance.selectedTarget.GetChild(0).GetComponent<HealthBar>();

            HUD.Instance.playerHP.ship = Entity;
            
        }

        private void UnselectTarget()
        {
            HUD.Instance.selectedTarget.gameObject.SetActive(false);
            HUD.Instance.targetToFollow.gameObject.SetActive(false);
            HUD.Instance.targetIndicator.gameObject.SetActive(false);
            HUD.Instance.targetName.gameObject.SetActive(false);
        }

        private void Update()
        {
            
            Vector2 turnInput = _turnAction.ReadValue<Vector2>();
            Vector3 scrollInput = _speedAction.ReadValue<Vector2>();
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = crosshairDistance;
            Vector3 targetPoint = Vector3.zero;
            ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Input.GetMouseButton(1)) // right click
            {
                foreach (Turret t in Entity.Turrets)
                    _shootCmd.Exec(t, true);
            }
            else if (Input.GetMouseButton(0)) // left click
            {
                if (Physics.Raycast(ray, out RaycastHit h, Camera.main.farClipPlane, ~targetIgnore))
                {
                    if (_selectedShip != null && _selectedShip.blip != null)
                    {
                        _selectedShip.blip.gameObject.SetActive(true);
                    }
                    _selectedShip = h.collider.gameObject.GetComponent<Ship>();
                }
                else if(RadarBlip.targetedShip != null)
                {
                    if (_selectedShip != null && _selectedShip.blip != null)
                    {
                        _selectedShip.blip.gameObject.SetActive(true);
                    }
                    _selectedShip = RadarBlip.targetedShip;
                }
            }
            else if (_selectedShip == null)
            {
                if (Entity.detectedShips.Count > 0)
                    _selectedShip = AIEntityManager.Get().GetEntity(Entity.detectedShips.First()).Owner as Ship;
                else
                    _selectedShip = null;
            }

            if (turnInput.y > 0)
            {
                StartMoving(Entity.MaxSpeed);
            }
            else if (turnInput.y < 0)
            {              
                StartReversing(Entity.MaxSpeed);
            }

            if (_interactAction.WasPressedThisFrame())
            {
                GameEvents.Instance.OnUseEvent(HUD.Instance.yesNo.id);
            }

            if (_inventoryAction.WasPressedThisFrame())
            {
                InventoryUI.Instance.Open();
            }
            

            if (turnInput.x < 0)
                _strafeCmd.Exec(this, -200f);
            else if (turnInput.x > 0)
                _strafeCmd.Exec(this, 200f);
            else
            {
                strafeValue = Mathf.LerpUnclamped(strafeValue, 0, Time.deltaTime * 5.0f);
            }

            //Change mouse color if it hits some enemy target
            if (Physics.Raycast(ray, out RaycastHit hit, Camera.main.farClipPlane, ~targetIgnore))
            {
                Ship sh = hit.collider.gameObject.GetComponent<Ship>();
                if (sh != null)
                {
                    targetPoint = hit.point;
                    if (sh.team != Entity.team)
                        Cursor.SetCursor(hud.enemyCrosshair, _cursorMid, CursorMode.Auto);
                    else
                        Cursor.SetCursor(hud.friendCrosshair, _cursorMid, CursorMode.Auto);
                }
            }
            else
            {
                targetPoint = Camera.main.ScreenToWorldPoint(mousePos);
                Cursor.SetCursor(hud.crosshair, _cursorMid, CursorMode.Auto);
            }

           
            if (_selectedShip != null)
            {
                if(_selectedShip.blip != null)
                    _selectedShip.blip.gameObject.SetActive(false);
                hud.selectedTarget.gameObject.SetActive(true);
                if (_selectedShip.team != Entity.team)
                {
                    hud.targetToFollow.gameObject.SetActive(true);
                    hud.selectedTargetImg.texture = hud.selectedEnemy;
                }
                else
                {
                    hud.targetToFollow.gameObject.SetActive(false);
                    hud.selectedTargetImg.texture = hud.selectedFriend;
                }
                hud.targetName.gameObject.SetActive(true);
                hud.targetName.text = _selectedShip.Name;
                hud.targetHP.ship = _selectedShip;

                Vector3 v = RectTransformUtility.WorldToScreenPoint(Camera.main, _selectedShip.transform.position);
                hud.selectedTarget.position = v;


                
                //Point cannot be seen in viewport
                Vector3 viewPos = Camera.main.WorldToViewportPoint(_selectedShip.transform.position);
                if (((viewPos.x < 0 || viewPos.x > 1) && (viewPos.y < 0 || viewPos.y > 1)) || viewPos.z < 0)
                {

                    Vector2 fromTo = v - mousePos;
                    float deg = Mathf.Atan2(fromTo.y, fromTo.x) * Mathf.Rad2Deg;
                    hud.targetIndicator.gameObject.SetActive(true);  
                    hud.targetIndicator.position = mousePos;
                    if (viewPos.z >= 0)
                        hud.targetIndicator.localRotation = Quaternion.Euler(0, 0, deg - 90);
                    else
                        hud.targetIndicator.localRotation = Quaternion.Euler(0, 0, 90 - deg);

                    hud.selectedTarget.gameObject.SetActive(false);
                    hud.targetToFollow.gameObject.SetActive(false);

                    foreach (Turret t1 in Entity.Turrets)
                        _lookAt.Exec(t1, targetPoint);

                    return;
                }

                //Put the target recticle at correct position
                if (Entity.selectedTurret == null)
                    return;

                float t = AimingUtils.AimAhead(Entity, _selectedShip, Entity.selectedTurret.projectile);

                if (t > 0)
                {
                    aimPoint = _selectedShip.transform.position + _selectedShip.controller.rb.linearVelocity * t;

                }
                else
                    aimPoint = _selectedShip.transform.position;


                hud.targetToFollow.position = RectTransformUtility.WorldToScreenPoint(Camera.main, aimPoint);

                if (TargetToFollowUI.PointOnTarget)
                {
                    targetPoint = aimPoint;
                }
            }
            else
            {
                UnselectTarget();
            }
            hud.targetIndicator.gameObject.SetActive(false);


            
            foreach (Turret t in Entity.Turrets)
                _lookAt.Exec(t, targetPoint);

        }

        private void FixedUpdate()
        {
            base.FixedUpdate();

            if (_isLookingMouse)
            {

                //find the vector pointing from our position to the target
                Vector3 direction = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position).normalized;

                //create the rotation we need to be in to look at the target
                Quaternion lookRotation = Quaternion.LookRotation(direction, transform.up);

                float angle = Quaternion.Angle(transform.rotation, lookRotation);
                float timeToComplete = angle / Entity.TurnSpeed;
                float donePercentage = Mathf.Min(1F, Time.deltaTime / timeToComplete);

                //Entity.transform.rotation = Quaternion.Slerp(Entity.transform.rotation,lookRotation, donePercentage);
                rb.MoveRotation(Quaternion.Slerp(Entity.transform.rotation, lookRotation, donePercentage));
            }
        }

 
    }
}
