using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class WBuff : IBuffGameScript
    {
        public BuffType BuffType => BuffType.HEAL;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "TeemoSmoke_cas.troy", unit, 2);
            StatsModifier.MoveSpeed.PercentBonus = 0.55f;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
