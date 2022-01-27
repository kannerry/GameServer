using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
namespace Spells
{
    public class LuluE : ISpellScript
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
            //owner.SpellAnimation("SPELL2");
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        static internal IObjAiBase ETarget;

        public void OnSpellPostCast(ISpell spell)
        {
            PlayAnimation(spell.CastInfo.Owner, "Spell3");
            var target = spell.CastInfo.Targets[0].Unit;
            if (target.Team != spell.CastInfo.Owner.Team)
            {
                var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.4;
                var spellvl = spell.CastInfo.SpellLevel * 40;
                target.TakeDamage(spell.CastInfo.Owner, (float)(ap + spellvl), GameServerCore.Enums.DamageType.DAMAGE_TYPE_MAGICAL, GameServerCore.Enums.DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
            else
            {
                var x = target as IObjAiBase;
                var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.6;
                var spellvl = spell.CastInfo.SpellLevel * 40;
                var shieldamt = (float)(ap + spellvl + 40);
                x.ApplyShield(target, shieldamt, true, true, false);

                spell.CastInfo.Owner.SetTargetUnit(null);

                ETarget = x;

                CreateTimer(6.0f, () => 
                { 
                    x.ApplyShield(target, -shieldamt, true, true, false);
                    ETarget = null;
                });
            }
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

        private void ChangeModel(int skinId, IAttackableUnit target)
        {
            switch (skinId)
            {
                case 0:
                    target.ChangeModel("LuluSquill");
                    break;

                case 1:
                    target.ChangeModel("LuluCupcake");
                    break;

                case 2:
                    target.ChangeModel("LuluKitty");
                    break;

                case 3:
                    target.ChangeModel("LuluDragon");
                    break;

                case 4:
                    target.ChangeModel("LuluSnowman");
                    break;
            }
        }
    }
}