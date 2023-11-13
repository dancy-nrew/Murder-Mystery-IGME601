using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DancyNrew.UI
{
    public class UI_System : MonoBehaviour
    {
        #region Variables
        [Header("Main Properties")]
        public UI_Screen m_StartScreen;
        [Header("System Events")]
        public UnityEvent onSwitchedScreen = new UnityEvent();

        [Header("Fader Properties")]
        public Image fadeImg;
        public float fadeInDuration = 1.0f;
        public float fadeOutDuration = 1.0f;    

        private Component[] screens = new Component[0];

        private UI_Screen prevScreen;
        private UI_Screen currentScreen;

        //property to get currentScreen and prevScreen
        public UI_Screen PrevScreen { get { return prevScreen; } }
        public UI_Screen CurrentScreen { get { return currentScreen; } }
        
        #endregion

        #region Main Methods
        void Start()
        {
            screens = GetComponentsInChildren<UI_Screen>(true);
            foreach(UI_Screen screen in screens)
            {
                screen.Init();
            }
            InitializeScreens();
            if (m_StartScreen)
            {
                SwitchScreens(m_StartScreen);
            }
            if (fadeImg)
            {
                fadeImg.gameObject.SetActive(true);
            }
            FadeIn();
        }

        #endregion

        #region Helper Methods
        public void SwitchScreens(UI_Screen screen)
        {
            if(screen)
            {
                if (currentScreen)
                {
                    currentScreen.CloseScreen();
                    prevScreen = currentScreen;
                }
                currentScreen = screen;
                currentScreen.gameObject.SetActive(true);
                currentScreen.StartScreen();
                if (onSwitchedScreen != null)
                {
                    onSwitchedScreen.Invoke();
                }
            }
        }

        public void FadeIn()
        {
            if (fadeImg)
            {
                fadeImg.CrossFadeAlpha(0f, fadeInDuration, false);
            }
        }
        public void FadeOut()
        {
            if (fadeImg)
            {
                fadeImg.CrossFadeAlpha(1f, fadeOutDuration, false);
            }
        }

        public void GoToPrevScreen()
        {
            if (prevScreen)
            {
                SwitchScreens(prevScreen);
            }
        }

       
        void InitializeScreens()
        {
            foreach(var screen in screens)
            {
                screen.gameObject.SetActive(true);
            }
        }
        #endregion
    }
}