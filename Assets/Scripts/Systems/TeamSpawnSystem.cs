using System;
using ECSBattleSimulation.Authoring;
using ECSBattleSimulation.Components;
using ECSBattleSimulation.Scriptable;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECSBattleSimulation.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(DestroyCharactersSystem))]
    public partial struct TeamSpawnSystem : ISystem
    {
        private enum E_CharacterType
        {
            Friend,
            Enemy
        }

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.Enabled = false;
            state.RequireForUpdate<CharacterAssetComponent>();
            state.RequireForUpdate<FriendlyTeamConfigComponent>();
            state.RequireForUpdate<EnemyTeamConfigComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var spawnerConfig = SystemAPI.GetSingletonEntity<SpawnerConfig>();
            var friendlyTeamConfig =SystemAPI.GetComponentRO<FriendlyTeamConfigComponent>(spawnerConfig);
            var enemyTeamConfig =SystemAPI.GetComponentRO<EnemyTeamConfigComponent>(spawnerConfig);
            
            //Spawn Teams from loaded data
            var friendlyEntities = SpawnTeam(ref state, E_CharacterType.Friend, ref ecb,
                friendlyTeamConfig.ValueRO.TeamData.Value.UnitsData.Length, friendlyTeamConfig.ValueRO.Rows,
                friendlyTeamConfig.ValueRO.Columns);
            var enemyEntities = SpawnTeam(ref state, E_CharacterType.Enemy, ref ecb,
                enemyTeamConfig.ValueRO.TeamData.Value.UnitsData.Length, enemyTeamConfig.ValueRO.Rows, enemyTeamConfig.ValueRO.Columns);

            for (int i = 0; i < friendlyEntities.Length; i++)
            {
                ApplyComponents(ref state, friendlyEntities[i], ref ecb,
                    ref friendlyTeamConfig.ValueRO.TeamData.Value.UnitsData[i], E_CharacterType.Friend);
            }
            
            for (int i = 0; i < enemyEntities.Length; i++)
            {
                ApplyComponents(ref state, enemyEntities[i], ref ecb, ref enemyTeamConfig.ValueRO.TeamData.Value.UnitsData[i],
                    E_CharacterType.Enemy);
            }

            ecb.Playback(state.EntityManager);
            state.WorldUnmanaged.GetExistingSystemState<UpdateHealthSystem>().Enabled = true;
            
        }

        [BurstCompile]
        private NativeList<Entity> SpawnTeam(ref SystemState state, E_CharacterType characterType,
            ref EntityCommandBuffer ecb, int length, uint rows,
            uint columns)
        {
            var ground = SystemAPI.GetSingleton<GroundData>();
            var characterAsset = SystemAPI.GetSingleton<CharacterAssetComponent>();
            Entity entityPrefab;
            float3 offset;
            float3 rotation;
            if (characterType == E_CharacterType.Friend)
            {
                offset = ground.TeamAOrigin;
                rotation = ground.TeamARotation;
                entityPrefab = characterAsset.FriendlyEntity;
            }
            else
            {
                offset = ground.TeamBOrigin;
                rotation = ground.TeamBRotation;
                entityPrefab = characterAsset.EnemyEntity;
            }

            NativeList<Entity> spawnedEntities =
                new NativeList<Entity>(length, Allocator.Persistent);

            for (int i = 0; i < rows; i++)
            {
                bool hasLimitReached = false;
                for (int j = 0; j < columns; j++)
                {
                    int dataIndex = i * (int)rows + j;
                    if (dataIndex >= length)
                    {
                        hasLimitReached = true;
                        break;
                    }

                    var newPosition = (new float3(i, 0, j) * ground.Spread) + offset;
                    var newEntity = ecb.Instantiate(entityPrefab);
                    ecb.SetComponent(newEntity, GetTransform(newPosition, rotation));
                    spawnedEntities.Add(newEntity);
                }

                if (hasLimitReached)
                {
                    break;
                }
            }

            return spawnedEntities;
        }

        [BurstCompile]
        private void ApplyComponents(ref SystemState state, Entity entity, ref EntityCommandBuffer ecb,
            ref CharacterProperties entityProperties, E_CharacterType type)
        {
            ecb.SetComponent(entity, new CharacterAttackDamage()
            {
                AttackDamage = entityProperties.AttackDamage
            });
            ecb.SetComponent(entity, new CharacterHealthPoints()
            {
                HealthPoints = entityProperties.HealthPoints,
                CurrentHealth = entityProperties.HealthPoints
            });
            ecb.SetComponent(entity, new CharacterMovementSpeed()
            {
                MovementSpeed = entityProperties.MovementSpeed
            });
            ecb.SetComponent(entity, new CharacterAttackRange()
            {
                AttackRange = entityProperties.AttackRange
            });
            ecb.SetComponent(entity, new CharacterAttackTime()
            {
                AttackSpeed = entityProperties.AttackSpeed,
                Cooldown = entityProperties.AttackSpeed
            });
            ecb.AddComponent(entity, new TeamComponent()
            {
                Team = type == E_CharacterType.Enemy ? Team.Enemy : Team.Friendly
            });
            ecb.AddComponent(entity, new CharacterTarget());
        }

        private LocalTransform GetTransform(float3 position, float3 rotation)
        {
            return LocalTransform.FromPositionRotation(position, quaternion.Euler(rotation));
        }
    }
}