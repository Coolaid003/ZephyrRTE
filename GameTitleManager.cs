using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace x360Tool
{
    public class GameTitleManager
    {
        private readonly List<GameTitle> _gameTitles;

        public GameTitleManager()
        {
            _gameTitles = JsonConvert.DeserializeObject<List<GameTitle>>(File.ReadAllText(Directory.GetCurrentDirectory() + "\\titles.json"));
        }

        public GameTitle GetTitleByID(uint titleId) => _gameTitles.Find(gameTitle => gameTitle.ID == titleId);
    }

    public class GameTitle
    {
        public string Title { get; set; }
        public uint ID { get; set; }
        public string IconPath { get; set; }
    }
}