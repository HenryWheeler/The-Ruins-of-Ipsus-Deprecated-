﻿using System;
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
            if (File.Exists(Path.Combine(savePath, "SaveFile.json"))) { savePresent = true; }
        }
        public static void CreateSave()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, directoryName);
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            Visibility[,] visibility = new Visibility[Program.gameMapWidth, Program.gameMapHeight];
            List<Entity> actors = new List<Entity>();
            List<Entity> items = new List<Entity>();
            List<Entity> terrain = new List<Entity>();
            foreach(Entity tile in World.tiles)
            {
                if (tile != null && tile.GetComponent<Traversable>() != null)
                {
                    Vector2 vector2 = tile.GetComponent<Coordinate>().vector2;
                    Visibility visibility1 = tile.GetComponent<Visibility>();
                    visibility1.entity = null;
                    visibility[vector2.x, vector2.y] = visibility1;
                    Traversable traversable = tile.GetComponent<Traversable>();
                    if (traversable.actorLayer != null) 
                    { 
                        if (traversable.actorLayer.GetComponent<ID>().id != 0) 
                        {
                            AI AI = CMath.ReturnAI(traversable.actorLayer);
                            AI.transitions.Clear();
                            AI.target = null;
                            actors.Add(traversable.actorLayer); 
                        } 
                    }
                    if (traversable.itemLayer != null) { items.Add(traversable.itemLayer); }
                    if (traversable.obstacleLayer != null) { terrain.Add(traversable.obstacleLayer); }
                }
            }
            

            SaveData data = new SaveData(World.depth, World.seedInt, RecordKeeper.record, Program.player, actors, items, terrain, visibility);

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
        public int depth { get; set; }
        public int seed { get; set; }
        public Record records { get; set; }
        public Entity player { get; set; }
        public List<Entity> actors = new List<Entity>();
        public List<Entity> items = new List<Entity>();
        public List<Entity> terrain = new List<Entity>();
        public Visibility[,] visibility { get; set; }
        public SaveData(int _depth, int _random, Record _records, Entity _player, List<Entity> _actors, List<Entity> _items, List<Entity> _terrain, Visibility[,] _visibility) 
        {
            depth = _depth;
            seed = _random;
            records = _records;
            player = _player;
            actors = _actors;
            items = _items;
            terrain = _terrain;
            visibility = _visibility;
        }
        public SaveData() { }
    }
}
