using System;

public interface IEmotable
{
    public event Action<MoodType> MoodChanged;
}
