using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECSBattleSimulation.Components
{
    public struct GroundData : IComponentData
    {
        public float3 TeamAOrigin;
        public float3 TeamBOrigin;
        public float3 TeamARotation;
        public float3 TeamBRotation;
        public float Spread;
    }

    public struct FriendlyTeamBlobReference : IComponentData
    {
        public BlobAssetReference<SpawnedCharacters> TeamBlobReference;
    }
    public struct EnemyTeamBlobReference : IComponentData
    {
        public BlobAssetReference<SpawnedCharacters> TeamBlobReference;
    }
    public struct SpawnedCharacters
    {
        public BlobArray<Entity> Team;
    }
}