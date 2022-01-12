using GameServerCore;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class ParticleCommand : ChatCommandBase
    {
        private readonly IPlayerManager _playerManager;

        public override string Command => "particle";
        public override string Syntax => $"{Command} particle";

        public ParticleCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.ToLower().Split(' ');
            if (split.Length < 2)
            {
                ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
            else if (true == true)
            {
                var user = _playerManager.GetPeerInfo(userId).Champion;
                AddParticlePos(user, split[1], user.Position, user.Position);
            }
        }
    }
}
