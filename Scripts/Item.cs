using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Item : IDraw, IDescription
    {
        public int id { get; set; }
        public int type { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public char character { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string fColor { get; set; }
        public string bColor { get; set; }
        public bool opaque { get; set; }
        public bool equipped { get; set; }
        public string spacer { get; set; }
        public string slot { get; set; }
        public int ac { get; set; }
        public int nutritionalValue { get; set; }
        public AtkData atkData { get; set; }
        public Item(int _id, int _x, int _y, int _type, char _character, string _name, string _description, string _fColor, int _ac, string _slot, int _nutritionalValue, AtkData _atkData)
        {
            id = _id;
            spacer = " + ";
            equipped = false;
            x = _x;
            y = _y;
            type = _type;
            character = _character;
            name = _name;
            description = _description;
            fColor = _fColor;
            bColor = "Black";
            opaque = false;
            slot = _slot;
            ac = _ac;
            nutritionalValue = _nutritionalValue;
            atkData = _atkData;
        }
        public Item(Item item, int _x, int _y)
        {
            x = _x;
            y = _y;
            id = item.id;
            spacer = " + ";
            equipped = false;
            type = item.type;
            character = item.character;
            name = item.name;
            description = item.description;
            fColor = item.fColor;
            bColor = "Black";
            opaque = false;
            slot = item.slot;
            ac = item.ac;
            nutritionalValue = item.nutritionalValue;
            atkData = item.atkData;
        }
        public Item() { }
        public string Describe() { return name + ": " + spacer + "Ac: " + ac + spacer + "Slot: " + slot + spacer + "Equipped: " + equipped + spacer + description; }
        public void Draw(RLConsole console) { console.Set(x, y, ColorFinder.ColorPicker(fColor), ColorFinder.ColorPicker(bColor), character); }
    }
}
