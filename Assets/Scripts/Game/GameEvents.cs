using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Instance { private set; get; }
        public event Action<int> OnUse;
        public event Action OnMessagePopup;


        private void Awake()
        {
            Instance = this;
        }

        public void OnUseEvent(int id)
        {
            if(OnUse != null)
                OnUse.Invoke(id);
        }


        public void OnPopup()
        {
            if(OnMessagePopup != null)
                OnMessagePopup.Invoke();
        }

    
    }
}
