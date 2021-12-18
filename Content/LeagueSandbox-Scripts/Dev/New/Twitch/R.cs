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
    public class TwitchFullAutomatic : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        internal static bool toggled = false;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnLaunchAttack(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            if (toggled == true)
            {
                CreateTimer(0.01f, () => { SpellCast(owner, 0, SpellSlotType.ExtraSlots, spell.CastInfo.Targets[0].Unit.Position, spell.CastInfo.Targets[0].Unit.Position, false, Vector2.Zero); });
            }
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            toggled = true;
            owner.Stats.Range.FlatBonus += 400;
            CreateTimer(7.0f, () => 
            {
                toggled = false;
                owner.Stats.Range.FlatBonus = 0; 
            });
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

    public class TwitchBasicAttack : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, TargetExecute, false);
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecuteSwag, false);
        }
        IAttackableUnit tar;
        public void TargetExecuteSwag(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            var damage = owner.Stats.AttackDamage.Total;
            if(target == tar)
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
        }

        private void TargetExecute(ISpell spell)
        {
            if (TwitchFullAutomatic.toggled == true)
            {

                spell.CastInfo.Owner.PlayAnimation("Spell4");

                tar = spell.CastInfo.Targets[0].Unit;
                spell.CreateSpellSector(new SectorParameters
                {
                    BindObject = spell.CastInfo.Owner,
                    Length = 1000f,
                    Width = 100f,
                    PolygonVertices = new Vector2[]
    {
                    // Basic square, however the values will be scaled by Length/Width, which will make it a rectangle
                    new Vector2(-1, 0),
                    new Vector2(-1, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0)
    },
                    SingleTick = true,
                    Type = SectorType.Polygon
                });
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