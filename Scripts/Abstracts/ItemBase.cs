using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class ItemBase : IDraw, IDescription
    {
        public int iType { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public char character { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public RLColor fColor { get; set; }
        public RLColor bColor { get; set; }
        public bool opaque { get; set; }
        public bool equipable = false;
        public bool useable = false;
        public ItemBase(ItemBaseData data, int _x, int _y)
        {
            iType = data.iType;
            x = _x;
            y = _y;
            character = data.character;
            name = data.name;
            description = data.description;
            fColor = data.fColor;
            bColor = RLColor.Black;
            opaque = false;
        }
        public string Describe() { return name + ": " + description + " Equipable: " + equipable + " Useable: " + useable; }
        public void Draw(RLConsole console) { console.Set(x, y, fColor, bColor, character); }
    }
    [Serializable]
    public class Armor : ItemBase, IEquipable
    {
        int ac { get; set; }
        public Armor(ArmorData data, int _x, int _y) : base(data, _x, _y)
        {
            ac = data.ac;
            equipable = true;
        }
        public void Equip(ActorBase actor) { actor.ac += ac; }
        public void Unequip(ActorBase actor) { actor.ac -= ac; }
    }
    [Serializable]
    public class Weapon : ItemBase, IEquipable
    {
        AtkData atkData { get; set; }
        public Weapon(WeaponData data, int _x, int _y) : base(data, _x, _y)
        {
            atkData = data.atkData;
            equipable = true;
        }
        public void Equip(ActorBase actor) { }
        public void Unequip(ActorBase actor) { }
    }
    [Serializable]
    public class Comestible : ItemBase, IUseable
    {
        int nutritionalValue { get; set; }
        public Comestible(ComestibleData data, int _x, int _y) : base(data, _x, _y)
        {
            nutritionalValue = data.nutritionalValue;
            useable = true;
        }
        public void Use(ActorBase user) { }
    }
    [Serializable]
    public class Potion : ItemBase, IUseable
    {
        public Potion(PotionData data, int _x, int _y) : base(data, _x, _y)
        {
            useable = true;
        }
        public void Use(ActorBase user) { }
    }
    public interface ItemBaseData
    {
        int id { get; set; }
        int iType { get; set; }
        char character { get; set; }
        string name { get; set; }
        string description { get; set; }
        RLColor fColor { get; set; }
    }
    [Serializable]
    public struct ArmorData : ItemBaseData
    {
        public int id { get; set; }
        public int iType { get; set; }
        public char character { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public RLColor fColor { get; set; }
        public int ac { get; set; }
        public ArmorData(int _id, int _iType, char _character, string _name, string _description, RLColor _fColor, int _ac)
        {
            id = _id;
            iType = _iType;
            character = _character;
            name = _name;
            description = _description;
            fColor = _fColor;
            ac = _ac;
        }
    }
    [Serializable]
    public struct WeaponData : ItemBaseData
    {
        public int id { get; set; }
        public int iType { get; set; }
        public char character { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public RLColor fColor { get; set; }
        public AtkData atkData { get; set; }
        public WeaponData(int _id, int _iType, char _character, string _name, string _description, RLColor _fColor, AtkData _atkData)
        {
            id = _id;
            iType = _iType;
            character = _character;
            name = _name;
            description = _description;
            fColor = _fColor;
            atkData = _atkData;
        }
    }
    [Serializable]
    public struct ComestibleData : ItemBaseData
    {
        public int id { get; set; }
        public int iType { get; set; }
        public char character { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public RLColor fColor { get; set; }
        public int nutritionalValue { get; set;}
        public ComestibleData(int _id, int _iType, char _character, string _name, string _description, RLColor _fColor, int _nutritionalValue)
        {
            id = _id;
            iType = _iType;
            character = _character;
            name = _name;
            description = _description;
            fColor = _fColor;
            nutritionalValue = _nutritionalValue;
        }
    }
    [Serializable]
    public struct PotionData : ItemBaseData
    {
        public int id { get; set; }
        public int iType { get; set; }
        public char character { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public RLColor fColor { get; set; }
        public PotionData(int _id, int _iType, char _character, string _name, string _description, RLColor _fColor)
        {
            id = _id;
            iType = _iType;
            character = _character;
            name = _name;
            description = _description;
            fColor = _fColor;
        }
    }
}
