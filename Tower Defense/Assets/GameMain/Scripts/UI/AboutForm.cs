//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using TowerDefence.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace TowerDefence
{
    public class AboutForm : UGuiForm
    {
        public Button buildtower;
        TowerData towerData;
        IPlacementArea placementArea;
        Vector3 position;
        Quaternion rotation;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            towerData = GameEntry.Data.GetData<DataTower>().GetTowerData(1);
            buildtower.onClick.AddListener(BuildTowerClick);
        }

        public void create(IPlacementArea placementArea, Vector3 position, Quaternion rotation)
        {
            this.placementArea = placementArea;
            this.position = position;
            this.rotation = rotation;
        }

        private void BuildTowerClick()
        {
            GameEntry.Event.Fire(this, BuildTowerEventArgs.Create(towerData, placementArea, position, rotation));
        }
    }
}
