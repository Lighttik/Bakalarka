using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ClassLibrary1
{
    public class Combination
    {
        public enum Operators
        {
            PLUS,
            MINUS,
            MULTIPLY,
            DIVIDE,
            POWER,
            ROOT
        };
        
        private List<Operators> operations = new List<Operators> {Operators.PLUS,Operators.MINUS,Operators.DIVIDE,Operators.MULTIPLY,Operators.POWER,Operators.ROOT};
        List<Solution> solutions = new List<Solution>();
        public List<Solution> find_best_move(List<string> input)
        {
            solutions = new List<Solution>();
            permutations(new List<string>(), input);
            solutions.Reverse();
            int j = 0;
            foreach (var solution in solutions)
            {
                Console.WriteLine(j++ +  ".    " + solution);
            }

            return solutions;
        }

        public void permutations(List<String> path, List<String> nodes)
        {
            foreach (var node in nodes)
            {
                if (path.Count(x => x == node) < nodes.Count(x => x == node))
                {
                    path.Add(node);
                    Solution solution = divide_to_strings(path,new List<String> {"","",""},0,0,true);
                    if (solution != null){
                        check_best(solution);
                    }
                    permutations(path, nodes);
                    path.Remove(node);
                }
            }
        }
        
        public Solution divide_to_strings(List<string> nodes,List<string> divides,int index,int level,bool jumper){
            
            /*foreach (var node in nodes)
            {
                Console.Write(node + " ");
            }
            Console.WriteLine();
            foreach (var node in divides)
            {
                Console.Write(node + " ");
            }
            Console.WriteLine();*/
            level += 1;
            if (divides.Any(s => s.Length > 0 && s[0] == '0')) return null;

            if (nodes.Count == 0) return null;
            List<string> nodesStart = new List<string>(nodes);
            List<string> dividesStart = new List<string>(divides);

            if (nodesStart.Count > 0){
                Solution jump = null;

                String node = nodesStart[0];
                nodesStart.RemoveAt(0);
                
                //preskoci
                if (jumper) jump = divide_to_strings(nodesStart,dividesStart,index,level++,true);

                String replacer = dividesStart[index];

                if (replacer == "" && node == "0") return null;

                replacer += node;
                dividesStart[index] = replacer;


                Solution solution = Control(dividesStart[0],dividesStart[1],dividesStart[2]);



                //bud ich da dokopy
                Solution unite = divide_to_strings(nodesStart,dividesStart,index,level++,false);
                //alebo rozdeli
                Solution divide = index + 1 < 3 ? divide_to_strings(nodesStart,dividesStart,index + 1 ,level++,false) : null ;
                Solution pom = ChooseParent(unite,divide,jump,solution);
                return pom;
            }
            return null;
        }
        
        public bool contains_all(Solution stonesFather,Solution stonesSon){
            //Console.Write(stonesFather + " ||| " + stonesSon);
            foreach (var i in stonesSon.stones)
            {
                if (stonesFather.stones.Count(x => x == i) < stonesSon.stones.Count(x => x == i))
                {
                    //Console.WriteLine(" FALSE");
                    return false;
                }
            }
            //Console.WriteLine(" TRUE");
            return true;
        }

        public Solution ChooseParent(Solution parent, Solution parent2, Solution parent3, Solution actual)
        {
            // ak ma tri nadpriklady
            Solution outer = null;
            List<Solution> parents = new List<Solution> {parent, parent2, parent3};
            if (parents.Count(x => x == null) == 0){
                if (parent2.value >= parent.value && parent2.value >= parent3.value){
                    parents.Remove(parent2);
                    outer = parent2;
                }
                else if (parent3.value >= parent.value){
                    parents.Remove(parent3);
                    outer = parent3;
                }
                else {
                    parents.Remove(parent);
                    outer = parent;
                }

                foreach (var s in parents)
                {
                    if (contains_all(outer,s)){
                        outer.add_child(s);
                    }
                }
            }
            //ak dvaja
            else if (parents.Count(x => x == null) == 1){
                parents.Remove(null);
                if (parents[0].value >= parents[1].value) {

                    outer = parents[0]; //odstrani lepsieho
                    parents.RemoveAt(0);
                }
                else {
                    outer = parents[1];
                    parents.RemoveAt(1);
                }
                foreach (var s in parents){
                    if (contains_all(outer,s)){
                        outer.add_child(s);
                    }
                }
            }
            //ak iba jeden
            else if (parents.Count(x => x == null) == 2){
                parents.RemoveAll(x => x == null);
                outer = parents[0];
            }
            //ak ziaden
            else if (actual != null){
                outer = actual;
            }

            if (actual != null && outer != null){
                if (contains_all(outer,actual)){
                    outer.add_child(actual);
                }
            }
            return outer;
        }

        private Solution Control(string one, string two, string three){

            int oneNum = ToInt(one);
            int twoNum = ToInt(two);
            int threeNum = ToInt(three);

            if (twoNum == -1 && threeNum == -1) return null;


            List<Solution> solutionValues;
            if (threeNum == -1){
                solutionValues = check_unary_solution(oneNum,twoNum);
            }
            else {
                solutionValues = check_binary_solution(oneNum,twoNum,threeNum);
            }
            if (solutionValues.Count > 0){
                Solution parent = solutionValues[0];
                solutionValues.RemoveAt(0);
                foreach (var s in solutionValues) parent.add_child(s);
                return parent;
            }
            return null;
        }
        
        private static int ToInt(String val) {
            return val == "" ? -1 : Convert.ToInt32(val);
        }
        
        public void check_best(Solution solution){
            bool isThere = true;
            foreach (var s in solutions){
                if (s.priklad == solution.priklad) isThere = false;
            }

            if (solutions.Count < 10 && isThere){
                solutions.Add(solution);
            }
            else if (solutions[0].value < solution.value && isThere){
                solutions.RemoveAt(0);
                solutions.Add(solution);
            }
            solutions.Sort();
        }

        private List<Solution> check_unary_solution(int oneNum,int twoNum){
            List<Solution> outer = new List<Solution>();
            if (check_operator_in_play(Operators.POWER) && check_power_of(oneNum,twoNum)){
                outer.Add(new Solution(count_digits(oneNum) + count_digits(twoNum),oneNum + " ** " + twoNum,make_stones_list(oneNum,twoNum,-1)));
            }
            if (check_operator_in_play(Operators.ROOT) && check_root(oneNum,twoNum)){
                outer.Add(new Solution(count_digits(oneNum) + count_digits(twoNum),oneNum + " /- " + twoNum,make_stones_list(oneNum,twoNum,-1)));
            }
            return outer;
        }

        private List<Solution> check_binary_solution(int oneNum,int twoNum,int threeNum){
            List<Solution> outer = new List<Solution>();
            if (check_operator_in_play(Operators.PLUS) && check_plus(oneNum,twoNum,threeNum)){
                outer.Add(new Solution(count_digits(oneNum) + count_digits(twoNum),oneNum + " + " + twoNum + " = " + threeNum,make_stones_list(oneNum,twoNum,threeNum)));
            }
            if (check_operator_in_play(Operators.MINUS) && check_minus(oneNum,twoNum,threeNum)){
                outer.Add(new Solution(count_digits(oneNum) + count_digits(twoNum),oneNum + " - " + twoNum + " = " + threeNum,make_stones_list(oneNum,twoNum,threeNum)));
            }
            if (check_operator_in_play(Operators.MULTIPLY) && check_multiply(oneNum,twoNum,threeNum)){
                outer.Add(new Solution(count_digits(oneNum) + count_digits(twoNum),oneNum + " * " + twoNum + " = " + threeNum,make_stones_list(oneNum,twoNum,threeNum)));
            }
            if (check_operator_in_play(Operators.DIVIDE) && check_divide(oneNum,twoNum,threeNum)){
                outer.Add(new Solution(count_digits(oneNum) + count_digits(twoNum),oneNum + " / " + twoNum + " = " + threeNum,make_stones_list(oneNum,twoNum,threeNum)));
            }
            return outer;
        }

        private int count_digits(int input){
            int output = 0;
            while (input > 0){
                output += input % 10;
                input /= 10;
            }
            return output;
        }

        private List<String> make_stones_list(int one,int two,int three){
            String s = one + two.ToString();
            if (three >= 0) s += three.ToString();
            List<String> outer = new List<String>();
            foreach (var c in s)
            {
                outer.Add(Convert.ToString(c));
            }
            return outer;
        }

        private bool check_operator_in_play(Operators o){
            return operations.Contains(o);
        }

        private bool check_power_of(int oneNum,int twoNum){
            return Math.Pow(oneNum,2) == twoNum || Math.Pow(oneNum,3) == twoNum;
        }
        public bool check_root(int one_num,int two_num){
            return Math.Sqrt(one_num) == two_num || Math.Pow(one_num, (double) 1 / 3) == two_num;
        }
        public bool check_plus(int one_num,int two_num,int three_num){
            return one_num + two_num == three_num;
        }
        public bool check_minus(int one_num,int two_num,int three_num){
            return one_num - two_num == three_num;
        }
        public bool check_multiply(int one_num,int two_num,int three_num){
            return one_num * two_num == three_num;
        }
        public bool check_divide(int one_num,int two_num,int three_num){
            return (double)one_num / two_num == three_num;
        }
    }
    
}