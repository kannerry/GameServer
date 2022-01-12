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
    public class AkaliShadowDance : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            //ApiEventManager.OnSpellHit.AddListener(this, spell, ApplyEffects, false);
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
        int intejer = 0;
        public void OnSpellPostCast(ISpell spell)
        {

            intejer++;

            var owner = spell.CastInfo.Owner;
            var target = spell.CastInfo.Targets[0].Unit;
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var to = Vector2.Normalize(new Vector2(target.Position.X, target.Position.Y) - current);
            var range = to * 800;

            if (AkaliSmokeBomb.SwagSector != null)
            {
                AkaliSmokeBomb.SwagSector.SetToRemove();
                AkaliSmokeBomb.smokeBomb.SetToRemove();
                AkaliSmokeBomb.smokeBombBorder.SetToRemove();
            }

            //TODO: Dash to the correct location (in front of the enemy IChampion) instead of far behind or inside them
            owner.SetTargetUnit(null);
            ForceMovement(owner, target, "Spell4", 2200, 0, 0, 0, 20000);
            //ForceMovement(spell.CastInfo.Owner, "Spell4", trueCoords, 2200, 0, 0, 0);
            AddParticleTarget(owner, target, "akali_shadowDance_tar.troy", target);

            var bonusAd = owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue;
            var ap = owner.Stats.AbilityPower.Total * 0.9f;
            var damage = 100 + spell.CastInfo.SpellLevel * 150 + bonusAd + ap;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            if(intejer == 3)
            {
                spell.SetCooldown(30f, true);
                intejer = 0;
            }
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