using LeagueSandbox.GameServer.API;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
namespace Spells
{
    public class JaxLeapStrike : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            //TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            ApiEventManager.OnFinishDash.AddListener(this, owner, deezNutsJax, false);
        }

        public void deezNutsJax(IAttackableUnit unit)
        {
            var spell = _owner.GetSpell("JaxLeapStrike");
            var owner = spell.CastInfo.Owner;
            var APratio = owner.Stats.AbilityPower.Total * 0.6f;
            var ADratio = owner.Stats.AttackDamage.FlatBonus;
            var damage = 70 * spell.CastInfo.SpellLevel + ADratio + APratio;
            if(unit.Team != owner.Team)
            {
                AddParticleTarget(owner, Target, "jax_leapstrike_tar.troy", Target, 1f, 1f);
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var to = Vector2.Normalize(target.Position - owner.Position);
            owner.SetTargetUnit(null);
            ForceMovement(owner, "Spell2", new Vector2(target.Position.X - to.X * 100f, target.Position.Y - to.Y * 100f), 1000, 0, 65, 0);
            Target = target;
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var target = spell.CastInfo.Targets[0].Unit;
            var to = Vector2.Normalize(target.Position - owner.Position);
            owner.SetTargetUnit(null);
            ForceMovement(owner, "Spell2", new Vector2(target.Position.X - to.X * 100f, target.Position.Y - to.Y * 100f), 1000, 0, 65, 0);

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