using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
// Kindlegem
namespace ItemPassives
{
    public class ItemID_3046 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IObjAiBase owner)
        {
            owner.SetStatus(GameServerCore.Enums.StatusFlags.Ghosted, true);
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            owner.SetStatus(GameServerCore.Enums.StatusFlags.Ghosted, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
