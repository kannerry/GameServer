using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;

namespace Spells
{
    public class KassadinBasicAttack2 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
            Random teste = new Random();
            int numero = teste.Next(1, 2);
            if (numero == 1)
            {
                spell.CastInfo.Owner.SetAutoAttackSpell("KassadinBasicAttack", false);
            }
            else
            {
                spell.CastInfo.Owner.SetAutoAttackSpell($"KassadinBasicAttack{numero + 1}", false);
            }
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void TargetExecute(IAttackableUnit target, bool Iscrit)
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