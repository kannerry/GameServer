using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class RodOfAgesCooldown : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_OVERLAPS;
        public int MaxStacks => 10;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //AddBuff("ShenKiAttack", float.MaxValue, 1, ownerSpell, unit, unit as IObjAiBase, true);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //unit.RemoveBuffsWithName("ShenKiAttack");
            LogDebug("appliedrod");
            AddBuff("RodOfAgesCounter", float.MaxValue, 1, ownerSpell, unit, unit as IObjAiBase, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}