using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Buffs
{
    internal class JaxRelentlessAttack : IBuffGameScript
    {
        public BuffType BuffType => BuffType.DAMAGE;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        int counter = 0;
        IBuff thisBuff;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;

            if (unit is IObjAiBase obj)
            {

                ApiEventManager.OnLaunchAttack.AddListener(this, obj, TargetExecute, true);




            }
        }
        public void TargetExecute(ISpell spell)


        {

            var owner = spell.CastInfo.Owner;
            var AP = owner.Stats.AbilityPower.Total * 0.7f;
            var target = spell.CastInfo.Targets[0].Unit;
            float damage = 100 * owner.GetSpell(1).CastInfo.SpellLevel + AP;

            counter++;
            if (counter >= 3)

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(owner, target, "RelentlessAssault_tar.troy", target, 1f);



        }



        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);

            thisBuff.DeactivateBuff();

        }

        public void OnUpdate(float diff)
        {

        }
    }
}