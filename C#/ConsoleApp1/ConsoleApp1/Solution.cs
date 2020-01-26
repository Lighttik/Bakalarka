using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class Solution : IComparable
    {
        public int value;
        public String priklad;
        public List<String> stones;
        private ArrayList childs = new ArrayList();
        
        public Solution(int value,String priklad,List<String> stones){
            this.value = value;
            this.priklad = priklad;
            this.stones = stones;
        }
        
        public int CompareTo(object obj)
        {
            if (obj is Solution)
            {
                Solution sol = (Solution) obj;
                if (value > sol.value) return 1;
                else if (value < sol.value) return -1;
            }
            return 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(priklad);
            sb.Append("[");
            sb.Append(value);
            sb.Append("]");
            sb.Append(" STONES: ");
            sb.Append("[" + String.Join(",",stones) + "]");
            if (childs.Count > 0) {
                sb.Append(" CHILDS: ");
                sb.Append(" [ ");
                foreach (var sol in childs)
                {
                    sb.Append(sol);
                }

                sb.Append(" ]");
            }
            return sb.ToString();
        }
        public void add_child(Solution solution){
            //System.out.println(this + "                   " + solution);
            if (this == solution) return;

            //if (!contains_child(this,solution)) {

            foreach (Solution s in solution.childs)
            {
                add_child(s);
            }

            //Console.WriteLine(is_substone(solution.stones));
            if (!childs.Contains(solution) && is_substone(solution.stones))
            {
                solution.childs = new ArrayList();
                solution.value = count_digits(solution.stones);
                childs.Add(solution);
                value += solution.value;
            }
            
        }
        
        public bool is_substone(List<String> childs_stones){
            return stones
                .Where((item, index) => index <= stones.Count - childs_stones.Count)
                .Select((item, index) => stones.Skip(index).Take(childs_stones.Count))
                .Any(part => part.SequenceEqual(childs_stones));
        }

        public int count_digits(List<String> input){
            int outer = 0;
            foreach (var s in input)
            {
                outer += Convert.ToInt32(s);
            }
            return outer;
        }
    }
}