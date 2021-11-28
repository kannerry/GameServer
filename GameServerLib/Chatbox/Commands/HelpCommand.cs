using System;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class HelpCommand : ChatCommandBase
    {
        private const string COMMAND_PREFIX = "<font color=\"#E175FF\"><b>";
        private const string COMMAND_SUFFIX = "</b></font>, ";
        private readonly int MESSAGE_MAX_SIZE = 512;

        public override string Command => "help";
        public override string Syntax => $"{Command}";

        public HelpCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {

        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            if (!Game.Config.ChatCheatsEnabled)
            {
                var msg = "chat commands are disabled in this game";
                ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.INFO, msg);
                return;
            }

            var commands = ChatCommandManager.GetCommandsStrings();
            var commandsString = "";
            var lastCommandString = "";
            var isNewMessage = false;

            ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.INFO, "list of commands");

            foreach (var command in commands)
            {
                if(isNewMessage)
                {
                    commandsString = new String(lastCommandString);
                    isNewMessage = false;
                }

                lastCommandString = $"{COMMAND_PREFIX}" +
                $"{ChatCommandManager.CommandStarterCharacter}{command}" +
                $"{COMMAND_SUFFIX}";

                if(commandsString.Length + lastCommandString.Length >= MESSAGE_MAX_SIZE)
                {
                    ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.NORMAL, commandsString);
                    commandsString = "";
                    isNewMessage = true;
                }
                else
                {
                    commandsString = $"{commandsString}{lastCommandString}";
                }
            }

            if (commandsString.Length != 0)
            {
                ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.NORMAL, commandsString);
            }

            ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.INFO, "there are " + commands.Count + " commands");
        }
    }
}
