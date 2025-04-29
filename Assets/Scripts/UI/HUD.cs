using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HUD : MonoBehaviour
    {

        public static HUD Instance { private set; get; }


        public RectTransform targetIndicator, selectedTarget, targetToFollow;
        public Texture2D crosshair, enemyCrosshair, friendCrosshair;
        public Texture2D selectedEnemy, selectedFriend;
        public RawImage selectedTargetImg;
        public HealthBar playerHP, targetHP;
        public TextMeshProUGUI targetName, level;
        public RadarBlip enemyBlip, friendlyBlip;
        public PrizeDialog prizeDialog;
        public YesNo yesNo;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
        }
    }
}
