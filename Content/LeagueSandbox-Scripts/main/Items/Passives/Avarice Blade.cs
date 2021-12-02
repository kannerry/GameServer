using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace ItemPassives
{
    public class ItemID_3093 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase itemOwner;

        public void OnActivate(IObjAiBase owner)
        {
            itemOwner = owner;
            ApiEventManager.OnKillUnit.AddListener(this, owner, TargetExecute, false);
        }

        private void TargetExecute(IDeathData obj)
        {
            itemOwner.Stats.Gold += 2;
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnKillUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}