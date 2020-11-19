using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework;

namespace TowerDefence.Data
{
    public class Tower : IReference
    {
        private TowerData towerData;

        public int SerialId
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return towerData.Name;
            }
        }

        public float MaxHP
        {
            get
            {
                return towerData.MaxHP;
            }
        }
        public float Money
        {
            get
            {
                return towerData.Money;
            }
        }

        public int EntityId
        {
            get
            {
                return towerData.EntityId;
            }
        }

        public string Type
        {
            get
            {
                return towerData.Type;
            }
        }

        public Tower()
        {
            towerData = null;
            SerialId = 0;
        }

        public static Tower Create(TowerData towerData, int serialId)
        {
            Tower tower = ReferencePool.Acquire<Tower>();
            tower.towerData = towerData;
            tower.SerialId = serialId;
            return tower;
        }

        public void Clear()
        {
            towerData = null;
            SerialId = 0;
        }
    }
}


