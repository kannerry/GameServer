using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class SingedQToggleBuff : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IAttackableUnit owner;
        private ISpell originSpell;
        private IBuff thisBuff;
        private IParticle red;
        private IParticle green;
        private float DamageManaTimer;
        private float SlowTimer;
        private float[] manaCost = { 13.0f };

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = unit;
            originSpell = ownerSpell;
            thisBuff = buff;
            originSpell.SetCooldown(1.0f, true);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
            if (owner != null && thisBuff != null && originSpell != null)
            {
                DamageManaTimer += diff;

                if (DamageManaTimer >= 500f)
                {
                    if (manaCost[0] > owner.Stats.CurrentMana)
                    {
                        RemoveBuff(thisBuff);
                    }
                    else
                    {
                        owner.Stats.CurrentMana -= manaCost[0];
                        var x = AddMinion(owner as IObjAiBase, "Singed", "trail", owner.Position, ignoreCollision: true, targetable: false, isVisible: false);

                        var Champs = GetAllChampionsInRange(owner.Position, 50000);
                        foreach (IChampion player in Champs)
                        {
                            x.SetStatus(StatusFlags.Targetable, false);
                            x.SetInvisible((int)player.GetPlayerId(), x, 0f, 0.0f);
                            x.SetHealthbarVisibility((int)player.GetPlayerId(), owner, false);
                        }
                        var DamageSector = originSpell.CreateSpellSector(new SectorParameters
                        {
                            Length = 360,
                            Tickrate = 1,
                            CanHitSameTargetConsecutively = true,
                            OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                            Type = SectorType.Area,
                            Lifetime = 3.25f,
                            BindObject = x
                        });
                        CreateTimer(3.25f, () => { x.TakeDamage(owner, 10000, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_RAW, false); });
                        AddParticle(owner, null, "SRU_Baron_AcidBall_Pool.troy", owner.Position, lifetime:3.25f);
                    }

                    DamageManaTimer = 0;
                }
            }
        }
    }
}