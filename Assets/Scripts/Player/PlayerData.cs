using System;

[Serializable]
public class PlayerData
{
    public string Name;
    public string DateTime;
    public float CurrentHealth;
    public float MaxHeath;
    public Position Position;
}

[Serializable]
public class Position
{
    public float X;
    public float Y;
    public float Z;
}