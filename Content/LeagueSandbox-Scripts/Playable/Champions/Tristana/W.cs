using System.Linq;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class RocketJump : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnFinishDash.AddListener(this, owner, deezNuts, false);
        }
        ISpell Spell;
        public void deezNuts(IAttackableUnit unit)
        {
            var spell = Spell;
            var owner = spell.CastInfo.Owner as IChampion;
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.8f;
            var damage = 70 * spell.CastInfo.SpellLevel + ap;
            var units = GetUnitsInRange(owner.Position, 350f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is IBaseTurret || units[i] is IObjBuilding || units[i] is IInhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    AddParticleTarget(owner, units[i], "tristana_rocketJump_unit_tar.troy", units[i], 1f);
                    AddParticleTarget(owner, owner, "RocketJump_cas.troy", owner, 1f);

                }
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Spell = spell;
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var coords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            owner.SetTargetUnit(null);
            ForceMovement(owner, "Spell3", coords, 1200, 0, 200, 0);
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.8f;
            var damage = 70*spell.CastInfo.SpellLevel  + ap;
            var coords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            owner.SetTargetUnit(null);
            if (Extensions.IsVectorWithinRange(owner.Position, coords, 900))
            {
                ForceMovement(owner, "Spell3", coords, 1200, 0, 50, 0);
            }
            else
            {
                ForceMovement(owner, "Spell3", GetPointFromUnit(owner, 900), 1200, 0, 50, 0);
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
        }
    }
}