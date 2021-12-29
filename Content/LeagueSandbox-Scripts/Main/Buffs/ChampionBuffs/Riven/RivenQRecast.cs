using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class RivenQRecast : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_RENEWS;
        public int MaxStacks => 3;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            LogDebug("activate");
            if (unit.HasBuff("RivenQRecast"))
            {
                var getbuff = unit.GetBuffWithName("RivenQRecast");
                LogDebug(getbuff.StackCount.ToString());
                if (getbuff.StackCount < 3)
                {
                    ownerSpell.SetCooldown(0.5f, true);
                }
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            LogDebug("deactivate");
        }

        public void OnUpdate(float diff)
        {
        }
    }
}