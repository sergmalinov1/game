﻿namespace Server.Services;

public interface IPlayerService
{
    void DoSomething();
}

public class PlayerService : IPlayerService
{
    public void DoSomething()
    {
        Console.WriteLine("hey!");
    }

}
