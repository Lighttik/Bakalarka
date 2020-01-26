using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class Runner
    {
        private static Combination comb = new Combination();
        private static Plocha plocha = new Plocha(1);
        private static List<List<Node>> Nodes_map;
        private static Purse purse = new Purse();

        //private static Player player1 = new Player();
        private static PlayerAi player2 = new PlayerAi(purse);
        
        public static void Main(string[] args) {
        Nodes_map = plocha.create_nodes();
        //comb.divide_to_strings(Arrays.asList("5","1","2","6","5","6","4"),Arrays.asList("","",""),0);
        //comb.find_best_move(Arrays.asList("5","1","2","6","5","6","4"));
        //comb.find_best_move(Arrays.asList("2","4","6","8","1","6"));
        //System.out.println(comb.divide_to_strings(Arrays.asList("6","6","1","2"),Arrays.asList("","",""),0,0,true));
        //comb.divide_to_strings(Arrays.asList("2","4","8","1","6"),Arrays.asList("","",""),0,0,true);
        //comb.find_best_move(Arrays.asList("7","2","9","8","1"));
        Console.WriteLine("START");
        //comb.find_best_move(new List<String> {"2", "4", "8"});
        //Console.WriteLine(String.Join("\n", comb.find_best_move(new List<String> {"2","4","8"})));
        Console.WriteLine("END");
        //72 + 9 = 81
        play_game();
    }

    public static void print_map(){

        foreach (var line in Nodes_map)
        {
            foreach (var node in line)
            {
                Console.Write(node);
                if (line.IndexOf(node) != line.Count - 1){
                    Console.Write(" | ");
                }
                else Console.WriteLine("");
            }

            for (var i = 0; i < line.Count * 4; i++){
                if (i == line.Count * 4 - 1) Console.WriteLine("");
                else Console.Write("-");
            }
        }
    }

    public static void play_game(){
        print_map();
        List<Solution> top10_moves = comb.find_best_move(player2.stones);
        Node start_node = null;
        String direction_placing;

        int i = 0;
        while (i < 4){
            if (plocha.is_empty()){
                foreach (var s in top10_moves)
                {
                    direction_placing = new Random().Next(2) > 0 ? "down" : "right";
                    start_node = plocha.middle;
                    player2.place_solution(start_node,direction_placing,s.stones);
                    break;
                    //}

                }
            }
            else{
                Solution best = null;
                String direction = "";
                foreach (var row in Nodes_map)
                {
                    foreach (var node in row)
                    {
                        if (!node.is_empty()){
                            Tuple<Tuple<Solution,Node>,String> current_with_direction = try_permutations_with_node(node,player2.stones);
                            Tuple<Solution,Node> current = null;
                            if (current_with_direction != null) current = current_with_direction.Item1;


                            if (current != null && best == null){
                                best = current.Item1;
                                start_node = current.Item2;
                                direction = current_with_direction.Item2;
                            }
                            else if (current != null && current.Item1.value > best.value){
                                best = current.Item1;
                                start_node = current.Item2;
                                direction = current_with_direction.Item2;
                            }
                        }
                    }
                }
                if (best != null){
                    player2.place_solution(start_node,direction,best.stones);
                    Console.WriteLine("PLACED: " + best + " DIRECTION: " + direction);
                }
                else{
                    Console.WriteLine("STONES: ");
                    player2.change_stones();
                    Console.WriteLine("STONES AFTER CHANGE : " + player2.stones);
                }
            }
            i++;
            Console.WriteLine();
            print_map();
        }
    }

    public static Tuple<Tuple<Solution,Node>,String> try_permutations_with_node(Node node, List<String> stones){
        stones.Add(node.name);
        List<Solution> solutions = comb.find_best_move(stones);
        stones.Remove(node.name);

        Tuple<Tuple<Solution,Node>,String> output;

        foreach (var s in solutions)
        {
            if (s.stones.Contains(node.name)){
                Tuple<Solution,Node> pom = try_down_placing(s,node);
                if (pom != null){
                    output = new Tuple<Tuple<Solution,Node>,String>(pom,"down");
                    return output;
                }
                pom = try_right_placing(s,node);
                if (pom != null){
                    output = new Tuple<Tuple<Solution,Node>,String>(pom,"right");
                    return output;
                }
            }
        }
        return null;
    }
    public static Tuple<Solution,Node> try_down_placing(Solution solution,Node start_node){
        Node actual_node = start_node;
        int index = -1;
        foreach (var stone in solution.stones)
        {
            index++;
            if (stone == start_node.name) break;
        }
        int pom_index = index;
        //pozrie smerom hore
        while (pom_index >= 0){
            if (actual_node == null || !actual_node.is_empty() && actual_node.name != solution.stones[pom_index]) return null;
            if (pom_index > 0) actual_node = actual_node.top;
            pom_index--;
        }
        Tuple<Solution,Node> output = new Tuple<Solution,Node>(solution,actual_node);
        actual_node = start_node;
        //pozrie smerom dole
        while (index < solution.stones.Count){
            if (actual_node == null || !actual_node.is_empty() && actual_node.name != solution.stones[index]) return null;
            actual_node = actual_node.bottom;
            index++;
        }
        //ak sa da polozit
        return output;
    }

    public static Tuple<Solution,Node> try_right_placing(Solution solution,Node start_node){
        Node actual_node = start_node;

        int index = -1;
        foreach (var stone in solution.stones)
        {
            index++;
            if (stone == start_node.name) break;
        }
        int pom_index = index;
        //pozrie smerom v lavo
        while (pom_index >= 0){
            if (actual_node == null || !actual_node.is_empty() && actual_node.name != solution.stones[pom_index]) return null;
            if (pom_index > 0) actual_node = actual_node.left;
            pom_index--;
        }
        Tuple<Solution,Node> output = new Tuple<Solution,Node>(solution,actual_node);
        actual_node = start_node;
        //pozrie smerom v pravo
        while (index < solution.stones.Count){
            if (actual_node == null || !actual_node.is_empty() && actual_node.name != solution.stones[index]) return null;
            actual_node = actual_node.right;
            index++;
        }
        return output;
    }
    }
}