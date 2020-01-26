import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Random;

public class Purse {
    List<String> stones = new ArrayList<>();
    Purse(){
       List<String> sorted = new ArrayList<>(Arrays.asList("0","0","0","0","0","0","0","0","0","0","0",
               "1","1","1","1","1","1","1","1","1","1","1",
               "2","2","2","2","2","2","2","2","2","2","2",
               "3","3","3","3","3","3","3","3","3","3","3",
               "4","4","4","4","4","4","4","4","4","4","4",
               "5","5","5","5","5","5","5","5","5","5","5",
               "6","6","6","6","6","6","6","6","6","6","6",
               "7","7","7","7","7","7","7","7","7","7","7",
               "8","8","8","8","8","8","8","8","8","8","8",
               "9","9","9","9","9","9","9","9","9","9","9"));
               //,"X","X","X"))
        while (sorted.size() > 0){
            stones.add(sorted.remove(new Random().nextInt(sorted.size())));
        }
    }

    public String give_stone(){
        if (stones.size() > 0) return stones.remove(0);
        return null;
    }
    public String exchange_stone(String stone){
        stones.add(new Random().nextInt(stones.size()),stone);
        return give_stone();
    }
}
