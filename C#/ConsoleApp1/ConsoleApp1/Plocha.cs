using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class Plocha
    {
        public List<List<string>> map = new List<List<string>>();
        public Node middle;

    public Plocha (int which){
        if (which == 1){
            map.Add(new List<string> {"T"," "," ","t"," "," "," ","T"," "," "," ","d"," "," ","T"});
            map.Add(new List<string> {" "," ","d"," "," "," ","d"," ","d"," "," "," ","t"," "," "});
            map.Add(new List<string> {" ","t"," "," "," ","D"," "," "," "," "," "," "," ","d"," "});
            map.Add(new List<string> {"d"," "," "," ","D"," "," ","d"," "," ","D"," "," "," ","d"});
            map.Add(new List<string> {" "," "," ","D"," "," "," "," "," "," "," ","D"," "," "," "});
            map.Add(new List<string> {" "," "," "," "," ","t"," "," "," ","t"," "," ","D"," "," "});
            map.Add(new List<string> {" ","t"," "," "," "," ","d"," ","d"," "," "," "," ","d"," "});
            map.Add(new List<string> {"T"," "," ","d"," "," "," ","S"," "," "," ","d"," "," ","T"});
            map.Add(new List<string> {" ","d"," "," "," "," ","d"," ","d"," "," "," "," ","d"," "});
            map.Add(new List<string> {" "," ","D"," "," ","t"," "," "," ","t"," "," "," "," "," "});
            map.Add(new List<string> {" "," "," ","D"," "," "," "," "," "," "," ","D"," "," "," "});
            map.Add(new List<string> {"t"," "," "," ","D"," "," ","d"," "," ","D"," "," "," ","d"});
            map.Add(new List<string> {" ","d"," "," "," "," "," "," "," ","D"," "," "," ","t"," "});
            map.Add(new List<string> {" "," ","t"," "," "," ","d"," ","d"," "," "," ","d"," "," "});
            map.Add(new List<string> {"T"," "," ","d"," "," "," ","T"," "," "," ","t"," "," ","T"});
        }

        //create_nodes();
    }

    public List<List<Node>> create_nodes(){
        List<List<Node>> output = new List<List<Node>>();
        foreach (var line in map)
        {
            output.Add(new List<Node>());
            List<Node> lastArray = output[output.Count-1];
            bool firstLine = map.IndexOf(line) == 0;
            foreach (var pom in line)
            {
                Node node = new Node(pom);
                lastArray.Add(node);
                bool leftLine = line.IndexOf(pom) == 0;

                if (pom == "|") node.type = Node.NodeType.TRIPLE_WHOLE;
                else if (pom == "=") node.type = Node.NodeType.DOUBLE_WHOLE;
                else if (pom == "T") node.type = Node.NodeType.TRIPLE;
                else if (pom == "D") node.type = Node.NodeType.DOUBLE;
                else if (pom == "S"){
                    node.type = Node.NodeType.START;
                    middle = node;
                }
                else node.type = Node.NodeType.REGULAR;


                if (!firstLine && !leftLine){
                    node.add_neightbour(output[output.IndexOf(lastArray) - 1][lastArray.IndexOf(node)],lastArray[lastArray.IndexOf(node) - 1]);
                }
                else if (!firstLine){
                    node.add_neightbour(output[output.IndexOf(lastArray) - 1][lastArray.IndexOf(node)],"top");
                }
                else if (!leftLine){
                    node.add_neightbour(lastArray[lastArray.IndexOf(node) - 1],"left");
                }
            }
        }
        return output;
    }
    public bool is_empty(){
        return "S" == middle.name;
    }
    }
}