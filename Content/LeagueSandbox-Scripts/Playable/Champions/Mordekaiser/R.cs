using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class MordekaiserChildrenOfTheGrave : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true
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
            AddBuff("MordekaiserChildrenOfTheGrave", 40.4f, 1, spell, target, spell.CastInfo.Owner);
            CreateTimer(10.0f, () =>
            {
                if (target.IsDead)
                {
                    LogDebug("dead");
                }
                if (!(target.IsDead))
                {
                    LogDebug("dead not");
                    target.RemoveBuffsWithName("MordekaiserChildrenOfTheGrave");
                }
            });
            target.TakeDamage(owner, 500, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_RAW, false);
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

        public void TargetExecute(IAttackableUnit unit, bool isCrit)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}