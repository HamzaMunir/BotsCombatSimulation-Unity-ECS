using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Components
{
    public struct CharacterHealthPoints : IComponentData
    {
        public int HealthPoints;
        public int CurrentHealth;
    }

    public struct CharacterAttackDamage : IComponentData
    {
        public int AttackDamage;
    }

    public struct CharacterMovementSpeed : IComponentData
    {
        public int MovementSpeed;
    }

    public struct CharacterAttackRange : IComponentData
    {
        public int AttackRange;
    }
    public struct CharacterAttackTime: IComponentData
    {
        public float AttackSpeed;
        public float Cooldown;
    }

    public struct DealAttackDamage : IComponentData
    {
        public Entity Origin;
        public uint AttackDamage;
    }

    public struct CharacterTarget : IComponentData
    {
        public Entity Target;
        public bool IsTargetLocked;
    }

    public enum Team
    {
        Friendly,
        Enemy
    }
    public struct TeamComponent : IComponentData
    {
        public Team Team;
    }
    
    public struct FriendlyTag : IComponentData{}
    
    public struct EnemyTag : IComponentData{}

    public struct CharacterRotation : IComponentData
    {
        public float Rotation;
    }
    
}