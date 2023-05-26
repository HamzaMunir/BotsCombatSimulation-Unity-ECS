using System;
using ECSBattleSimulation.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(TargetLockSystem))]
    public partial class GameResultSystem : SystemBase
    {
        protected override void OnCreate()
        {
            Enabled = false;
        }

        public Action<Team> OnGameResult;
        [BurstCompile]
        protected override void OnUpdate()
        {
            EntityQuery friendlyUnitsQuery = EntityManager.CreateEntityQuery(typeof(FriendlyTag));
            EntityQuery enemyUnitsQuery = EntityManager.CreateEntityQuery(typeof(EnemyTag));
            NativeArray<Entity> friendlyUnits = friendlyUnitsQuery.ToEntityArray(Allocator.TempJob);
            NativeArray<Entity> enemyUnits = enemyUnitsQuery.ToEntityArray(Allocator.TempJob);

            if (friendlyUnits.Length == 0 && enemyUnits.Length == 0)
            {
                return;
            }
            if (friendlyUnits.Length <= 0)
            {
                OnGameResult?.Invoke(Team.Enemy);
                Enabled = false;
                EntityManager.WorldUnmanaged.GetExistingSystemState<TargetLockSystem>().Enabled = false;
            }
            
            if (enemyUnits.Length <= 0)
            {
                OnGameResult?.Invoke(Team.Friendly);
                Enabled = false;
                EntityManager.WorldUnmanaged.GetExistingSystemState<TargetLockSystem>().Enabled = false;
            }
        }
    }
}