using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3136 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner)
        {
            StatsModifier.MagicPenetration.FlatBonus += 10;
            owner.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IObjAiBase owner)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}