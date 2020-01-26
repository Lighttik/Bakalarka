public class Node {
    public Node left;
    public Node top;
    public Node bottom;
    public Node right;
    public String name;
    public NodeType type;

    enum NodeType{
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
        if ("top".equals(side)){
            top = node;
            node.bottom = this;
        }
        else if ("left".equals(side)){
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

    @Override
    public String toString() {
        return name;
    }

    public boolean is_empty() {
        try {
            int i = Integer.parseInt(name);
            return false;
        } catch (NumberFormatException nfe) {
            return true;
        }
    }
}
