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
        private static string directoryName = "TheRuinsOfIpsusSaveFiles";
        private static JsonSerializerOptions options;
        public static bool savePresent = false;
        public SaveDataManager()
        {
            options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
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

            SaveData data = new SaveData(_player);
            string saveData = JsonSerializer.Serialize(data, options);
            File.WriteAllText(Path.Combine(path, "DebugSaveFile.json"), saveData);
        }
        public static void LoadDebugSave(Player _player)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string savePath = Path.Combine(path, directoryName);

            Map.map[_player.x, _player.y].actor = null;
            TurnManager.RemoveActor(_player);
            _player.turnActive = false;
            _player.rootConsole.Update -= _player.Update;

            string pullData = File.ReadAllText(Path.Combine(savePath, "DebugSaveFile.json"));

            SaveData saveData = JsonSerializer.Deserialize<SaveData>(pullData, options);
            Program.ReloadPlayer(saveData.player);
        }
        public static void CreateSave(Player _player)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, directoryName);
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            List<ActorSaveData> actors = new List<ActorSaveData>();
            List<ItemSaveData> items = new List<ItemSaveData>();
            List<VisibilitySaveData> visibility = new List<VisibilitySaveData>();

            foreach(Tile tile in Map.map)
            {
                if (tile != null)
                {
                    if (tile.actor != null && tile.actor.name != "Player") 
                    {
                        ActorSaveData actorData = new ActorSaveData(tile.x, tile.y, tile.actor.id, tile.actor.hp, tile.actor.actLeft, tile.actor.bodyPlot, tile.actor.inventory, tile.actor.attacks);
                        actors.Add(actorData); 
                    }
                    if (tile.item != null) 
                    {
                        ItemSaveData itemData = new ItemSaveData(tile.x, tile.y, tile.item.id);
                        items.Add(itemData); 
                    }
                    if (tile.explored) { visibility.Add(new VisibilitySaveData(tile.x, tile.y)); }
                }
            }

            SaveData data = new SaveData(CMath.seedInt, _player, actors, items, visibility);


            string saveData = JsonSerializer.Serialize(data, options);
            File.WriteAllText(Path.Combine(path, "SaveFile.json"), saveData);
        }
        public static void LoadSave()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string savePath = Path.Combine(path, directoryName);
            string pullData = File.ReadAllText(Path.Combine(savePath, "SaveFile.json"));
            SaveData saveData = JsonSerializer.Deserialize<SaveData>(pullData, options);

            Program.LoadSave(saveData);   
        }
    }
    [Serializable]
    public class SaveData
    {
        public int seed { get; set; }
        public Player player { get; set; }
        public List<ActorSaveData> actors { get; set; }
        public List<ItemSaveData> items { get; set; }
        public List<VisibilitySaveData> visibility { get; set; }
        public SaveData(int _seed, Player _player, List<ActorSaveData> _actors, List<ItemSaveData> _items, List<VisibilitySaveData> _visibility) 
        {
            seed = _seed;
            player = _player;
            actors = _actors;
            items = _items;
            visibility = _visibility;
        }
        public SaveData(Player _player)
        {
            player = _player;
        }
        public SaveData() { }
    }
    [Serializable]
    public class VisibilitySaveData
    {
        public int x { get; set; }
        public int y { get; set; }
        public VisibilitySaveData(int _x, int _y) { x = _x; y = _y; }
        public VisibilitySaveData() { }
    }
    [Serializable]
    public class ActorSaveData
    {
        public int x { get; set; }
        public int y { get; set; }
        public int id { get; set; }
        public int hp { get; set; }
        public float actions { get; set; }
        public EquipmentSlot[] bodyPlot { get; set; }
        public List<Item> inventory { get; set; }
        public List<AtkData> attacks { get; set; }
        public ActorSaveData(int _x, int _y, int _id, int _hp, float _actions, EquipmentSlot[] _bodyPlot, List<Item> _inventory, List<AtkData> _attacks)
        {
            x = _x;
            y = _y;
            id = _id;
            hp = _hp;
            actions = _actions;
            bodyPlot = _bodyPlot;
            inventory = _inventory;
            attacks = _attacks;
        }
        public ActorSaveData() { }
    }
    [Serializable]
    public class ItemSaveData
    {
        public int x { get; set; }
        public int y { get; set; }
        public int id { get; set; }
        public ItemSaveData(int _x, int _y, int _id) { x = _x; y = _y; id = _id; }
        public ItemSaveData() { }
    }
}
