using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using Spells;
namespace Buffs
{
    internal class AatroxPassiveDebuff : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IAttackableUnit _owner;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _owner = unit;
            unit.SetStatus(StatusFlags.CanCast, false);
            unit.SetStatus(StatusFlags.CanAttack, false);
            unit.SetStatus(StatusFlags.Targetable, false);
            StatsModifier.MoveSpeed.PercentBonus = StatsModifier.MoveSpeed.PercentBonus - 0.4f;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            unit.SetStatus(StatusFlags.CanCast, true);
            unit.SetStatus(StatusFlags.CanAttack, true);
            unit.SetStatus(StatusFlags.Targetable, true);
        }

        public void OnUpdate(float diff)
        {
            _owner.Stats.AttackSpeedFlat = (float)(_owner.Stats.CurrentMana * 0.25);
        }
    }
}