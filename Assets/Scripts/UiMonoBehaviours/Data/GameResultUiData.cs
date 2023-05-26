using System;
using ECSBattleSimulation.Components;

namespace ECSBattleSimulation.Mono.UiMonoBehaviours.Data
{
    public class GameResultUiData : UiBaseData
    {
        public Team team;
        public Action OnRestartButtonClicked;
    }
}