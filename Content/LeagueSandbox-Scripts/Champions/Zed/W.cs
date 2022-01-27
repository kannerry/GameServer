using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class ZedShadowDash : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            AutoFaceDirection = false,
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
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

        static internal bool WCast = false;
        public void OnSpellPostCast(ISpell spell)
        {
            WCast = true;
            var owner = spell.CastInfo.Owner as IChampion;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            ZedShadowDashMissile._spell = spell;
            SpellCast(owner, 4, SpellSlotType.ExtraSlots, spellPos, spellPos, true, Vector2.Zero);
            PlayAnimation(owner, "Spell2_Cast", timeScale: 0.6f);
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }

    public class ZedShadowDashMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        IObjAiBase _owner;
        static internal ISpell _spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            LogDebug("yo");
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }
        static internal IMinion WShadow;
        static internal IMinion RShadow;
        public void OnMissileEnd(ISpellMissile missile)
        {
            if(ZedShadowDash.WCast == true)
            {
                WShadow = AddMinion(_owner, "ZedShadow", "WShadow", missile.Position, targetable: false, ignoreCollision: true);
                WShadow.FaceDirection(_owner.Direction);
                AddBuff("ShadowTimer", 4.0f, 1, _spell, WShadow, _owner);
                _owner.SetSpell("ZedW2", 1, true);
                ZedShadowDash.WCast = false;
            }
            else
            {
                LogDebug("yo");
                RShadow = AddMinion(_owner, "ZedShadow", "RShadow", _owner.Position, targetable: false, ignoreCollision: true);
                RShadow.FaceDirection(_owner.Direction);
                AddBuff("ShadowTimer", 4.0f, 1, _spell, RShadow, _owner);
                _owner.SetSpell("ZedR2", 3, true);
                ZedUlt.RCast = false;
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

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }

    public class ZedW2 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        static internal bool recast = false;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var WShadow = ZedShadowDashMissile.WShadow;
            var ownerPos = owner.Position;
            var WShadowPos = WShadow.Position;
            WShadow.TeleportTo(ownerPos.X, ownerPos.Y);
            owner.TeleportTo(WShadowPos.X, WShadowPos.Y);
            owner.SetSpell("ZedShadowDash", 1, true);

            AddParticleTarget(owner, owner, "zed_base_cloneswap.troy", owner);
            AddParticleTarget(owner, WShadow, "zed_base_cloneswap.troy", WShadow);
            recast = true;

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

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
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
