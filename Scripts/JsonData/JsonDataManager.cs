using System;
using RLNET;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class JsonDataManager
    {
        private static JsonSerializerOptions options;
        public static Dictionary<int, Monster> actors = new Dictionary<int, Monster>();
        public static Dictionary<int, Item> items = new Dictionary<int, Item>();
        public JsonDataManager()
        {
            options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };

            string actorPath = Path.Combine(Environment.CurrentDirectory, "MonsterData");
            string itemPath = Path.Combine(Environment.CurrentDirectory, "ItemData");

            int actorCount = Directory.GetFiles(actorPath).Count();
            int itemCount = Directory.GetFiles(itemPath).Count();

            for (int x = 1; x < actorCount + 1; x++)
            {
                string pullData = File.ReadAllText(Path.Combine(actorPath, "Monster_" + x + ".json"));
                actors.Add(x, JsonSerializer.Deserialize<Monster>(pullData, options));
            }
            for (int y = 1; y < itemCount + 1; y++)
            {
                string pullData = File.ReadAllText(Path.Combine(itemPath, "Item_" + y + ".json"));
                items.Add(y, JsonSerializer.Deserialize<Item>(pullData, options));
            }
        }
        public static void SaveActor(Monster actor)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "MonsterData");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "Monster_" + actor.id + ".json"), JsonSerializer.Serialize(actor, options));
        }
        public static void SaveItem(Item item)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "ItemData");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "Item_" + item.id + ".json"), JsonSerializer.Serialize(item, options));
        }
    }
}
