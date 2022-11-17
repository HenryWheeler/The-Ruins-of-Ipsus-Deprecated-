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
        private static string path { get; set; }
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

            path = Path.Combine(Environment.CurrentDirectory, "EntityData");
        }
        public static Entity ReturnEntity(int uID, int type)
        {
            totalEntity++;
            string pullData = File.ReadAllText(Path.Combine(path, "Entity-" + type + "-" + uID + ".json"));
            return new Entity(JsonConvert.DeserializeObject<Entity>(pullData, options));
        }
        public static void SaveEntity(Entity entity, int type)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "Entity-" + type + "-" + entity.uID + ".json"), JsonConvert.SerializeObject(entity, options));
        }
    }
}
