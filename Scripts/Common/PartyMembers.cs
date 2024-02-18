using System;
using Godot;

namespace RustyRedemption.Common;

public enum PartyMembers
{
    KANAKO = 0, CLOVER = 1
}

public static class PartyMemberData
{
    public static readonly PartyMember KANAKO = new("Kanako", new Color(1.0f, 1.0f, 1.0f));
    public static readonly PartyMember CLOVER = new("Clover", new Color(1.0f, 1.0f, 0.0f));

    public static PartyMember Of(PartyMembers enumValue)
    {
        return enumValue switch
        {
            PartyMembers.KANAKO => KANAKO,
            PartyMembers.CLOVER => CLOVER,
            _ => null
        };
    }

    public static PartyMember OfOpposite(PartyMembers enumValue)
    {
        return enumValue switch
        {
            PartyMembers.KANAKO => CLOVER,
            PartyMembers.CLOVER => KANAKO,
            _ => null
        };
    }

    public static PartyMembers GetOpposite(PartyMembers enumValue)
    {
        return enumValue switch
        {
            PartyMembers.KANAKO => PartyMembers.CLOVER,
            PartyMembers.CLOVER => PartyMembers.KANAKO
        };
    }
}

public class PartyMember
{
    public readonly string Name;
    public readonly Color Color;

    public PartyMember(string name, Color color)
    {
        Name = name;
        Color = color;
    }
}