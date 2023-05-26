using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Components
{
    public struct CharacterAssetComponent : IComponentData
    {
        public Entity FriendlyEntity;
        public Entity EnemyEntity;
    }
}