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
    public class NasusQ : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true
            // TODO
        };

        private IObjAiBase itemOwner;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            itemOwner = owner;
            ApiEventManager.OnPreAttack.AddListener(this, owner, ChangeAnim, false);
        }

        public void ChangeAnim(ISpell spell)
        {
            if (NasusQAttack.Applied == 0)
            {
                spell.CastInfo.Owner.PlayAnimation("Spell1", 3.5f, flags: AnimationFlags.Override);
                CreateTimer(0.5f, () => { spell.CastInfo.Owner.StopAnimation("Spell1", fade: true); });
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        private int stack = 0;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            //var owner = spell.CastInfo.Owner as IChampion;
            AddBuff("NasusQAttack", 6.0f, 1, spell, owner, owner);
            RemoveBuff(owner, "NasusQStacks");
            CreateTimer(0.01f, () => { AddBuff("NasusQStacks", int.MaxValue, (byte)stack, spell, owner, owner); });
            stack += 1;
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

    public class NasusQAttack : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = false,
            NotSingleTargetSpell = true
            // TODO
        };

        private ISpell originspell;
        private IObjAiBase ownermain;
        internal static int Applied = 1;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            originspell = spell;
            ownermain = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        //ApiEventManager.OnLaunchAttack.AddListener(this, owner, ChangeAnim, false);

        public void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            var owner = ownermain;
            var ADratio = owner.Stats.AttackDamage.PercentBonus * 0.3f;
            var damage = 40f + (30f * (originspell.CastInfo.SpellLevel - 1)) + ADratio;
            var v = owner.GetBuffWithName("NasusQStacks").StackCount;
            if (v != null)
            {
                damage += v;
            }
            if (Applied != 1)
            {
                unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                Applied = 1;
                //CreateTimer((float)6, () => { Applied = 1; });
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Applied = 0;
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