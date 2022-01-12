using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace Buffs
{
    internal class ShenTaunt : IBuffGameScript
    {
        public BuffType BuffType => BuffType.TAUNT;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IAttackableUnit _owner;
        IChampion unitChamp;
        bool forceAttack = false;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _owner = ownerSpell.CastInfo.Owner;
            if(unit is IChampion)
            {
                unitChamp = unit as IChampion;
                forceAttack = true;
                unitChamp.SetStatus(StatusFlags.CanCast, false);
            }
            // ApplyAssistMarker
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (unit is IChampion)
            {
                var unitChamp = unit as IChampion;
                unitChamp.SetTargetUnit(null);
                unitChamp.SetStatus(StatusFlags.CanCast, true);
                forceAttack = false;
            }
        }

        public void OnUpdate(float diff)
        {
            if(unitChamp != null)
            {
                if(forceAttack == true)
                {
                    unitChamp.SetTargetUnit(_owner);
                }
            }
        }
    }
}