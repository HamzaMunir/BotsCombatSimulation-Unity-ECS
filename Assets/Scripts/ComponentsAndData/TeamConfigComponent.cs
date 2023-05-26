using System.Collections;
using System.Collections.Generic;
using ECSBattleSimulation.Scriptable;
using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Components
{
    
    public struct FriendlyTeamConfigComponent : IComponentData
    {
        public uint Rows;
        public uint Columns;
        public BlobAssetReference<SpawnableTeamBlobAsset> TeamData;
    }
    public struct EnemyTeamConfigComponent : IComponentData
    {
        public uint Rows;
        public uint Columns;
        public BlobAssetReference<SpawnableTeamBlobAsset> TeamData;
    }
}