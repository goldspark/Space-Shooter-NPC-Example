using Assets.Scripts.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.UI
{
    public class YesNo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private float _cT = 0.0f;
        private const float DISSAPEAR_TIME = 30.0f;
        private bool _isUsable = false;
        private AudioSource _uiSound;
        public int id = -1;
        
        private void Awake()
        {
            _uiSound = GetComponent<AudioSource>();
        }

        private void Start()
        {
            GameEvents.Instance.OnMessagePopup += PlaySound;
        }


        private void Update()
        {
            if(gameObject.activeSelf && !_isUsable)
            {
                _cT += Time.deltaTime;

                if( _cT > DISSAPEAR_TIME )
                {
                    _cT = 0.0f;
                    gameObject.SetActive(false);
                }
            }
        }

        public void SetText(string text, bool isZone = false, int id = -1)
        {
            gameObject.SetActive(true);

            _text.text = text;

            _isUsable = isZone;

            if (!isZone)
                GameEvents.Instance.OnPopup();

            if(id >= 0)
            {
                this.id = id;
            }
        }

        public void StopText()
        {
            _cT = 0.0f;
            gameObject.SetActive(false);
            id = -1;
        }

        private void PlaySound()
        {
            _uiSound.Play();
        }


        private void OnDestroy()
        {
            id = -1;
        }
    }
}
