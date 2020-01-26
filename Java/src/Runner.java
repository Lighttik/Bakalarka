import javafx.util.Pair;

import java.util.*;

public class Runner {
    private static Combination comb = new Combination();
    private static Plocha plocha = new Plocha(1);
    private static List<ArrayList<Node>> Nodes_map;
    private static Purse purse = new Purse();

    private static Player player1 = new Player();
    private static PlayerAi player2 = new PlayerAi(purse);

    public static void main(String[] args) {
        Nodes_map = plocha.create_nodes();
        //comb.divide_to_strings(Arrays.asList("5","1","2","6","5","6","4"),Arrays.asList("","",""),0);
        //comb.find_best_move(Arrays.asList("5","1","2","6","5","6","4"));
        //comb.find_best_move(Arrays.asList("2","4","6","8","1","6"));
        //System.out.println(comb.divide_to_strings(Arrays.asList("6","6","1","2"),Arrays.asList("","",""),0,0,true));
        //comb.divide_to_strings(Arrays.asList("2","4","8","1","6"),Arrays.asList("","",""),0,0,true);
        //comb.find_best_move(Arrays.asList("7","2","9","8","1"));
        comb.find_best_move(Arrays.asList("2","4","8"));
        //72 + 9 = 81
        //play_game();
    }

    public static void print_map(){

        for (ArrayList<Node> line : Nodes_map){
            for (Node node : line){
                System.out.print(node);
                if (line.indexOf(node) != line.size() - 1){
                    System.out.print(" | ");
                }
                else System.out.println("");
            }

            for (int i = 0; i < line.size() * 4;i++){
                if (i == line.size() * 4 - 1) System.out.println("");
                else System.out.print("-");
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
                for (Solution s : top10_moves){
                    direction_placing = new Random().nextInt(2) > 0 ? "down" : "right";
                    start_node = plocha.middle;
                    player2.place_solution(start_node,direction_placing,s.stones);
                    break;
                    //}

                }
            }
            else{
                Solution best = null;
                String direction = "";
                for (List<Node> row : Nodes_map){
                    for (Node node : row){
                        if (!node.is_empty()){
                            Pair<Pair<Solution,Node>,String> current_with_direction = try_permutations_with_node(node,player2.stones);
                            Pair<Solution,Node> current = null;
                            if (current_with_direction != null) current = current_with_direction.getKey();


                            if (current != null && best == null){
                                best = current.getKey();
                                start_node = current.getValue();
                                direction = current_with_direction.getValue();
                            }
                            else if (current != null && current.getKey().value > best.value){
                                best = current.getKey();
                                start_node = current.getValue();
                                direction = current_with_direction.getValue();
                            }
                        }
                    }
                }
                if (best != null){
                    player2.place_solution(start_node,direction,best.stones);
                    System.out.println("PLACED: " + best + " DIRECTION: " + direction);
                }
                else{
                    System.out.println("STONES: ");
                    player2.change_stones();
                    System.out.println("STONES AFTER CHANGE : " + player2.stones);
                }
            }
            i++;
            System.out.println();
            print_map();
        }
    }

    public static Pair<Pair<Solution,Node>,String> try_permutations_with_node(Node node, List<String> stones){
        stones.add(node.name);
        List<Solution> solutions = comb.find_best_move(stones);
        stones.remove(node.name);

        Pair<Pair<Solution,Node>,String> out;

        for (Solution s : solutions){
            if (s.stones.contains(node.name)){
                Pair<Solution,Node> pom = try_down_placing(s,node);
                if (pom != null){
                    out = new Pair<>(pom,"down");
                    return out;
                }
                pom = try_right_placing(s,node);
                if (pom != null){
                    out = new Pair<>(pom,"right");
                    return out;
                }
            }
        }
        return null;
    }
    public static Pair<Solution,Node> try_down_placing(Solution solution,Node start_node){
        Node actual_node = start_node;
        int index = -1;
        for (String stone : solution.stones){
            index++;
            if (stone.equals(start_node.name)) break;
        }
        int pom_index = index;
        //pozrie smerom hore
        while (pom_index >= 0){
            if (actual_node == null || !actual_node.is_empty() && !actual_node.name.equals(solution.stones.get(pom_index))) return null;
            if (pom_index > 0) actual_node = actual_node.top;
            pom_index--;
        }
        Pair<Solution,Node> out = new Pair<>(solution,actual_node);
        actual_node = start_node;
        //pozrie smerom dole
        while (index < solution.stones.size()){
            if (actual_node == null || !actual_node.is_empty() && !actual_node.name.equals(solution.stones.get(index))) return null;
            actual_node = actual_node.bottom;
            index++;
        }
        //ak sa da polozit
        return out;
    }

    public static Pair<Solution,Node> try_right_placing(Solution solution,Node start_node){
        Node actual_node = start_node;

        int index = -1;
        for (String stone : solution.stones){
            index++;
            if (stone.equals(start_node.name)) break;
        }
        int pom_index = index;
        //pozrie smerom v lavo
        while (pom_index >= 0){
            if (actual_node == null || !actual_node.is_empty() && !actual_node.name.equals(solution.stones.get(pom_index))) return null;
            if (pom_index > 0) actual_node = actual_node.left;
            pom_index--;
        }
        Pair<Solution,Node> out = new Pair<>(solution,actual_node);
        actual_node = start_node;
        //pozrie smerom v pravo
        while (index < solution.stones.size()){
            if (actual_node == null || !actual_node.is_empty() && !actual_node.name.equals(solution.stones.get(index))) return null;
            actual_node = actual_node.right;
            index++;
        }
        return out;
    }
}
