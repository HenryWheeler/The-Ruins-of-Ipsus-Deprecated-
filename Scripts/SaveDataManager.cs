using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using RLNET;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class SaveDataManager
    {
        public static void CreateSave(Player _player)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };

            Player data = _player;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string saveData = JsonSerializer.Serialize(data, options);
            File.WriteAllText(Path.Combine(path, "SaveFile.json"), saveData);
        }
        public static void LoadSave(Player _player)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string savePath = Path.Combine(path, "SaveFile.json");

            Map.map[_player.x, _player.y].actor = null;
            TurnManager.RemoveActor(_player);
            _player.turnActive = false;
            _player.rootConsole.Update -= _player.Update;

            string pullData = File.ReadAllText(savePath);

            Player player = JsonSerializer.Deserialize<Player>(pullData, options);

            Program.ReloadPlayer(player);

            Map.map[player.x, player.y].actor = player;
            TurnManager.AddActor(player);
            player.FOV();
            player.turnActive = true;
        }
    }
    [Serializable]
    public class SaveData
    {
        public Player player { get; set; }
        public SaveData(Player _player) 
        { 
            player = _player; 
        }
        public SaveData() { }
    }
}
