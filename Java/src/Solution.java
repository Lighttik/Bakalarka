import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class Solution implements Comparable {

    Integer value;
    String priklad;
    List<String> stones;
    List<Solution> childs = new ArrayList<>();

    public Solution(int value,String priklad,List<String> stones){
        this.value = value;
        this.priklad = priklad;
        this.stones = stones;
    }

    @Override
    public int compareTo(Object o) {
        if (o instanceof Solution){
            Solution sol = (Solution) o;
            if (value > sol.value) return 1;
            else if (value < sol.value) return -1;
            //return value.compareTo(sol.value);
        }
        return 0;
    }

    @Override
    public String toString() {
        StringBuilder pom = new StringBuilder();
        //String pom = priklad + "[" + value + "]" + " STONES: " + stones + " CHILDS: ";
        pom.append(priklad);
        pom.append("[");
        pom.append(value);
        pom.append("]");
        pom.append(" STONES: ");
        pom.append(stones);

        if (childs.size() > 0) {
            pom.append(" CHILDS: ");
            pom.append(" [ ");
            for (Solution s : childs) pom.append(s.toString());
            pom.append(" ]");
        }
        return pom.toString();
        //return priklad + "[" + value + "]" + " STONES: " + stones + " CHILDS: " + childs.forEach(x -> x.toString() + " ");
        /*
        pom += " [ ";
        for (Solution s : childs) pom += s.toString();
        pom += " ] ";
        return pom;*/
    }

    public void add_child(Solution solution){

        //System.out.println(this + "                   " + solution);
        if (this == solution) return;

        //if (!contains_child(this,solution)) {

        for (Solution s : solution.childs) {
            add_child(s);
        }
        if (!childs.contains(solution) && is_substone(solution.stones)){
            solution.childs = new ArrayList<>();
            solution.value = count_digits(solution.stones);
            childs.add(solution);
            value += solution.value;
        }

        //childs.add(solution);
        //value += solution.value;
        //}
    }

    public boolean is_substone(List<String> childs_stones){
        return Collections.indexOfSubList(stones , childs_stones) != -1;
    }

    public int count_digits(List<String> in){
        int out = 0;
        for (String s : in){
            out += Integer.parseInt(s);
        }
        return out;
    }

    public boolean contains_child(Solution solution,Solution what){
        System.out.println("halo");
        for (Solution s : solution.childs){
            if (s == what) return true;
            return contains_child(solution,what);
        }
        return false;
    }

    public void add_childs(List<Solution> solutions){
        for (Solution s : solutions) {
            childs.add(s);
            value += s.value;
        }
    }

    public void add_child_recursive(Solution solution){

        if (contains_all(this,solution)){
            childs.add(solution);
            return;
        }
        for (Solution s : solution.childs){
            add_child_recursive(s);
        }
    }

    public boolean contains_all(Solution stones_father,Solution stones_son){
        for (String i : stones_son.stones){
            if (Collections.frequency(stones_father.stones,i) < Collections.frequency(stones_son.stones,i)) return false;
        }
        return true;
    }
}
