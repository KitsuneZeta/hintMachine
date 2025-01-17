﻿namespace HintMachine.Games
{
    class BustAMove4Connector : IGameConnector
    {
        private readonly HintQuestCumulative _winQuest = new HintQuestCumulative
        {
            Name = "Wins",
            GoalValue = 3
        };

        private ProcessRamWatcher _ram = null;
        private long _winAddr = 0;

        public BustAMove4Connector()
        {
            Name = "Bust a Move 4";
            Description = "Puzzle Bobble 4 (also known as Bust-a-Move 4 in North America and Europe) is the third sequel to the video game Puzzle Bobble and is the final appearance of the series on the Arcade, PlayStation and Dreamcast. The game is also the final title to be recognizably similar in presentation to the original.\r\n\r\nBuilding upon the success of Puzzle Bobble 3, the game adds a pulley system that requires two sets of bubbles, attached to either side of a rope hanging across two pulleys. The game contains a story mode for single player play.\r\n\r\nIn total, the game features 640 levels. The console version features a level editor to either create and save a level, set a succession of levels, or to create an unlimited amount of extra levels and stages. It also has an alternative \"story mode\".";
            Platform = "PS1";
            SupportedVersions.Add("European ROM");
            SupportedEmulators.Add("ePSXe 1.7.0");
            CoverFilename = "bust_a_move_4.png";
            Author = "CalDrac";

            Quests.Add(_winQuest);
        }

        public override bool Connect()
        {
            _ram = new ProcessRamWatcher("ePSXe");
            if (!_ram.TryConnect())
                return false;

            _winAddr = _ram.BaseAddress + 0x62634A;
            return true;
        }

        public override void Disconnect()
        {
            _ram = null;
        }

        public override bool Poll()
        {
            _winQuest.UpdateValue(_ram.ReadInt8(_winAddr));
            return true;
        }
    }
}
