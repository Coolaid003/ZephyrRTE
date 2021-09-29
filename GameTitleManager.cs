using System.IO;
using System.Collections.Generic;
using JRPC_Client;
using Newtonsoft.Json;
using XDevkit;

namespace x360Tool
{
    public class GameTitleManager
    {
        private readonly List<GameTitle> _gameTitles;

        public GameTitleManager()
        {
            _gameTitles = JsonConvert.DeserializeObject<List<GameTitle>>(File.ReadAllText(Directory.GetCurrentDirectory() + "\\titles.json"));
        }

        /// <summary>
        /// Find an Xbox title by its ID if it exists in titles.json
        /// </summary>
        public GameTitle GetTitleById(uint titleId) => _gameTitles.Find(gameTitle => gameTitle.ID == titleId);

        /// <summary>
        /// Checks if the current running title has changed.
        /// </summary>
        public bool TitleChanged(ref IXboxConsole console, ref GameTitle currentTitle)
        {
            GameTitle changedTitle = GetTitleById(JRPC.XamGetCurrentTitleId(console));
            if (changedTitle != null && changedTitle.ID != 0 && changedTitle.ID != currentTitle.ID)
                return true;
            return false;
        }
    }

    public class GameTitle
    {
        public string Title { get; set; }
        public uint ID { get; set; } = 0;
        public string IconPath { get; set; }
    }
}