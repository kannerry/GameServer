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
    public class YasuoRKnockUpComboW : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, HideE, false);
            own = owner;
        }

        public void HideE(ISpell spell)
        {
            CreateTimer((float)0.1, () => { SealSpellSlot(own, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true); });
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        private IObjAiBase own;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Vector2 xy = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            //var green = AddParticle(owner, null, "cryo_storm_green_team.troy", xy, lifetime: 1.0f, reqVision: false);
            var ch = GetChampionsInRange(xy, 300, true);
            foreach (var champ in ch)
            {
                if (champ.HasBuff("Pulverize"))
                {
                    var dmg = own.Stats.AttackDamage.Total * 1.5f;
                    var xyz = champ as IObjAiBase;
                    xyz.SetTargetUnit(null);
                    ForceMovement(champ, "RUN", new Vector2(champ.Position.X + 10f, champ.Position.Y + 10f), 13f, 0, 16.5f, 0);
                    PlayAnimation(owner, "Spell4");
                    TeleportTo(owner, xy.X, xy.Y);

                    var lvl = 25+80 - spell.CastInfo.SpellLevel * 25;
                    CreateTimer(0.1f, () => { ((IObjAiBase)spell.CastInfo.Owner).GetSpell(3).SetCooldown(lvl); });

                    champ.TakeDamage(owner, dmg, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
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

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
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