using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class KhazixR : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
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
            //AddBuff("TalonDisappear", 2f, 1, spell, owner, owner);
            AddParticleTarget(owner, owner, "Khazix_Base_R_Cas.troy", owner);
            var Champs = GetAllChampionsInRange(owner.Position, 50000);
            foreach (IChampion player in Champs)
            {
                CreateTimer(2f, () => { owner.SetStatus(StatusFlags.Targetable, true); });
                owner.SetStatus(StatusFlags.Targetable, false);
                if (player.Team.Equals(owner.Team))
                {
                    CreateTimer(2f, () => { owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f); });
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0.5f, 0.1f);
                }
                if (!(player.Team.Equals(owner.Team)))
                {
                    if (player.IsAttacking)
                    {
                        player.CancelAutoAttack(false);
                    }
                    CreateTimer(2f, () => { owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f); });
                    CreateTimer(2f, () => { owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, true); });
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.1f);
                    owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, false);
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