using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using GameFramework;

namespace TowerDefence.Data
{
    public sealed class DataTower : DataBase
    {
        private IDataTable<DRTower> dtTower;

        private Dictionary<int, TowerData> dicTowerData;
        private Dictionary<int, Tower> dicTower;

        private int serialId = 0;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Tower");
        }

        protected override void OnLoad()
        {
            dtTower = GameEntry.DataTable.GetDataTable<DRTower>();
            if (dtTower == null)
                throw new System.Exception("Can not get data table Tower");

            dicTowerData = new Dictionary<int, TowerData>();
            dicTower = new Dictionary<int, Tower>();

            DRTower[] drTowers = dtTower.GetAllDataRows();
            foreach (var drTower in drTowers)
            {
                TowerData towerData = new TowerData(drTower);
                dicTowerData.Add(drTower.Id, towerData);
            }
        }

        private int GenrateSerialId()
        {
            return ++serialId;
        }

        public TowerData GetTowerData(int id)
        {
            if (!dicTowerData.ContainsKey(id))
            {
                Log.Error("Can not find tower data id '{0}'.", id);
                return null;
            }

            return dicTowerData[id];
        }

        public Tower CreateTower(int towerId, int level = 0)
        {
            if (!dicTowerData.ContainsKey(towerId))
            {
                Log.Error("Can not find tower data id '{0}'.", towerId);
                return null;
            }

            int serialId = GenrateSerialId();
            Tower tower = Tower.Create(dicTowerData[towerId], serialId);
            dicTower.Add(serialId, tower);

            return tower;
        }

        public void DestroyTower(int serialId)
        {
            if (!dicTower.ContainsKey(serialId))
            {
                Log.Error("Can not find tower serialId '{0}'.", serialId);
                return;
            }

            ReferencePool.Release(dicTower[serialId]);
            dicTower.Remove(serialId);
        }

        public void DestroyTower(Tower tower)
        {
            DestroyTower(tower.SerialId);
        }

        

        public void SellTower(int serialId)
        {
            if (!dicTower.ContainsKey(serialId))
            {
                Log.Error("Can not find tower serialId '{0}'.", serialId);
                return;
            }

            Tower tower = dicTower[serialId];
            GameEntry.Event.Fire(this, HideTowerInLevelEventArgs.Create(tower.SerialId));
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRTower>();

            dicTowerData = null;

            if (dicTower != null)
            {
                foreach (var item in dicTower.Values)
                {
                    ReferencePool.Release(item);
                }
                dicTower.Clear();
            }

            serialId = 0;
        }

        protected override void OnShutdown()
        {
        }
    }

}