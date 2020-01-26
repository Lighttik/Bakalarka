using System;

namespace ClassLibrary1
{
    public class Node
    {
        public Node left;
        public Node top;
        public Node bottom;
        public Node right;
        public String name;
        public NodeType type;
        
        public enum NodeType{
            DOUBLE,
            TRIPLE,
            DOUBLE_WHOLE,
            TRIPLE_WHOLE,
            REGULAR,
            START
        }
        
        public Node(String name){
            this.name = name;
        }
        
        public void add_neightbour(Node node, String side){
            if ("top" == side){
                top = node;
                node.bottom = this;
            }
            else if ("left" == side){
                left = node;
                node.right = this;
            }
        }
        public void add_neightbour(Node top,Node left){
            this.top = top;
            top.bottom = this;
            this.left = left;
            left.right = this;
        }

        public override string ToString()
        {
            return name;
        }

        public bool is_empty() {
            try {
                int i = Convert.ToInt32(name);
                return false;
            } catch{
                return true;
            }
        }
    }
}