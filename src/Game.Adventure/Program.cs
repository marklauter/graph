﻿using System;

namespace Game.Adventure
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Application();
            try
            {
                app.Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"fatal error {ex.GetType()}, '{ex.Message}'");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
