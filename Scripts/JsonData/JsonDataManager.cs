using System;
using RLNET;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class JsonDataManager
    {
        private static JsonSerializerSettings options;
        private static string actorPath { get; set; }
        private static string itemPath { get; set; }
        public static int totalEntity = 0;
        public JsonDataManager()
        {
            options = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            actorPath = Path.Combine(Environment.CurrentDirectory, "MonsterData");
            itemPath = Path.Combine(Environment.CurrentDirectory, "ItemData");
        }
        public static Entity ReturnEntity(int uID, int type)
        {
            totalEntity++;
            switch (type)
            {
                case 0:
                    {
                        string pullData = File.ReadAllText(Path.Combine(actorPath, "Monster_" + uID + ".json"));
                        return new Entity(JsonConvert.DeserializeObject<Entity>(pullData, options));
                    }
                case 1:
                    {
                        string pullData = File.ReadAllText(Path.Combine(itemPath, "Item_" + uID + ".json"));
                        return new Entity(JsonConvert.DeserializeObject<Entity>(pullData, options));
                    }
            }
            return null;
        }
        public static void SaveActor(Entity actor)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "MonsterData");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "Monster_" + actor.uID + ".json"), JsonConvert.SerializeObject(actor, options));
        }
        public static void SaveItem(Entity item)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "ItemData");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "Item_" + item.uID + ".json"), JsonConvert.SerializeObject(item, options));
        }
    }
}
