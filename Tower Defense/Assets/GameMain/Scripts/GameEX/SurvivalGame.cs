//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;
using TowerDefence.Data;

namespace TowerDefence
{
    public class SurvivalGame : GameBase
    {
        private IPlacementArea currentArea;
        private int? towerDefence_id;
        IntVector2 m_GridPosition;
        EntityLoader entityLoader;
        DataTower dataTower;
        public override GameMode GameMode
        {
            get
            {
                return GameMode.Survival;
            }
        }


        public override void Initialize()
        {
            base.Initialize();
            GameEntry.Event.Subscribe(BuildTowerEventArgs.EventId, OnBuildTower);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, ChangeBuildTower);
            dataTower = GameEntry.Data.GetData<DataTower>();
            entityLoader = EntityLoader.Create(this);
        }

        private void ChangeBuildTower(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne == null)
                return;
            UIForm towerselect = GameEntry.UI.GetUIForm((int)towerDefence_id);
            AboutForm aboutForm = (AboutForm)towerselect.Logic;
            Vector3 position = currentArea.GridToWorld(m_GridPosition, IntVector2.one);
            Quaternion rotation = currentArea.transform.rotation;
            aboutForm.create(currentArea, position, rotation);
        }

        private void OnBuildTower(object sender, GameEventArgs e)
        {
            BuildTowerEventArgs ne = (BuildTowerEventArgs)e;
            if (ne == null)
                return;

            CreateTower(ne.TowerData, ne.PlacementArea, IntVector2.one, ne.Position, ne.Rotation);

        }

        public void CreateTower(TowerData towerData, IPlacementArea placementArea, IntVector2 placeGrid, Vector3 position, Quaternion rotation)
        {
            if (towerData == null)
                return;
            Tower tower = dataTower.CreateTower(towerData.Id);
            entityLoader.ShowEntity(towerData.EntityId, TypeUtility.GetEntityType(tower.Type),
            (entity) =>
            {
                /*EntityTowerBase entityTowerBase = entity.Logic as EntityTowerBase;
                dicTowerInfo.Add(tower.SerialId, TowerInfo.Create(tower, entityTowerBase, placementArea, placeGrid));*/
            }
            );

        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MoveGhost();
            }
            else if(Input.GetMouseButtonDown(1)){
                if(towerDefence_id != null)
                    GameEntry.UI.CloseUIForm((int)towerDefence_id);
            }
        }

        private void MoveGhost(bool hideWhenInvalid = true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction*500, Color.blue, 50);
            int LayerMaskid = LayerMask.GetMask("towerbuild");
            if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMaskid))
            {
                MoveGhostWithRaycastHit(hit);
            }
        }

        private void MoveGhostWithRaycastHit(RaycastHit raycast)
        {
            currentArea = raycast.collider.GetComponent<IPlacementArea>();

            if (currentArea == null)
            {
                Log.Error("There is not an IPlacementArea attached to the collider found on the m_PlacementAreaMask");
                return;
            }
            m_GridPosition = currentArea.WorldToGrid(raycast.point, IntVector2.one);
            TowerFitStatus fits = currentArea.Fits(m_GridPosition, IntVector2.one);

            bool canPlace = fits == TowerFitStatus.Fits;
            if (canPlace)
            {
                towerDefence_id = GameEntry.UI.OpenUIForm(EnumUIForm.UITowerSelectForm);
            }
        }
    }
}
