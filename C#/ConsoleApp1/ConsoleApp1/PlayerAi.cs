using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class PlayerAi
    {
        public List<string> stones = new List<string>();
        public Purse purse;

        public PlayerAi(Purse purse){
            //this.stones = stones;
            this.purse = purse;
            get_rocks();
        }


        public void place_solution(Node start_node,String direction_placing,List<String> stones){
            Node actual = start_node;
            foreach (var num in stones)
            {
                actual.name = num;
                actual = direction_placing == "down" ? actual.bottom : actual.right;
            }
            remove_rocks(stones);
            get_rocks();
        }

        public void remove_rocks(List<string> rocks){
            foreach (var rock in rocks)
            {
                stones.Remove(rock);
            }
        }

        public void get_rocks(){
            while (stones.Count != 7){
                String stone = purse.give_stone();
                if (stone == null) break;
                stones.Add(purse.give_stone());
            }
        }

        public void change_stones(){
            List<string> new_stones = new List<string>();
            foreach (var s in stones)
            {
                if (!new_stones.Contains(s) && s != "0"){
                    new_stones.Add(s);
                }
                else{
                    new_stones.Add(purse.exchange_stone(s));
                }
            }
        }
    }
}