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
    public class JarvanIVCataclysm : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
        ISpell _spell;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _spell = spell;
            ApiEventManager.OnFinishDash.AddListener(this, owner, terrain, false);
        }
        public void terrain(IAttackableUnit unit)
        {
            if (JarvanIVDragonStrike.isQing != true)
            {
                var spell = _spell;
                var dmg = spell.CastInfo.Owner.Stats.AttackDamage.Total * 1.8f + (125 * spell.CastInfo.SpellLevel) + 100;
                FaceDirection(new Vector2(spell.CastInfo.Targets[0].Unit.Position.X + 1, spell.CastInfo.Targets[0].Unit.Position.Y + 1), spell.CastInfo.Targets[0].Unit);
                spell.CastInfo.Targets[0].Unit.TakeDamage(spell.CastInfo.Owner, dmg, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                for (int bladeCount = 0; bladeCount <= 7; bladeCount++)
                {
                    var x = AddMinion(spell.CastInfo.Owner, "JarvanIVWall", "Wall", GetPointFromUnit(spell.CastInfo.Targets[0].Unit, 250, bladeCount * 45));
                    x.SetCollisionRadius(90);
                    CreateTimer(3.5f, () => { x.TakeDamage(spell.CastInfo.Owner, 100000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false); });
                }
            }
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
            spell.CastInfo.Owner.SetTargetUnit(null);
            ForceMovement(spell.CastInfo.Owner, "Spell4", spell.CastInfo.Targets[0].Unit.Position, 1500, 0, 150, 0);
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