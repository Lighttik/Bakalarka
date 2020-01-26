import javafx.util.Pair;

import javax.xml.soap.Node;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

public class Combination {
    private int max = 0;
    private List<Operators> operations = Arrays.asList(Operators.values());
    private List<Solution> solutions = new ArrayList<>();

    /*public static void main(String[] args) {
        find_best_move("1 2 3 4 5 6 7 8 2 5");
    }*/

    /**
     *
     * @param input
     */
    public List<Solution> find_best_move(List<String> input){
        solutions = new ArrayList<>();
        permutations(new ArrayList<>(),input);
        System.out.println("======================================");
        //System.out.println(solutions);
        Collections.reverse(solutions);
        int i = 0;
        for (Solution s : solutions) System.out.println(i++ + ".     " + s);
        System.out.println(input);
        return solutions;
    }

    public void permutations(List<String> path,List<String> nodes){
        //dostane vstup a vyrobi vsetky mozne permutacie kamenov

        for (String node: nodes){
            if (Collections.frequency(path, node) < Collections.frequency(nodes, node)) {
                path.add(node);
                Solution solution = divide_to_strings(path,new ArrayList<>(Arrays.asList("","","")),0,0,true);
                if (solution != null){
                    check_best(solution);
                }


                permutations(path, nodes);
                path.remove(node);
            }
        }
    }


    //private static void divide_to_strings(List<String> nodes,String one,String two,String three,int index){
    public Solution divide_to_strings(List<String> nodes,List<String> divides,int index,int level,boolean jumper){
        level += 1;
        for (String s : divides){
            if (s.length() > 0 && s.charAt(0) == '0') return null;
        }

        if (nodes.size() == 0) return null;
        List<String> nodes_start = new ArrayList<>(nodes);
        List<String> divides_start = new ArrayList<>(divides);

        if (nodes_start.size() > 0){
            Solution jump = null;

            String node = nodes_start.remove(0);
            //preskoci
            if (jumper) jump = divide_to_strings(nodes_start,divides_start,index,level++,true);

            String replacor = divides_start.get(index);

            if (replacor.equals("") && node.equals("0")) return null;

            replacor += node;
            divides_start.set(index, replacor);


            Solution solution = control(divides_start.get(0),divides_start.get(1),divides_start.get(2));



            //bud ich da dokopy
            Solution unite = divide_to_strings(nodes_start,divides_start,index,level++,false);
            //alebo rozdeli
            Solution divide = index + 1 < 3 ? divide_to_strings(nodes_start,divides_start,index + 1 ,level++,false) : null ;
            Solution pom = choose_parrent1(unite,divide,jump,solution);
            return pom;
        }
        return null;
    }
    public boolean contains_all(Solution stones_father,Solution stones_son){
        for (String i : stones_son.stones){
            if (Collections.frequency(stones_father.stones,i) < Collections.frequency(stones_son.stones,i)) return false;
        }
        return true;
    }

    private int toInt(String val) {
        return val.equals("") ? -1 :Integer.parseInt(val);
    }

    public Solution choose_parrent1(Solution parent,Solution parent2 , Solution parent3 , Solution actual){
        // ak ma tri nadpriklady
        Solution out = null;
        List<Solution> parents = new ArrayList<Solution>(Arrays.asList(parent,parent2,parent3));
        if (Collections.frequency(parents,null) == 0){
            if (parent2.value >= parent.value && parent2.value >= parent3.value){
                parents.remove(parent2);
                out = parent2;
            }
            else if (parent3.value >= parent.value){
                parents.remove(parent3);
                out = parent3;
            }
            else {
                parents.remove(parent);
                out = parent;
            }

            for (Solution s : parents){
                if (contains_all(out,s)){
                    out.add_child(s);
                }
            }

        }
        //ak dvaja
        else if (Collections.frequency(parents,null) == 1){
            parents.remove(null);
            if (parents.get(0).value >= parents.get(1).value) {

                out = parents.remove(0); //odstrani lepsieho
            }
            else {
                out = parents.remove(1);
            }
            for (Solution s : parents){
                if (contains_all(out,s)){
                    out.add_child(s);
                }
            }
        }
        //ak iba jeden
        else if (Collections.frequency(parents,null) == 2){
            parents.removeAll(Collections.singleton(null));
            out = parents.get(0);
        }
        //ak ziaden
        else if (actual != null){
            out = actual;
        }

        if (actual != null && out != null){
            if (contains_all(out,actual)){
                out.add_child(actual);
            }
        }
        return out;
    }

