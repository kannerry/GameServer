using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class ZedPassiveToolTip : ICharScript
    {
        private ISpell originspell;
        private IObjAiBase ownermain;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnHitUnit.AddListener(this, owner, PassiveAtk, false);
            originspell = spell;
            ownermain = owner;
        }

        public void PassiveAtk(IAttackableUnit unit, bool crit)
        {
            if(unit.Stats.CurrentHealth <= unit.Stats.HealthPoints.Total * 0.6f)
            {
                if (!unit.HasBuff("ZedPassiveCD"))
                {
                    var dmg = unit.Stats.HealthPoints.Total * 0.08f;
                    unit.TakeDamage(ownermain, dmg, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, crit);
                    AddBuff("ZedPassiveCD", 10.0f, 1, originspell, unit, ownermain);
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            //LogDebug("yo");
        }
    }
}