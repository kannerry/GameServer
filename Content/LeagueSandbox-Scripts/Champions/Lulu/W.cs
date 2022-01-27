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
    public class LuluW : ISpellScript
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

        public void OnSpellPostCast(ISpell spell)
        {
            if (spell.CastInfo.Targets[0].Unit.Team != spell.CastInfo.Owner.Team)
            {
                var time = 1 + 0.25f * spell.CastInfo.SpellLevel;
                var target = spell.CastInfo.Targets[0].Unit;
                var model = target.Model;
                AddBuff("LuluWDebuff", time, 1, spell, target, spell.CastInfo.Owner);
                ChangeModel(spell.CastInfo.Owner.SkinID, target);
                CreateTimer(time, () =>
                {
                    //RemoveParticle(p);
                    target.ChangeModel(model);
                });
            }
            else
            {
                var time = 2.5f + 0.5f * spell.CastInfo.SpellLevel;
                AddBuff("LuluWBuff", time, 1, spell, spell.CastInfo.Targets[0].Unit, spell.CastInfo.Owner);
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