using System;
using ECSBattleSimulation.Components;
using ECSBattleSimulation.Scriptable;
using UnityEngine;

namespace ECSBattleSimulation.Mono.UiMonoBehaviours.Data
{
    public class StartGameUiData : UiBaseData
    {
        public MatchConfigData MatchConfig;
        public Action<TeamSpawnData> OnStartGame;
        public Action<TeamSpawnData, Team> OnTeamSelected;
    }
}