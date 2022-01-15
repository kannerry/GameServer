using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class GarenPassive : ICharScript
    {
        IObjAiBase ownermain;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ownermain = owner;
            AddBuff("GarenCooldown", 10.0f, 1, spell, owner, owner);
            ApiEventManager.OnTakeDamage.AddListener(this, owner, ResetPassive, false); 
        }

        public void ResetPassive(IAttackableUnit unit1, IAttackableUnit unit2)
        {
            if(!(unit2 is IMinion))
            {
                if (unit1.HasBuff("GarenHealing"))
                {
                    unit1.GetBuffWithName("GarenHealing").DeactivateBuff();
                }
                if (unit1.HasBuff("GarenCooldown"))
                {
                    unit1.GetBuffWithName("GarenCooldown").ResetTimeElapsed();
                }
            }
        }
        float timeSinceLastTick = 0;
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        private void PerformHeal(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            var ap = owner.Stats.HealthPoints.Total * 0.04f;
            float healthGain = ap;
            if (target.HasBuff("HealCheck"))
            {
                healthGain *= 0.5f;
            }
            var newHealth = target.Stats.CurrentHealth + healthGain;
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);
        }

        public void OnUpdate(float diff)
        {
            if (ownermain.HasBuff("GarenHealing"))
            {
                LogDebug("yo");
                timeSinceLastTick += diff;
                if (timeSinceLastTick >= 1100f)
                {
                    PerformHeal(ownermain, ownermain.GetSpell(0), ownermain);
                    timeSinceLastTick = 0;
                }
            }
        }
    }
}