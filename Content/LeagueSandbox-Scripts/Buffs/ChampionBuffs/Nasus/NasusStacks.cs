using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace Buffs
{
    internal class NasusQStacks : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COUNTER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_RENEWS;
        public int MaxStacks => 10000;
        public bool IsHidden => false;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}