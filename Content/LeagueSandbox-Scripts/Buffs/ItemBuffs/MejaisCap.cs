using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;

namespace Buffs
{
    internal class MejaisCap : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_OVERLAPS;
        public int MaxStacks => 25;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();



        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if(ItemPassives.ItemID_3141.occult == true)
            {
                StatsModifier.AttackDamage.FlatBonus += 5;
            }
            else
            {
                StatsModifier.AbilityPower.FlatBonus += 8;
            }
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
