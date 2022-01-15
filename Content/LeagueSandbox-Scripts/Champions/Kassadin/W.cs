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
    public class NetherBlade : ISpellScript
    {
        private IObjAiBase Owner;
        private IAttackableUnit Target;

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
        IObjAiBase own;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            own = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        public void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            if (own.GetSpell(1).CastInfo.SpellLevel != 0)
            {
                var ap = own.Stats.AbilityPower.Total * 0.1f;
                float damage = 20 + ap;
                unit.TakeDamage(own, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            owner.CancelAutoAttack(true);
            Target = target;
            Owner = owner;
        }

        public void OnSpellCast(ISpell spell)
        {
            AddBuff("EStacks", float.MaxValue, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner, true);
            AddBuff("NetherBlade", 6f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
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