using ECSBattleSimulation.Authoring;
using ECSBattleSimulation.Components;
using ECSBattleSimulation.Mono;
using ECSBattleSimulation.Scriptable;
//using ECSBattleSimulation.Scriptable;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Systems
{
    [BurstCompile]
    public partial class ReadInitialDataSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Enabled = false;
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            Enabled = false;

            Entities.WithAll<SpawnerConfig>().ForEach(
                (ref FriendlyTeamConfigComponent friendlyTeam,
                    ref EnemyTeamConfigComponent enemyTeamConfig) =>
                {
                    friendlyTeam.Rows = DataManager.Instance.CurrentSelectedFriendlyConfig.Rows;
                    friendlyTeam.Columns = DataManager.Instance.CurrentSelectedFriendlyConfig.Columns;
                    friendlyTeam.TeamData = DataManager.Instance.CurrentSelectedFriendlyConfig.Convert();

                    enemyTeamConfig.Rows = DataManager.Instance.CurrentSelectedEnemyConfig.Rows;
                    enemyTeamConfig.Columns = DataManager.Instance.CurrentSelectedEnemyConfig.Columns;
                    enemyTeamConfig.TeamData = DataManager.Instance.CurrentSelectedEnemyConfig.Convert();
                    
                }).WithoutBurst().Run();
            
            EntityManager.WorldUnmanaged.GetExistingSystemState<DestroyCharactersSystem>().Enabled = true;
        }
    }
}