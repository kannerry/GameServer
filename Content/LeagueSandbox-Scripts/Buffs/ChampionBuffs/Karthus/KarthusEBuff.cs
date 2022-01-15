using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class KarthusEBuff : IBuffGameScript
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
        private float[] manaCost = { 30.0f, 50.0f, 60.0f };

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = unit;
            originSpell = ownerSpell;
            thisBuff = buff;

            var spellPos = new Vector2(originSpell.CastInfo.TargetPositionEnd.X, originSpell.CastInfo.TargetPositionEnd.Z);

            originSpell.SetCooldown(1.0f, true);

            SetTargetingType((IObjAiBase)unit, SpellSlotType.SpellSlots, 3, TargetingType.Self);

            if (owner.Team == TeamId.TEAM_BLUE)
            {
                red = AddParticle(owner, owner, "Karthus_Base_E_Defile_Red.troy", spellPos, lifetime: buff.Duration, reqVision: false, teamOnly: TeamId.TEAM_PURPLE);
                green = AddParticle(owner, owner, "Karthus_Base_E_Defile.troy", spellPos, lifetime: buff.Duration, reqVision: false, teamOnly: TeamId.TEAM_BLUE);
            }
            else
            {
                red = AddParticle(owner, owner, "Karthus_Base_E_Defile_Red.troy", spellPos, lifetime: buff.Duration, reqVision: false, teamOnly: TeamId.TEAM_BLUE);
                green = AddParticle(owner, owner, "Karthus_Base_E_Defile.troy", spellPos, lifetime: buff.Duration, reqVision: false, teamOnly: TeamId.TEAM_PURPLE);
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ownerSpell.SetCooldown(6.0f);

            SetTargetingType((IObjAiBase)unit, SpellSlotType.SpellSlots, 3, TargetingType.Area);

            RemoveParticle(red);
            RemoveParticle(green);
        }

        public void OnUpdate(float diff)
        {
            if (owner != null && thisBuff != null && originSpell != null)
            {
                DamageManaTimer += diff;

                if (DamageManaTimer >= 1000f)
                {
                    if (manaCost[originSpell.CastInfo.SpellLevel - 1] > owner.Stats.CurrentMana)
                    {
                        RemoveBuff(thisBuff);
                    }
                    else
                    {
                        owner.Stats.CurrentMana -= manaCost[originSpell.CastInfo.SpellLevel - 1];
                    }

                    DamageManaTimer = 0;
                }
            }
        }
    }
}