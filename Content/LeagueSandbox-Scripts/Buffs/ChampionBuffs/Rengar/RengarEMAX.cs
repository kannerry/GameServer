using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class RengarEMAX : IBuffGameScript
    {
        public BuffType BuffType => BuffType.SNARE;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; }

        private IParticle buff1;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var caster = ownerSpell.CastInfo.Owner;
            unit.StopMovement();
            buff1 = AddParticleTarget(caster, unit, "Rengar_Base_E_Max_Tar.troy", unit, lifetime: 1.75f);
            unit.Stats.SetActionState(ActionState.CAN_MOVE, false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(buff1);
            unit.Stats.SetActionState(ActionState.CAN_MOVE, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}