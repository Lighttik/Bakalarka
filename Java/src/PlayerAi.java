import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class PlayerAi {
    public List<String> stones = new ArrayList<>();
    public Purse purse;

    PlayerAi(Purse purse){
        //this.stones = stones;
        this.purse = purse;
        get_rocks();
    }


    public void place_solution(Node start_node,String direction_placing,List<String> stones){
        Node actual = start_node;
        for (String num: stones){
            actual.name = String.valueOf(num);

            actual = direction_placing.equals("down") ? actual.bottom : actual.right;
        }
        remove_rocks(stones);
        get_rocks();
    }

    public void remove_rocks(List<String> rocks){
        stones.removeAll(rocks);
    }

    public void get_rocks(){
        while (stones.size() != 7){
            String stone = purse.give_stone();
            if (stone == null) break;
            else stones.add(purse.give_stone());
        }
    }

    public void change_stones(){
        List<String> new_stones = new ArrayList<>();
        for (String s : stones){
            if (!new_stones.contains(s) && !s.equals("0")){
                new_stones.add(s);
            }
            else{
                new_stones.add(purse.exchange_stone(s));
            }
        }
    }
}
