using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class RocketGrab2 : IBuffGameScript
    {
        public BuffType BuffType => BuffType.INTERNAL;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            buff.SetStatusEffect(StatusFlags.CanAttack | StatusFlags.CanMove | StatusFlags.CanCast, false);

            var xy = unit as IObjAiBase;
            xy.SetTargetUnit(null);

            ForceMovement(unit, "RUN", buff.SourceUnit.Position, 1800f, 0, 5.0f, 0, movementOrdersType: ForceMovementOrdersType.CANCEL_ORDER);
        }

        // TODO: Use OnMoveEnd event and call this manually.
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            buff.SetStatusEffect(StatusFlags.CanAttack | StatusFlags.CanMove | StatusFlags.CanCast, true);

            if (buff.SourceUnit is IObjAiBase ai && unit is IChampion ch)
            {
                ai.SetTargetUnit(ch, true);
            }
        }

        public void OnUpdate(float diff)
        {
        }
    }
}