    public Solution control(String one, String two,String three){

        int one_num = toInt(one);
        int two_num = toInt(two);
        int three_num = toInt(three);

        if (two_num == -1 && three_num == -1) return null;


        List<Solution> solution_values;
        if (three_num == -1){
            solution_values = check_unary_solution(one_num,two_num);
        }
        else {
            solution_values = check_binary_solution(one_num,two_num,three_num);
        }
        if (solution_values.size() > 0){
            Solution parent = solution_values.remove(0);

            for (Solution s : solution_values) parent.add_child(s);
            return parent;
        }
        return null;
    }

    public void check_best(Solution solution){
        boolean is_there = true;


        for (Solution s : solutions){
            if (s.priklad.equals(solution.priklad)) is_there = false;
        }

        if (solutions.size() < 10 && is_there){
            solutions.add(solution);
        }
        else if (solutions.get(0).value < solution.value && is_there){
            solutions.remove(0);
            solutions.add(solution);
        }
        Collections.sort(solutions);
    }


    public int count_digits(int in){
        int out = 0;
        while (in > 0){
            out += in % 10;
            in /= 10;
        }
        return out;
    }
    public List<String> make_stones_list(int one,int two,int three){
        String s = one + Integer.toString(two);
        if (three >= 0) s += Integer.toString(three);
        List<String> out = new ArrayList<>();
        for (char c : s.toCharArray()){
            out.add(Character.toString(c));
        }
        return out;
    }

    public List<Solution> check_unary_solution(int one_num,int two_num){
        List<Solution> out = new ArrayList<>();
        if (check_operator_in_play(Operators.POWER) && check_power_of(one_num,two_num)){
            out.add(new Solution(count_digits(one_num) + count_digits(two_num),one_num + " ** " + two_num,make_stones_list(one_num,two_num,-1)));
        }
        if (check_operator_in_play(Operators.ROOT) && check_root(one_num,two_num)){
            out.add(new Solution(count_digits(one_num) + count_digits(two_num),one_num + " /- " + two_num,make_stones_list(one_num,two_num,-1)));
        }
        return out;
    }
    public List<Solution> check_binary_solution(int one_num,int two_num,int three_num){
        List<Solution> out = new ArrayList<>();
        if (check_operator_in_play(Operators.PLUS) && check_plus(one_num,two_num,three_num)){
            out.add(new Solution(count_digits(one_num) + count_digits(two_num),one_num + " + " + two_num + " = " + three_num,make_stones_list(one_num,two_num,three_num)));
        }
        if (check_operator_in_play(Operators.MINUS) && check_minus(one_num,two_num,three_num)){
            out.add(new Solution(count_digits(one_num) + count_digits(two_num),one_num + " - " + two_num + " = " + three_num,make_stones_list(one_num,two_num,three_num)));
        }
        if (check_operator_in_play(Operators.MULTIPLY) && check_multiply(one_num,two_num,three_num)){
            out.add(new Solution(count_digits(one_num) + count_digits(two_num),one_num + " * " + two_num + " = " + three_num,make_stones_list(one_num,two_num,three_num)));
        }
        if (check_operator_in_play(Operators.DIVIDE) && check_divide(one_num,two_num,three_num)){
            out.add(new Solution(count_digits(one_num) + count_digits(two_num),one_num + " / " + two_num + " = " + three_num,make_stones_list(one_num,two_num,three_num)));
        }
        return  out;
    }

    public boolean check_operator_in_play(Operators o){
        return operations.contains(o);
    }

    public boolean check_power_of(int one_num,int two_num){
        return Math.pow(one_num,2) == two_num || Math.pow(one_num,3) == two_num;
    }
    public boolean check_root(int one_num,int two_num){
        return Math.sqrt(one_num) == two_num || Math.cbrt(one_num) == two_num;
    }
    public boolean check_plus(int one_num,int two_num,int three_num){
        return one_num + two_num == three_num;
    }
    public boolean check_minus(int one_num,int two_num,int three_num){
        return one_num - two_num == three_num;
    }
    public boolean check_multiply(int one_num,int two_num,int three_num){
        return one_num * two_num == three_num;
    }
    public boolean check_divide(int one_num,int two_num,int three_num){
        return (double)one_num / two_num == three_num;
    }
}
