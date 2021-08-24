using System;

namespace Game.Controller
{
    public static class Dice
    {
        public static bool Roll(int chance)
        {
            var die1 = new Random(Environment.ProcessId + (int)DateTime.Now.TimeOfDay.TotalMilliseconds);
            var roll1 = die1.Next(5) + 1;

            var die2 = new Random(Environment.ProcessId + (int)DateTime.Now.TimeOfDay.TotalSeconds);
            var roll2 = die2.Next(5) + 1;

            var threshold = 100.0 / chance * 12.0;

            return roll1 + roll2 < threshold;
        }
    }

}
