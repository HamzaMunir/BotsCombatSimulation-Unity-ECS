using ECSBattleSimulation.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace ECSBattleSimulation.Authoring
{
    public class GroundPropertiesAuthoring : MonoBehaviour
    {
        public float3 TeamAOrigin;
        public float3 TeamBOrigin;
        public float3 TeamARotation;
        public float3 TeamBRotation;
        public float Spread;
        public uint RandomSeed;

        public class GroundPropertiesBaker : Baker<GroundPropertiesAuthoring>
        {
            public override void Bake(GroundPropertiesAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new GroundData()
                {
                    TeamAOrigin = authoring.TeamAOrigin,
                    TeamBOrigin = authoring.TeamBOrigin,
                    TeamARotation = authoring.TeamARotation,
                    TeamBRotation = authoring.TeamBRotation,
                    Spread = authoring.Spread
                });
                AddComponent(entity, new RandomComponent()
                {
                    Value = Random.CreateFromIndex(authoring.RandomSeed)
                });
            }
        }
    }
}