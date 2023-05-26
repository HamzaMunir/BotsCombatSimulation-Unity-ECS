using ECSBattleSimulation.Mono.UiMonoBehaviours.Data;
using UnityEngine;

namespace ECSBattleSimulation.Mono.UiMonoBehaviours
{
    public abstract class UiBase : MonoBehaviour
    {
        private UiBaseData _data;
        protected UiBaseData Data => _data;

        public virtual void SetData(UiBaseData data)
        {
            _data = data;
            RefreshViewInternal();
        }

        private void RefreshViewInternal()
        {
            Show();
        }
        protected abstract void RefreshView();
        public void Show()
        {
            this.gameObject.SetActive(true);
            RefreshView();
        }

        public void Hide()
        {
            ResetView();
            this.gameObject.SetActive(false);
        }

        protected abstract void ResetView();
    }
    
    
}