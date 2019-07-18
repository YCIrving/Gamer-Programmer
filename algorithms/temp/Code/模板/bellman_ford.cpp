//poj 3259 Wormholes

#include <iostream>
#include <vector>
#define INF 100000000
#define MAXN 550
#define MAXM 10000

using namespace std;

int lowcost[MAXN];
int edge_num;

struct Edge
{
    int u, v;
    int w;
}edge[MAXM];



void addedge(int u, int v, int w)
{
    edge[edge_num].u = u;
    edge[edge_num].v = v;
    edge[edge_num++].w = w;
}



void bellman_ford_init()
{
    for(int i=0; i<MAXN; i++) lowcost[i]=INF;
    edge_num = 0;
}

bool bellman_ford(int source, int n)
{
    /*
    * 与kruskal类似，也是对边进行遍历。但仍需要记录距离，所以有lowcost数组。
    * 参数说明：source 起点， n节点数
    * 算法流程：初始化：赋值lowcost[source]
    * 循环n-1次，每次对每条边进行遍历，如果lowcost到边终点的距离大于lowcost到起点的距离加边长，则更新lowcost终点
    * 更新时记录更新符号，作为早停依据
    * 再进行一次循环，如果还有更新，则输出false，说明有环
    * 形式上与floyd很类似，floyd是对每个顶点做一次n重循环的bellman-ford
    */

    lowcost[source] = 0;
    for(int i=0; i<n-1; i++)
    {
        bool flag = false; //距离更新标记
        for(int j=0; j<edge_num; j++)
        {
            int u = edge[j].u;
            int v = edge[j].v;
            int w = edge[j].w;
            if(lowcost[v]>lowcost[u]+w)
            {
                lowcost [v] = lowcost[u] + w;
                flag = true;
            }
        }
        if(!flag) return true; //所有值不更新，早停，并且没有负环
    }
    //最后一次循环，如果还更新，则说明有负环
    for(int j =0; j<edge_num; j++)
    {
        if(lowcost[edge[j].v] > lowcost[edge[j].u] + edge[j].w )
        {
            return false; //有负环
        }
    }
    return true;
}
int main()
{
    int t;
    int n, m, w;
    int u,v,l;
    int temp;
    cin>>t;
    /*
     * 本题我加了超级源点，连到所有节点上，边权为0，这样可以检测图中所有的节点是否在一个环上。
     * 由于本题说明了农夫可以从任意节点出发，所以要检测所有节点。
    */
    while(t--)
    {
        bellman_ford_init();
        cin>>n>>m>>w;
        for(int i =0; i<m; i++)
        {
            cin >> u>>v>>l;
            addedge(u, v, l);
            addedge(v, u, l);
        }
        for(int i=0; i<w; i++)
        {
            cin >> u>>v>>l;
            addedge(u, v, -l);
        }
        for(int i=1; i<=n; i++)
        {
            addedge(0, i, 0);
        }
        if( bellman_ford(0, n+1))
        {
            cout<<"NO"<<endl;
        }
        else
        {
            cout<<"YES"<<endl;
        }
    }
    return 0;
}
