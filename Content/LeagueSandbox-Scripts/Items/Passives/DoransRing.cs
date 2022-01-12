using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace ItemPassives
{
    public class ItemID_1056 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IObjAiBase owner)
        {
            ApiEventManager.OnKillUnit.AddListener(this, owner, TargetExecute, false);
            StatsModifier.ManaRegeneration.BaseBonus += 0.6f;
            owner.AddStatModifier(StatsModifier);
        }

        public void TargetExecute(IDeathData deathData)
        {
            deathData.Killer.Stats.CurrentMana += 4;
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