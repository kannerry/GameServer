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
    public class RengarQ : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true
            // TODO
        };

        IObjAiBase _owner;
        ISpell _spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            _spell = spell;
            ApiEventManager.OnPreAttack.AddListener(this, owner, ChangeAnim, false);
        }

        public void ChangeAnim(ISpell spell)
        {
            if (RengarBasicAttack.Applied == 0)
            {
                if(!RengarQ.inBush == true)
                {
                    spell.CastInfo.Owner.PlayAnimation("Spell1", 1f, flags: AnimationFlags.Override);
                    CreateTimer(0.5f, () => { spell.CastInfo.Owner.StopAnimation("Spell1", fade: true); });
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        static internal bool maxStacked = false;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            if (!(spell.CastInfo.Owner.Stats.CurrentMana == 5))
            {
                //var owner = spell.CastInfo.Owner as IChampion;
                AddBuff("RengarQBuff", 3.0f, 1, spell, owner, owner);
                RengarBasicAttack.Applied = 0;
                LogDebug("yo");
            }
            if (spell.CastInfo.Owner.Stats.CurrentMana == 5)
            {
                //var owner = spell.CastInfo.Owner as IChampion;
                AddBuff("RengarQBuff", 3.0f, 1, spell, owner, owner);
                RengarBasicAttack.Applied = 0;
                maxStacked = true;
                owner.Stats.CurrentMana = 1;
            }
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var cd = (new float[] { 6f, 5.5f, 5f, 4.5f, 4f }[spell.CastInfo.SpellLevel - 1]);
            spell.SetCooldown(cd, true);
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

        static internal bool inBush = false;
        bool JumpParticle;
        float i = 0;
        bool boolProcMax = false;
        //FOR SOME REASON RENGAR PASSIVE OnUpdate
        //DOES NOT WORK?!
        // so i have to do passive logic in Q :DDDD fun
        public void OnUpdate(float diff)
        {

            if(_owner.Stats.CurrentMana == 5)
            {
                if(boolProcMax == false)
                {
                    _owner.GetSpell(0).SetCooldown(0, true);
                    _owner.GetSpell(1).SetCooldown(0, true);
                    _owner.GetSpell(2).SetCooldown(0, true);
                    boolProcMax = true;
                    CreateTimer(1.0f, () => { boolProcMax = false; });
                }
            }

            // IF i am inside of a bush
            if (_owner.IsBrush(_owner.Position))
            {
                //AND inBush bool is false
                if (inBush == false)
                {
                    // make it true
                    //and add my range
                    inBush = true;
                    _owner.Stats.Range.FlatBonus = 600;
                }
                if (JumpParticle == false)
                {
                    //add it (WHILE IN BUSH)
                    JumpParticle = true;
                }
            }
            // IF i am outside of a bush
            if (!_owner.IsBrush(_owner.Position))
            {
                //AND my ult is not procced
                if(RengarR.proccedUlt != true)
                {
                    // check if inBush bool is true
                    if (inBush == true)
                    {
                        // and set it to false
                        inBush = false;
                        _owner.Stats.Range.FlatBonus = 0;
                    }
                    // remove particle
                    if (JumpParticle != null)
                    {
                        JumpParticle = false;
                    }
                }
            }
            // IF my ult is procced
            if (RengarR.proccedUlt == true)
            {
                //AND inBush bool is false
                if (inBush == false)
                {
                    // make it true
                    //and add my range
                    inBush = true;
                    _owner.Stats.Range.FlatBonus = 600;
                }
                JumpParticle = true;
            }

            if(JumpParticle == true)
            {
                AddParticle(_owner, _owner, "Rengar_Base_P_Buf_Ring.troy", _owner.Position, lifetime: 0.025f);
                JumpParticle = false;
            }

        }
    }

    public class RengarBasicAttack : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = false,
            NotSingleTargetSpell = true
            // TODO
        };

        private ISpell originspell;
        private IObjAiBase ownermain;
        static internal int Applied = 1;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            originspell = spell;
            ownermain = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
            ApiEventManager.OnPreAttack.AddListener(this, owner, JumpBush, false);
            ApiEventManager.OnFinishDash.AddListener(this, owner, JumpBushAttack, false);
        }

        IAttackableUnit target;

        public void JumpBush(ISpell spell)
        {
            target = ownermain.TargetUnit;
            if (RengarQ.inBush == true)
            {
                CreateTimer(0.02f, () =>
                {
                    if(ownermain.Stats.CurrentMana == 1)
                    {
                        ownermain.Stats.CurrentMana++;
                    }
                    RengarR.proccedUlt = false;
                    var to = Vector2.Normalize(target.Position - ownermain.Position);
                    ForceMovement(ownermain, "Spell2", new Vector2(target.Position.X - to.X * 100f, target.Position.Y - to.Y * 100f), 1500, 0, 75, 0);
                    ownermain.SetTargetUnit(null);
                });
                //ownermain.CancelAutoAttack(true);
            }
        }

        public void JumpBushAttack(IAttackableUnit unit)
        {
            //ownermain.SetTargetUnit(target);
            ownermain.SetTargetUnit(target);
            //target.TakeDamage(ownermain, ownermain.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            PlayAnimation(ownermain, "ATTACK1");
        }

        public void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            var owner = ownermain;
            owner.SetStatus(StatusFlags.Targetable, true);
            var Champs = GetChampionsInRange(owner.Position, 50000, true);

            var bonusdmg = 30 * owner.Spells[0].CastInfo.SpellLevel;
            var adscaling = owner.Stats.AttackDamage.Total * (5 * (owner.Spells[0].CastInfo.SpellLevel - 1) * 0.01);
            var adscalingMAX = owner.Stats.AttackDamage.Total * 0.5;

            foreach (IChampion player in Champs)
            {
                owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f);
                owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, true);
            }
            if (Applied != 1)
            {
                LogDebug("yo");
                if (RengarQ.maxStacked == true)
                {
                    unit.TakeDamage(owner, (float)(adscalingMAX + bonusdmg), DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    RengarQ.maxStacked = false;
                }
                else
                {
                    unit.TakeDamage(owner, (float)(adscaling + bonusdmg), DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    owner.Stats.CurrentMana += 1;
                }
                Applied = 1;
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