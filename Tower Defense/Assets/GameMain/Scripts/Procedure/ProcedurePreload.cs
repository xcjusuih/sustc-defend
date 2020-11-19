﻿using GameFramework.Procedure;
using GameFramework.Event;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;
using TowerDefence.Data;

namespace TowerDefence
{
    public class ProcedurePreload : ProcedureBase
    {

        public static readonly string[] DataTableNames = new string[]
        {
            "Scene",
            "UIForm",
            "Sound",
            "UISound"
        };
        private DataBase[] datas;
        private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);

            //GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            //GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            m_LoadedFlag.Clear();

            GameFramework.Data.Data[] _datas = GameEntry.Data.GetAllData();

            datas = new DataBase[_datas.Length];
            for (int i = 0; i < _datas.Length; i++)
            {
                if (_datas[i] is DataBase)
                {
                    datas[i] = _datas[i] as DataBase;
                }
                else
                {
                    throw new System.Exception(string.Format("Data {0} is not derive form DataBase", _datas[i].GetType()));
                }
            }

            PreloadResources();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            //GameEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            //GameEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            foreach (KeyValuePair<string, bool> loadedFlag in m_LoadedFlag)
            {
                if (!loadedFlag.Value)
                {
                    return;
                }
            }
            if (datas == null)
                return;

            foreach (var item in datas)
            {
                if (!item.IsPreloadReady)
                    return;
            }
            SetComponents();
            procedureOwner.SetData<VarInt>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        private void PreloadResources()
        {
            LoadConfig("DefaultConfig");

            // Preload data tables
            foreach (string dataTableName in DataTableNames)
            {
                LoadDataTable(dataTableName);
            }

            GameEntry.Data.PreLoadAllData();

            // Preload dictionaries
            //LoadDictionary("Default");

            // Preload fonts
            //            LoadFont("MainFont");
        }
        private void SetComponents()
        {
            SetDataComponent();
            SetUIComponent();
            SetEntityComponent();
            SetItemComponent();
            //SetSoundComponent();
        }
        private void SetDataComponent()
        {
            GameEntry.Data.LoadAllData();
        }

        private void SetUIComponent()
        {
            UIGroupData[] uiGroupDatas = GameEntry.Data.GetData<DataUI>().GetAllUIGroupData();
            foreach (var item in uiGroupDatas)
            {
                GameEntry.UI.AddUIGroup(item.Name, item.Depth);
            }
        }

        private void SetItemComponent()
        {
            ItemGroupData[] itemGroupDatas = GameEntry.Data.GetData<Data.DataItem>().GetAllItemGroupData();
            foreach (var item in itemGroupDatas)
            {
                PoolParamData poolParamData = item.PoolParamData;
                GameEntry.Item.AddItemGroup(item.Name, poolParamData.InstanceAutoReleaseInterval, poolParamData.InstanceCapacity, poolParamData.InstanceExpireTime, poolParamData.InstancePriority);
            }
        }

        private void SetEntityComponent()
        {
            EntityGroupData[] entityGroupDatas = GameEntry.Data.GetData<DataEntity>().GetAllEntityGroupData();
            foreach (var item in entityGroupDatas)
            {
                PoolParamData poolParamData = item.PoolParamData;
                GameEntry.Entity.AddEntityGroup(item.Name, poolParamData.InstanceAutoReleaseInterval, poolParamData.InstanceCapacity, poolParamData.InstanceExpireTime, poolParamData.InstancePriority);
            }
        }

       /* private void SetSoundComponent()
        {
            SoundGroupData[] soundGroupDatas = GameEntry.Data.GetData<DataSound>().GetAllSoundGroupData();
            foreach (var item in soundGroupDatas)
            {
                GameEntry.Sound.AddSoundGroup(item.Name, item.AvoidBeingReplacedBySamePriority, item.Mute, item.Volume, item.SoundAgentCount);
                GameEntry.Sound.SetVolume(item.Name, GameEntry.Setting.GetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, item.Name), 1));
            }
        }*/

        private void LoadConfig(string configName)
        {
            string configAssetName = AssetUtility.GetConfigAsset(configName, false);
            m_LoadedFlag.Add(configAssetName, false);
            GameEntry.Config.ReadData(configAssetName, this);
        }

        private void LoadDataTable(string dataTableName)
        {
            string dataTableAssetName = AssetUtility.GetDataTableAsset(dataTableName, false);
            m_LoadedFlag.Add(dataTableAssetName, false);
            GameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);
        }

        private void LoadDictionary(string dictionaryName)
        {
            string dictionaryAssetName = AssetUtility.GetDictionaryAsset(dictionaryName, false);
            m_LoadedFlag.Add(dictionaryAssetName, false);
            GameEntry.Localization.ReadData(dictionaryAssetName, this);
        }

        private void LoadFont(string fontName)
        {
            
        }

        private void OnLoadConfigSuccess(object sender, GameEventArgs e)
        {
            LoadConfigSuccessEventArgs ne = (LoadConfigSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[ne.ConfigAssetName] = true;
            Log.Info("Load config '{0}' OK.", ne.ConfigAssetName);
        }

        private void OnLoadConfigFailure(object sender, GameEventArgs e)
        {
            LoadConfigFailureEventArgs ne = (LoadConfigFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load config '{0}' from '{1}' with error message '{2}'.", ne.ConfigAssetName, ne.ConfigAssetName, ne.ErrorMessage);
        }

        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[ne.DataTableAssetName] = true;
            Log.Info("Load data table '{0}' OK.", ne.DataTableAssetName);
        }

        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {
            LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableAssetName, ne.DataTableAssetName, ne.ErrorMessage);
        }

        private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {
            LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[ne.DictionaryAssetName] = true;
            Log.Info("Load dictionary '{0}' OK.", ne.DictionaryAssetName);
        }

        private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
        {
            LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryAssetName, ne.DictionaryAssetName, ne.ErrorMessage);
        }
    }
}
