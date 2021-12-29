using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class ShenWayOfTheNinjaMarker : ICharScript
    {

        IObjAiBase _owner;
        ISpell _spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            _spell = spell;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);
            AddBuff("PassiveCooldown", 9.0f, 1, _spell, _owner, _owner);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnHitUnit(IAttackableUnit unit, bool crit)
        {
            if (_owner.HasBuff("PassiveCooldown"))
            {
                LogDebug("hit with cooldown");
                var buff = _owner.GetBuffWithName("PassiveCooldown");
                buff.TimeElapsed += 1;
                if (_owner.HasBuff("ShenWShield"))
                {
                    buff.TimeElapsed += 1;
                }
            }
            if (_owner.HasBuff("ShenWayOfTheNinjaMarker"))
            {
                var basedamage = 4 + 4 * _owner.Stats.Level;
                var hpscaling = _owner.Stats.HealthPoints.TotalBonus * 0.1f;
                unit.TakeDamage(_owner, basedamage + hpscaling, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                _owner.RemoveBuffsWithName("ShenWayOfTheNinjaMarker");
                AddBuff("PassiveCooldown", 9.0f, 1, _spell, _owner, _owner);
            }
        }

        public void OnUpdate(float diff)
        {
            //if (_owner.HasBuff("PassiveCooldown"))
            //{
            //    var buff = _owner.GetBuffWithName("PassiveCooldown");
            //    LogDebug(buff.TimeElapsed.ToString());
            //}
        }
    }
}