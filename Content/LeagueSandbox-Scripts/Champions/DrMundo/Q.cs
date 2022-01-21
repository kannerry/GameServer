using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class InfectedCleaverMissileCast : ISpellScript
    {
        private IObjAiBase Owner;

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Owner = owner;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var targetPos = GetPointFromUnit(owner, 975f);
            float SelfDamage = 40 + 10f * spell.CastInfo.SpellLevel;

            FaceDirection(targetPos, owner);
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
            owner.StopMovement();
            owner.SetTargetUnit(null);
            if (owner.Stats.CurrentHealth > SelfDamage)
            {
                owner.Stats.CurrentHealth -= SelfDamage;
            }
            else
            {
                owner.Stats.CurrentHealth = 1f;
            }
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            if (Owner != null)
            {
                Owner.Stats.HealthRegeneration.FlatBonus = (Owner.Stats.HealthPoints.Total / 100f) * 0.3f;
                //Double Check if that's right
            }
        }
    }

    public class InfectedCleaverMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
            // TODO
        };

        //Vector2 direction;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        private void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector arg4)
        {
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("InfectedCleaverMissileCast").CastInfo.SpellLevel;
            var damage = target.Stats.CurrentHealth * (0.12f + 0.03f * spellLevel);
            float minimunDamage = 30f + (50f * spellLevel);
            float maxDamageMonsters = 200 + 100f * spellLevel;
            float Heal = 20 + 5f * spellLevel;
            //TODO: Implement max damage when monsters gets added.

            if (damage < minimunDamage)
            {
                damage = minimunDamage;
            }

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            owner.Stats.CurrentHealth += Heal;
            AddParticleTarget(owner, null, "dr_mundo_as_mundo_infected_cleaver_tar.troy", target);
            AddParticleTarget(owner, null, "dr_mundo_infected_cleaver_tar.troy", target);
            AddBuff("InfectedCleaverMissile", 2f, 1, spell, target, owner);

            missile.SetToRemove();
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}