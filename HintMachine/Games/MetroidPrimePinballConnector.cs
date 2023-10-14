﻿namespace HintMachine.Games
{
    class MetroidPrimePinballConnector : INintendoDSConnector
    {
        private readonly HintQuestCumulative _pointsQuest = new HintQuestCumulative
        {
            Name = "Points",
            GoalValue = 500000
        };
        private readonly HintQuestCumulative _artifactsQuest = new HintQuestCumulative
        {
            Name = "Chozo Artifacts",
            Description = "Only applicable in Multi-Mission mode.",
            GoalValue = 3
        };

        enum Region
        {
            PAL,
            NTSC_U
        }
        private Region _region = Region.PAL;

        // ---------------------------------------------------------

        public MetroidPrimePinballConnector()
        {
            Name = "Metroid Prime Pinball (DS)";
            Description = "Metroid Prime, but abridged as a Pinball game.";
            SupportedVersions = "Tested on PAL and NTSC-U roms with BizHawk 2.9.1";
            Author = "Knuxfan24";
            Quests.Add(_pointsQuest);
            Quests.Add(_artifactsQuest);
        }

        public override bool Connect()
        {
            if (!base.Connect()) 
                return false;

            // Look for the PAL ROM first.
            byte[] MPP_SIG = new byte[] { 0xFF, 0xDE, 0xFF, 0xE7, 0xFF, 0xDE, 0xFF, 0xE7, 0xFF, 0xDE, 0xFF, 0xE7, 0xFF, 0xDE, 0x05, 0xE9 };
            if (FindRamSignature(MPP_SIG, 0))
            {
                _region = Region.PAL;
                return true;
            }

            // If we didn't find the PAL ROM, then look for the NTSC-U ROM.
            MPP_SIG = new byte[] { 0xFF, 0xDE, 0xFF, 0xE7, 0xFF, 0xDE, 0xFF, 0xE7, 0xFF, 0xDE, 0xFF, 0xE7, 0xFF, 0xDE, 0x43, 0xF3 };
            if (FindRamSignature(MPP_SIG, 0))
            {
                _region = Region.NTSC_U;
                return true;
            }

            return false;
        }

        public override void Disconnect()
        {
            _ram = null;
        }

        public override bool Poll()
        {
            switch (_region)
            {
                case Region.PAL:
                    _pointsQuest.UpdateValue(_ram.ReadInt32(_dsRamBaseAddress + 0x3BB9B4));
                    _artifactsQuest.UpdateValue(_ram.ReadInt32(_dsRamBaseAddress + 0x3D428C));
                    break;
                case Region.NTSC_U:
                    _pointsQuest.UpdateValue(_ram.ReadInt32(_dsRamBaseAddress + 0x3AFC50));
                    _artifactsQuest.UpdateValue(_ram.ReadInt32(_dsRamBaseAddress + 0x3C7658));
                    break;
            }
            return true;
        }
    }
}
