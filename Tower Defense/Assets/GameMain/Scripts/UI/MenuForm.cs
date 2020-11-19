//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;

namespace TowerDefence
{
    public class MenuForm : UGuiForm
    {
        public Button levelSelectButton;
        public Button optionButton;
        public Button quitButton;
        public ProcedureMenu m_ProcedureMenu;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            levelSelectButton.onClick.AddListener(OnLevelSelectButtonClick);
            optionButton.onClick.AddListener(OnOptionButtonClick);
            quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureMenu = (ProcedureMenu)userData;
            if (m_ProcedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnLevelSelectButtonClick()
        {
            m_ProcedureMenu.StartGame();
        }

        private void OnOptionButtonClick()
        {

        }

        private void OnQuitButtonClick()
        {
           
        }
    }
}