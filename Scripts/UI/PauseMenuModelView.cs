using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    // панель меню паузы - скрипт висит на префабре в папке Resources
    public class PauseMenuModelView : MonoBehaviour
    {
        public Button resumeButton;
        public Button exitToMenuButton;

        public event EventHandler OnResume = (sender, e) => { };
        public event EventHandler OnExitToMainMenu = (sender, e) => { };
        
        private void Start()
        {
            resumeButton.onClick.AddListener(delegate
            {
                OnResume(this, EventArgs.Empty);
            });
            exitToMenuButton.onClick.AddListener(delegate
            {
                OnExitToMainMenu(this, EventArgs.Empty);
            });
        }
    }
