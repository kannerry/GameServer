using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class RenektonW : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IBuff thisBuff;
        private IObjAiBase Unit;
        private IParticle p;
        private IParticle p2;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            if (unit is IObjAiBase ai)
            {
                Unit = ai;

                ApiEventManager.OnHitUnit.AddListener(this, ai, TargetExecute, true);
                p = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "Renekton_Weapon_Hot.troy", unit, buff.Duration, 1, "R_hand", "R_hand");
                p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "Renekton_Weapon_CoolOff.troy", unit, buff.Duration, 1, "R_hand", "R_hand");
                ai.SkipNextAutoAttack();
            }

            StatsModifier.Range.FlatBonus += 50;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //ApiEventManager.OnHitUnit.RemoveListener(this);

            RemoveParticle(p);
            RemoveParticle(p2);
        }

        public void TargetExecute(IAttackableUnit target, bool Iscrit)
        {
            if (!thisBuff.Elapsed() && thisBuff != null && Unit != null)
            {
                if(Unit.Stats.CurrentMana >= 50)
                {
                    float ap = Unit.Stats.AttackDamage.Total * 1.25f;
                    float damage = 15 + 25 * Unit.GetSpell(1).CastInfo.SpellLevel + ap;
                    if (!(target is ILaneTurret))
                    {
                        target.TakeDamage(Unit, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                        AddBuff("Stun", 1.5f, 1, Unit.GetSpell(1), target, Unit);
                    }
                    Unit.Stats.CurrentMana -= 50;
                    PlayAnimation(Unit, "ATTACK2");
                    CreateTimer(0.25f, () => { Unit.Stats.CurrentMana += 5; target.TakeDamage(Unit, Unit.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false); });

                    thisBuff.DeactivateBuff();
                }
                else
                {
                    float ap = Unit.Stats.AttackDamage.Total * 0.6f;
                    float damage = 15 + 25 * Unit.GetSpell(1).CastInfo.SpellLevel + ap;
                    if (!(target is ILaneTurret))
                    {
                        target.TakeDamage(Unit, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                        AddBuff("Stun", 0.75f, 1, Unit.GetSpell(1), target, Unit);
                    }

                    PlayAnimation(Unit, "ATTACK2");
                    CreateTimer(0.25f, () => { Unit.Stats.CurrentMana += 5; target.TakeDamage(Unit, Unit.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false); });

                    thisBuff.DeactivateBuff();
                }
            }
        }

        public void OnUpdate(float diff)
        {
        }
    }
}