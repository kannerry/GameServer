using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ShenQMarker : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IAttackableUnit _shen;
        ISpell _spell;

        IParticle particle1;
        IParticle particle2;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            particle1 = AddParticle(_shen, unit, "shen_vorpalStar_tar.troy", unit.Position, lifetime: 5.0f);
            particle2 = AddParticle(_shen, unit, "shen_life_tap_tar_02.troy", unit.Position, lifetime: 5.0f);
            _shen = ownerSpell.CastInfo.Owner;
            _spell = ownerSpell;
            ApiEventManager.OnTakeDamage.AddListener(this, unit, ShenHealing, false);
        }

        public void ShenHealing(IAttackableUnit damageTaker, IAttackableUnit damageDealer)
        {
            AddBuff("ShenHealing", 3.0f, 1, _spell, damageDealer, damageDealer as IObjAiBase);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnTakeDamage.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}