using LeagueSandbox.GameServer.API;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;

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
                if(Spells.JaxCounterStrikeAttack.HasReprocced == false)
                {
                    ((IObjAiBase)unit).GetSpell(2).Cast(unit.Position, unit.Position);
                    ((IObjAiBase)unit).GetSpell(2).FinishCasting();
                    //unit.SetDashingState(ForceMovementState.NOT_DASHING);
                }
                else
                {
                    Spells.JaxCounterStrikeAttack.HasReprocced = false;
                }
                ((IObjAiBase)unit).SetSpell("JaxCounterStrike", 2, true);
            }
        }

        public void OnUpdate(float diff)
        {
            //nothing!
        }
    }
}