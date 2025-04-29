using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class RadarBlip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Ship ship;

        private RectTransform _rect;
        public RawImage image;

        public static Ship targetedShip;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _rect.localScale = new Vector3(0.35f, 0.35f, 1f);
            image = GetComponent<RawImage>();
        }


        private void Update()
        {
            if (ship != null && ship.currentHp > 0)
            {
                Vector3 viewPos = Camera.main.WorldToViewportPoint(ship.transform.position);
                if (((viewPos.x < 0 || viewPos.x > 1) && (viewPos.y < 0 || viewPos.y > 1)) || viewPos.z < 0)
                {
                    image.enabled = false;
                }
                else
                {
                    image.enabled = true;
                }

                _rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, ship.transform.position);
            }
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            targetedShip = ship;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            targetedShip = null;
        }


    }
}
