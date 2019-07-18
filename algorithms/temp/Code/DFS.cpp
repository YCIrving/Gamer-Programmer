void Graph<EdgeType>::DFSNoReverse()
{
    int i,v,u;
    ArrayStack<int> s;
    for(i=0;i<VerticesNum();i++)
        Mark[i]=UNVISITED;
    for(i=0;i<VerticesNum();i++)
    {
        if(Mark[i]==UNVISITED)
        {
            s.push(i);
            while(!s.isEmpty())
            {
                v=s.pop();
                if(Mark[v]==UNVISITED)
                {
                    visit(v);
                    Mark[v]=VISITED;
                    for(Edge<EdgeType> e=FirstEdge(v);isEdge(e);e=NextEdge(e))
                    {
                        u=EndVertex(e);
                        if(Mark[u]==UNVISITED)
                        s.push(u);
                    }
                }
            }
        }
    }
}
