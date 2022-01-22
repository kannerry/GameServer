using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class Isolation : IBuffGameScript
    {
        public BuffType BuffType => BuffType.INTERNAL;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //LogDebug("proc");
            AddParticle(ownerSpell.CastInfo.Owner, unit, "Khazix_Base_Q_SingleEnemyPOV_Indicator.troy", unit.Position, buff.Duration + 0.1f, bone: "hip");
            //AddParticle(unit, unit, "Khazix_Base_Q_SingleEnemyPOV_Indicator.troy", unit.Position, buff.Duration, bone: "head", reqVision: false, size: 5);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}