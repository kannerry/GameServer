using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class AnnieStun : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_OVERLAPS;
        public int MaxStacks => 5;
        public bool IsHidden => false;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IParticle p;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            LogDebug(unit.GetBuffWithName("AnnieStun").StackCount.ToString());
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
