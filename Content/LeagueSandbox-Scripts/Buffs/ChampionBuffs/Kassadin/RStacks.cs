using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
namespace Buffs
{
    internal class RiftWalk : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_RENEWS;
        public int MaxStacks => 4;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IChampion _owner;
        ISpell _spell;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _spell = ownerSpell;
            var owner = ownerSpell.CastInfo.Owner as IChampion;
            _owner = owner;
            owner.GetSpell("RiftWalk").SpellData.ManaCost[1] *= 2f;
            LogDebug(owner.GetSpell("RiftWalk").SpellData.ManaCost[1].ToString());
        }

        public void lowerstacks()
        {
            var owner = _spell.CastInfo.Owner as IChampion;
            owner.GetSpell("RiftWalk").SpellData.ManaCost[1] *= 0.5f;
            LogDebug(owner.GetSpell("RiftWalk").SpellData.ManaCost[1].ToString());
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            int i = 0;
            while (i++ < buff.StackCount)
            {
                lowerstacks();
            }
        }

        public void OnUpdate(float diff)
        {
        }
    }
}