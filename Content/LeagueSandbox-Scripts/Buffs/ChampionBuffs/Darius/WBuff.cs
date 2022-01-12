using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Buffs
{
    internal class DariusNoxianTacticsActive: IBuffGameScript
    {
        public BuffType BuffType => BuffType.DAMAGE;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff thisBuff;
        IObjAiBase Unit;
        
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            if (unit is IObjAiBase ai)
            {
                Unit = ai;

                ApiEventManager.OnHitUnit.AddListener(this, ai, TargetExecute, true);

                ai.SkipNextAutoAttack();


            }
            
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //ApiEventManager.OnHitUnit.RemoveListener(this);

        }
        public void TargetExecute(IAttackableUnit target, bool Iscrit)
        {
            if (!thisBuff.Elapsed() && thisBuff != null && Unit != null)
            {
                float ad = Unit.Stats.AttackDamage.Total * 0.2f;
                
                float damage =  ad;
                
                
                target.TakeDamage(Unit, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                
                thisBuff.DeactivateBuff();
            }
        }
        public void OnUpdate(float diff)
        {

        }
    }
}