using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class KarthusLayWasteA1 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            AddParticleTarget(owner, owner, "Karthus_Base_Q_Hand_Glow.troy", owner, bone: "R_Hand");
            AddParticle(owner, null, "Karthus_Base_Q_Point.troy", spellPos, lifetime: 1.6f);
            AddParticle(owner, null, "Karthus_Base_Q_Ring.troy", spellPos, lifetime: 1.6f);
            AddParticle(owner, null, "Karthus_Base_Q_Skull_Child.troy", spellPos, lifetime: 1.6f);
        }

        public void OnSpellPostCast(ISpell spell)
        {
            CreateTimer(0.55f, () =>
            {
                var owner = spell.CastInfo.Owner;
                var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
                IGameObject m = AddParticle(owner, null, "Karthus_Base_Q_Explosion.troy", spellPos);
                var affectedUnits = GetUnitsInRange(m.Position, 200, true);
                var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total;
                var damage = 20f + spell.CastInfo.SpellLevel * 20f + ap * 0.3f;

                if (affectedUnits.Count == 0)
                {
                    AddParticle(owner, null, "Karthus_Base_Q_Hit_Miss.troy", spellPos);
                }
                foreach (var unit in affectedUnits
                .Where(x => x.Team == CustomConvert.GetEnemyTeam(spell.CastInfo.Owner.Team)))
                {
                    if (unit is IChampion || unit is IMinion)
                    {
                        if (affectedUnits.Count == 1)
                        {
                            damage *= 2;
                            AddParticle(owner, null, "Karthus_Base_Q_Hit_Single.troy", spellPos);
                            unit.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, true);
                        }
                        if (affectedUnits.Count > 1)
                        {
                            AddParticle(owner, null, "Karthus_Base_Q_Hit_Many.troy", spellPos);
                            unit.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        }
                    }
                }
                m.SetToRemove();
                AddParticle(owner, null, "Karthus_Base_Q_Explosion_Sound.troy", spellPos);
            });
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

        public void OnUpdate(float diff)
        {
        }
    }
}