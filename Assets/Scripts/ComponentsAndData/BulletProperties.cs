using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECSBattleSimulation.Components
{
    public struct BulletProperties : IComponentData
    {
        public float3 TargetPosition;
        public Entity TargetEntity;
        public Entity OriginEntity;
        public int Damage;
    }
}