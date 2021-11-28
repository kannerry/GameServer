using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Linq;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class KatarinaR : ISpellScript
    {
        bool cancelled;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };
        Vector2 basepos;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            CreateTimer(3.0f, () => { cancelled = false; });
            CreateTimer(2.5f, () => { owner.PlayAnimation("IDLE1", 1); owner.StopAnimation("IDLE1"); });
            basepos = owner.Position;
            for (var i = 0.0f; i < 2.5; i += 0.25f)
            {
                CreateTimer(i, () => { ApplySpinDamage(owner, spell, target); });
            }
        }

        private void ApplySpinDamage(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            if (owner.Position.X != basepos.X)
            {
                cancelled = true;
            }
            if (owner.Position.Y != basepos.Y)
            {
                cancelled = true;
            }
            var units = GetUnitsInRange(owner.Position, 500, true);
            foreach (var unit in units)
            {
                if (unit.Team != owner.Team)
                {
                    var damage = 35.0f;
                    var ap = owner.Stats.AbilityPower.Total * 0.25f;
                    var ad = owner.Stats.AttackDamage.Total * 0.37f;
                    if (unit is Minion) damage *= 0.75f;
                    if (!cancelled)
                    {
                        unit.TakeDamage(owner, ap + ad + damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
            }
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

