using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Buffs
{
    internal class IreliaHitenStyleCharged : IBuffGameScript
    {
        public BuffType BuffType => BuffType.DAMAGE;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {

                if (unit is IObjAiBase obj)
            { 
                    ApiEventManager.OnLaunchAttack.AddListener(this, obj, TargetExecute, false);




            }
        }
        public void TargetExecute(ISpell spell)


        {

            var owner = spell.CastInfo.Owner;
            var target = spell.CastInfo.Targets[0].Unit;
            float damage = 15 * owner.GetSpell(1).CastInfo.SpellLevel;
            float heal = 10 * spell.CastInfo.SpellLevel;
            owner.Stats.CurrentHealth += heal;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);


        }
    
        

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);

        }
        
        public void OnUpdate(float diff)
        {

        }
    }
}