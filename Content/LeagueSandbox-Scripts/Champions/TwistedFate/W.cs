using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class PickACard : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        private IObjAiBase _owner;
        private ISpell _spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            _spell = spell;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, RemoveCard, false);
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        public void RemoveCard(ISpell spell)
        {
            if (cardParticle != null)
            {
                cardParticle.SetToRemove();
            }
        }

        private void AddMana(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            float manaGain = 25 + (spell.CastInfo.SpellLevel * 25);
            var newMana = target.Stats.CurrentMana + manaGain;
            target.Stats.CurrentMana = Math.Min(newMana, target.Stats.ManaPoints.Total);
        }

        public void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            var ap = _owner.Stats.AbilityPower.Total * 0.5f;
            switch (card)
            {
                case 1:
                    AddParticle(_owner, unit, "PickaCard_blue_tar.troy", unit.Position, lifetime: 6.0f);
                    unit.TakeDamage(_owner, 200, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    AddMana(_owner, _spell, _owner);
                    card = 0;
                    return;

                case 2:
                    AddParticle(_owner, unit, "PickaCard_red_tar.troy", unit.Position, lifetime: 6.0f);
                    var aoe = GetUnitsInRange(unit.Position, 250, true);
                    foreach (var unitx in aoe)
                    {
                        if (unitx.Team != _owner.Team)
                        {
                            unitx.TakeDamage(_owner, 200, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                            AddBuff("TFSlow", 2.5f, 1, _spell, unitx, _owner);
                        }
                    }
                    card = 0;
                    return;

                case 3:
                    AddParticle(_owner, unit, "PickaCard_yellow_tar.troy", unit.Position, lifetime: 6.0f);
                    unit.TakeDamage(_owner, 200, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    AddBuff("VeigarEventHorizon", 1.0f, 1, _spell, unit, _owner);
                    card = 0;
                    return;
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        private bool toggled = false;
        private int card = 0;
        private IParticle cardParticle;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            if (toggled != true)
            {
                CreateTimer(0.01f, () => { ((IObjAiBase)spell.CastInfo.Owner).GetSpell(1).SetCooldown(0f); });
                //PARTICLES
                card = 1;
                AddParticle(owner, owner, "TwistedFate_Base_W_BlueCard.troy", owner.Position);
                CreateTimer(1.0f, () =>
                {
                    if (toggled == true)
                    {
                        card = 2;
                        AddParticle(owner, owner, "TwistedFate_Base_W_RedCard.troy", owner.Position);
                    }
                });
                CreateTimer(2.0f, () =>
                {
                    if (toggled == true)
                    {
                        card = 3;
                        AddParticle(owner, owner, "TwistedFate_Base_W_GoldCard.troy", owner.Position);
                    }
                });
                CreateTimer(3.0f, () =>
                {
                    if (toggled == true)
                    {
                        card = 1;
                        AddParticle(owner, owner, "TwistedFate_Base_W_BlueCard.troy", owner.Position);
                    }
                });
                CreateTimer(4.0f, () =>
                {
                    if (toggled == true)
                    {
                        card = 2;
                        AddParticle(owner, owner, "TwistedFate_Base_W_RedCard.troy", owner.Position);
                    }
                });
                CreateTimer(5.0f, () =>
                {
                    if (toggled == true)
                    {
                        card = 3;
                        AddParticle(owner, owner, "TwistedFate_Base_W_GoldCard.troy", owner.Position);
                    }
                });
                CreateTimer(6.0f, () =>
                {
                    card = 0;
                    toggled = false;
                });
                toggled = true;
            }
            else
            {
                toggled = false;
                switch (card)
                {
                    case 1:
                        cardParticle = AddParticle(owner, owner, "Card_Blue.troy", owner.Position, lifetime: 6.0f);
                        CreateTimer(6.0f, () => { card = 0; });
                        return;

                    case 2:
                        cardParticle = AddParticle(owner, owner, "Card_Red.troy", owner.Position, lifetime: 6.0f);
                        CreateTimer(6.0f, () => { card = 0; });
                        return;

                    case 3:
                        cardParticle = AddParticle(owner, owner, "Card_Yellow.troy", owner.Position, lifetime: 6.0f);
                        CreateTimer(6.0f, () => { card = 0; });
                        return;
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