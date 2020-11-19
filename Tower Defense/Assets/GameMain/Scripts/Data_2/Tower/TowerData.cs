using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace TowerDefence.Data
{
    public class TowerData
    {
        private DRTower dRTower;

        public int Id
        {
            get
            {
                return dRTower.Id;
            }
        }

        public string Name
        {
            get
            {
                return GameEntry.Localization.GetString(dRTower.NameId);
            }
        }

        public int EntityId
        {
            get
            {
                return dRTower.EntityId;
            }
        }

        public float MaxHP
        {
            get
            {
                return dRTower.MaxHP;
            }
        }

        public int Money
        {
            get
            {
                return dRTower.Price;
            }
        }

        public string Type
        {
            get
            {
                return dRTower.Type;
            }
        }

        public TowerData(DRTower dRTower)
        {
            this.dRTower = dRTower;
        }
    }
}


