using System;
using UnityEngine;

    
    public class TrackModelView : MonoBehaviour
    {
        
        public event EventHandler OnPause = (sender, e) => { };
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPause(this, EventArgs.Empty);
            }
        }
    }
