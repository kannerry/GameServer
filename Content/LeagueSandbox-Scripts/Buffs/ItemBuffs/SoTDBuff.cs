using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    internal class SoTDBuff : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_OVERLAPS;
        public int MaxStacks => 5;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        int aa = 0;
        IAttackableUnit _owner;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _owner = unit;
            ApiEventManager.OnHitUnit.AddListener(this, ownerSpell.CastInfo.Owner, TargetExecute, false);
            unit.Stats.CriticalChance.FlatBonus = 1.0f;
            StatsModifier.AttackSpeed.PercentBonus = 1.0f;
            unit.AddStatModifier(StatsModifier);
        }

        bool proc3 = false;

        public void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            aa++;
            if(aa == 3)
            {
                LogDebug("yo");
                _owner.Stats.CriticalChance.FlatBonus -= 1.0f;
                proc3 = true;
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
            if (proc3 == false)
            {
                unit.Stats.CriticalChance.FlatBonus -= 1.0f;
            }
            proc3 = false;
        }

        public void OnUpdate(float diff)
        {
        }
    }
}