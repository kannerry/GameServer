using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class JaxCounterStrike: ISpellScript
    {
        IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Owner = owner;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        static internal IParticle x;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
            var cd = (new float[] { 18f, 16f, 14f, 12f, 10f }[spell.CastInfo.SpellLevel - 1]);
            var owner = spell.CastInfo.Owner;
            PlayAnimation(owner, "Spell3", 2.0f);
            AddBuff("JaxCounterStrikeAttack", 2f, 1, spell, Owner, Owner, false); // REPROC COSTS MANA SO WE ADD THE COST BACK TO THE MANA POOL (so you can actually recast it :D)
            CreateTimer(0.1f, () => { owner.Spells[2].SetCooldown((float)0.1); owner.Stats.CurrentMana += 60;  });
            x = AddParticleTarget(owner, owner, "JaxDodger.troy", owner, 5f);
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