using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;

namespace Buffs
{
    internal class JaxCounterStrikeAttack : IBuffGameScript
    {
        public BuffType BuffType => BuffType.INTERNAL;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => true;

        public IStatsModifier StatsModifier { get; private set; }

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ((IObjAiBase)unit).SetSpell("JaxCounterStrikeAttack", 2, true);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (((IObjAiBase)unit).Spells[2].SpellName == "JaxCounterStrikeAttack")
            {
                ((IObjAiBase)unit).SetSpell("JaxCounterStrike", 2, true);
            }
        }

        public void OnUpdate(float diff)
        {
            //nothing!
        }
    }
}