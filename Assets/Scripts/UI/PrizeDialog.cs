using Assets.Scripts.Entities;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class PrizeDialog : MonoBehaviour
    {

        public static PrizeDialog Instance { private set; get; }

        public Button btnHp, btnTurret, btnShip, btnSpecial;

        public List<string> shipsToSpawn = new List<string>();


        private void Awake()
        {
            Instance = this;
            btnHp.onClick.AddListener(HPSelected);
            btnTurret.onClick.AddListener(EvasionSelected);
            btnShip.onClick.AddListener(FollowerSelected);

            Hide();
        }


        public void Show()
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }


        public void HPSelected()
        {
            Player.ship.maxHp += Player.ship.maxHp * 0.05f;
            Player.ship.currentHp = Player.ship.maxHp;
            Hide();
        }

        public void EvasionSelected()
        {
            Hide();
            Player.ship.evade += 5.0f;
            if (Player.ship.evade > 80f)
                Player.ship.evade = 80.0f;
        }

        public void FollowerSelected()
        {
            Hide();
            foreach(string s in shipsToSpawn)
                Spawns.Get().shipSpawnFunctions[s]("Drone");
        }

        public void SpecialSelected()
        {

        }


    }
}
