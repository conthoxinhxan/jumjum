using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public string Name;
    public string DateTime;
    public float CurrentHealth;
    public float MaxHeath;
    public Position Position;
    public PlayerInventoryData Inventory;
}

[Serializable]
public class PlayerInventoryData
{
    public List<ItemBase> Items;
}

[Serializable]
public class Position
{
    public float X;
    public float Y;
    public float Z;
}
