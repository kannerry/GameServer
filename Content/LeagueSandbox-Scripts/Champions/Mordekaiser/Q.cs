using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class MordekaiserMaceOfSpades : ISpellScript
    {
        private IObjAiBase Owner;
        private IBuff Buff;
        private ISpell Spell;
        private string particles = "mordakaiser_maceOfSpades_tar.troy";

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = false,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnHitUnit.AddListener(this, spell.CastInfo.Owner, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        bool proc = false;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
            Spell = spell;
            //spell.CastInfo.Owner.CancelAutoAttack(true);
            Buff = AddBuff("MordekaiserMaceOfSpades", 10.0f, 1, spell, owner, owner);
            proc = true;
            //TODO: Make it so it doesn't deal the Physical damage from normal Auto Attack
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

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        int ix = 0;

        int i;

        public void TargetExecute(IAttackableUnit unit, bool isCrit)
        {
            if (proc == true)
            {
                ix++;
                var ADratio = Owner.Stats.AttackDamage.FlatBonus;
                var APratio = Owner.Stats.AbilityPower.Total * 0.4f;
                var damage = 80f + (30 * (Spell.CastInfo.SpellLevel - 1)) + ADratio + APratio;
                isCrit = false;

                AddParticleTarget(Owner, Owner, "mordakaiser_siphonOfDestruction_self.troy", Owner, 1f);

                var units = GetUnitsInRange(Owner.Position, 300f, true);
                for (i = units.Count - 1; i >= 0; i--)
                {
                    if (units[i].Team == Owner.Team || units[i] is IBaseTurret || units[i] is INexus || units[i] is IObjBuilding || units[i] is ILaneTurret)
                    {
                        units.RemoveAt(i);
                    }
                }
                for (i = 0; i < units.Count; i++)
                {
                    if ((units.Count) == 1)
                    {
                        damage *= 1.65f;
                        isCrit = true;
                        particles = "mordakaiser_maceOfSpades_tar2.troy";
                    }
                    units[i].TakeDamage(Owner, damage / 3, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, isCrit);
                    if (ix == 3)
                    {
                        units[i].TakeDamage(Owner, damage / 3, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, isCrit);
                    }
                    AddParticleTarget(Owner, Owner, particles, units[i], 1f);
                }
                if (ix == 3)
                {
                    ix = 0;
                    proc = false;
                }
            }
        }

        public void OnUpdate(float diff)
        {
        }
    }
}