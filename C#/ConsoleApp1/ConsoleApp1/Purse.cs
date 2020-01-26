using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class Purse
    {
        List<string> stones = new List<string>();

        public Purse()
        {
            List<string> sorted = new List<string>
            {
                "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0",
                "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1",
                "2", "2", "2", "2", "2", "2", "2", "2", "2", "2", "2",
                "3", "3", "3", "3", "3", "3", "3", "3", "3", "3", "3",
                "4", "4", "4", "4", "4", "4", "4", "4", "4", "4", "4",
                "5", "5", "5", "5", "5", "5", "5", "5", "5", "5", "5",
                "6", "6", "6", "6", "6", "6", "6", "6", "6", "6", "6",
                "7", "7", "7", "7", "7", "7", "7", "7", "7", "7", "7",
                "8", "8", "8", "8", "8", "8", "8", "8", "8", "8", "8",
                "9", "9", "9", "9", "9", "9", "9", "9", "9", "9", "9"
            };
            //,"X","X","X"))
            while (sorted.Count > 0)
            {
                int rand = new Random().Next(sorted.Count);
                stones.Add(sorted[rand]);
                sorted.RemoveAt(rand);
            }
        }

        public String give_stone(){
            if (stones.Count > 0)
            {
                var output = stones[0];
                stones.RemoveAt(0);
                return output;
            }
            return null;
        }
        public String exchange_stone(String stone){
            stones.Insert(new Random().Next(stones.Count),stone);
            return give_stone();
        }
    }
}