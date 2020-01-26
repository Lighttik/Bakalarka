import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class Plocha {

    public List<ArrayList<String>> map = new ArrayList<ArrayList<String>>();
    public Node middle;

    public Plocha (int which){
        if (which == 1){
            map.add(new ArrayList<String>(Arrays.asList("T"," "," ","t"," "," "," ","T"," "," "," ","d"," "," ","T")));
            map.add(new ArrayList<String>(Arrays.asList(" "," ","d"," "," "," ","d"," ","d"," "," "," ","t"," "," ")));
            map.add(new ArrayList<String>(Arrays.asList(" ","t"," "," "," ","D"," "," "," "," "," "," "," ","d"," ")));
            map.add(new ArrayList<String>(Arrays.asList("d"," "," "," ","D"," "," ","d"," "," ","D"," "," "," ","d")));
            map.add(new ArrayList<String>(Arrays.asList(" "," "," ","D"," "," "," "," "," "," "," ","D"," "," "," ")));
            map.add(new ArrayList<String>(Arrays.asList(" "," "," "," "," ","t"," "," "," ","t"," "," ","D"," "," ")));
            map.add(new ArrayList<String>(Arrays.asList(" ","t"," "," "," "," ","d"," ","d"," "," "," "," ","d"," ")));
            map.add(new ArrayList<String>(Arrays.asList("T"," "," ","d"," "," "," ","S"," "," "," ","d"," "," ","T")));
            map.add(new ArrayList<String>(Arrays.asList(" ","d"," "," "," "," ","d"," ","d"," "," "," "," ","d"," ")));
            map.add(new ArrayList<String>(Arrays.asList(" "," ","D"," "," ","t"," "," "," ","t"," "," "," "," "," ")));
            map.add(new ArrayList<String>(Arrays.asList(" "," "," ","D"," "," "," "," "," "," "," ","D"," "," "," ")));
            map.add(new ArrayList<String>(Arrays.asList("t"," "," "," ","D"," "," ","d"," "," ","D"," "," "," ","d")));
            map.add(new ArrayList<String>(Arrays.asList(" ","d"," "," "," "," "," "," "," ","D"," "," "," ","t"," ")));
            map.add(new ArrayList<String>(Arrays.asList(" "," ","t"," "," "," ","d"," ","d"," "," "," ","d"," "," ")));
            map.add(new ArrayList<String>(Arrays.asList("T"," "," ","d"," "," "," ","T"," "," "," ","t"," "," ","T")));
        }

        //create_nodes();
    }

    public List<ArrayList<Node>> create_nodes(){
        List<ArrayList<Node>> out = new ArrayList<>();
        for (ArrayList<String> line : map){
            out.add(new ArrayList<Node>());
            ArrayList<Node> last_array = out.get(out.size()-1);
            boolean first_line = map.indexOf(line) == 0;
            for (String pom : line){
                Node node = new Node(pom);
                last_array.add(node);
                boolean left_line = line.indexOf(pom) == 0;

                if (pom.equals("|")) node.type = Node.NodeType.TRIPLE_WHOLE;
                else if (pom.equals("=")) node.type = Node.NodeType.DOUBLE_WHOLE;
                else if (pom.equals("T")) node.type = Node.NodeType.TRIPLE;
                else if (pom.equals("D")) node.type = Node.NodeType.DOUBLE;
                else if (pom.equals("S")){
                    node.type = Node.NodeType.START;
                    middle = node;
                }
                else node.type = Node.NodeType.REGULAR;


                if (!first_line && !left_line){
                    node.add_neightbour(out.get(out.indexOf(last_array) - 1).get(last_array.indexOf(node)),last_array.get(last_array.indexOf(node) - 1));
                }
                else if (!first_line){
                    node.add_neightbour(out.get(out.indexOf(last_array) - 1).get(last_array.indexOf(node)),"top");
                }
                else if (!left_line){
                    node.add_neightbour(last_array.get(last_array.indexOf(node) - 1),"left");
                }
            }
        }
        return out;
    }
    public boolean is_empty(){
        return "S".equals(middle.name);
    }
}
