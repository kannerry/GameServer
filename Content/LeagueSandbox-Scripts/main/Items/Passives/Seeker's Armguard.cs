using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace ItemPassives
{
    public class ItemID_3191 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase ItemOwner;
        private int stacks = 0;

        public void OnActivate(IObjAiBase owner)
        {
            ItemOwner = owner;
            ApiEventManager.OnKillUnit.AddListener(this, owner, TargetExecute, false);
        }

        private void TargetExecute(IDeathData obj)
        {
            if (stacks != 30)
            {
                StatsModifier.AbilityPower.FlatBonus = (float)0.5;
                StatsModifier.Armor.FlatBonus = (float)0.5;
                ItemOwner.AddStatModifier(StatsModifier);
                stacks++;
            }
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