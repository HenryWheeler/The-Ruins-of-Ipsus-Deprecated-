using System;
using RLNET;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class SaveDataManager
    {
        private static string directoryName = "TheRuinsOfIpsusSaveFiles";
        private static JsonSerializerSettings options;
        public static bool savePresent = false;
        public SaveDataManager()
        {
            options = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string savePath = Path.Combine(path, directoryName);
            if (File.Exists(Path.Combine(savePath, "DebugSaveFile.json"))) { savePresent = true; }
        }
        public static void CreateDebugSave(Player _player)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, directoryName);
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            string saveData = JsonConvert.SerializeObject(_player, options);
            File.WriteAllText(Path.Combine(path, "DebugSaveFile.json"), saveData);
        }
        public static void LoadDebugSave(Player _player)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string savePath = Path.Combine(path, directoryName);

            Coordinate coordinate = _player.GetComponent<Coordinate>();
            Map.map[coordinate.x, coordinate.y].actor = null;
            TurnManager.RemoveActor(_player.GetComponent<TurnFunction>());
            _player.GetComponent<TurnFunction>().turnActive = false;
            Program.rootConsole.Update -= _player.Update;

            string pullData = File.ReadAllText(Path.Combine(savePath, "DebugSaveFile.json"));

            Player saveData = JsonConvert.DeserializeObject<Player>(pullData, options);
            Program.ReloadPlayer(saveData.components);
        }
        public static void CreateSave(Player _player)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, directoryName);
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            List<Tile> tiles = new List<Tile>();
            foreach(Tile tile in Map.map)
            {
                if (tile != null)
                {
                    if (tile.actor == _player) { tile.actor = null; }
                    tiles.Add(tile);
                }
            }

            SaveData data = new SaveData(CMath.seedInt, _player, tiles);

            string saveData = JsonConvert.SerializeObject(data, options);
            File.WriteAllText(Path.Combine(path, "SaveFile.json"), saveData);
        }
        public static void LoadSave()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string savePath = Path.Combine(path, directoryName);
            string pullData = File.ReadAllText(Path.Combine(savePath, "SaveFile.json"));
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(pullData, options);

            Program.LoadSave(saveData);   
        }
    }
    [Serializable]
    public class SaveData
    {
        public int seed { get; set; }
        public Player player { get; set; }
        public List<Tile> tiles { get; set; }
        public SaveData(int _seed, Player _player, List<Tile> _tiles) 
        {
            seed = _seed;
            player = _player;
            tiles = _tiles;
        }
        public SaveData() { }
    }
}
