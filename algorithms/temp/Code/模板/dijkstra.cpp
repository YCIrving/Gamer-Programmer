//POJ 2394: Checking an Alibi

#include <iostream>
#include <queue>
#include <vector>
#include <algorithm>
using namespace std;

const int MAXN=1010;
#define typec int
const typec INF = 0x3f3f3f3f;
bool vis[MAXN];
int pre[MAXN];
typec cost[MAXN][MAXN];
typec lowcost[MAXN];

void dijkstra(int source, int n)
{
    /*
    参数说明：
    cost是原始的邻接矩阵，编号从0开始
    lowcost是最终的最短距离
    n是节点数
    source是起始点
    */

    /*
    * 算法流程：
    * 初始化：
    * 注意，这里只将源点的lowcost设置为0，不能初始化vis和pre。
    * 一共遍历n遍，每遍首先找到lowcost最小的节点p，之后修改vis，
    * 再次遍历所有节点，看是否有节点能够通过p再到达，使得lowcost更小。
    * 有则更新lowcost的值。注意，这里的lowcost与prim不同，是cost+mincost的和，而prim直接使用cost更新
    */
    for(int i=0; i<n; i++)
    {
        lowcost[i]= INF;
        vis[i]=false;
        pre[i]=-1;
    }
    lowcost[source] = 0; //重要：注意这里跟prim算法的不同。prim求MST时，只需要n-1条边，即n-1次循环。
                         //初始化时就将其一个结点的lowcost全部算出
                         //而这里我们仅对source节点计算lowcost

    for(int i=0; i<n; i++) //最多循环n次
    {
        int p=-1; //用来记录当前lowcost最短的节点，加入到已访问集合中
        int mincost = INF;
        for(int j=0; j<n; j++)
        {
            if(!vis[j] && lowcost[j]< mincost)
            {
                mincost = lowcost[j];
                p=j;
            }
        }
        if ( p==-1) break; //如果所有节点都访问过，则退出
        vis[p] = true;
        for(int j=0; j<n; j++) //遍历所有节点，更新节点k能到达的所有节点的lowcost。这里是唯一使用到cost的地方
        {
            if (!vis[j] && lowcost[p]+ cost[p][j] < lowcost[j]) //跟prim的又一个区别
            {
                lowcost[j] = lowcost[p] + cost[p][j];
                pre[j] = p;
            }
        }
    }
}

void printPath(int target)
{
    /*
    * 打印source到target的最短路径经过的节点和路径长度。
    */
    int temp =target;
    vector<int> v_node, v_length;
    while(temp!= -1)
    {
        v_node.push_back(temp);
        temp = pre[temp];

    }
    cout<<"Path "<<target<<": ";
    for(int i =v_node.size()-1; i>=1; i--)
    {
        cout<<v_node[i]<<"--->";
    }
    cout<<v_node[0]<<endl;

}

int main()
{
    int f, p, c, m;
    int s,t,l;
    int cows[MAXN];
    priority_queue <int, vector<int>, greater<int> > ans;
    cin >> f>>p>>c>>m;
    for(int i=0; i<f; i++)
    {
        for(int j=0; j<f; j++)
        {
            if(i == j) cost[i][j] = 0;
            else
            {
                cost[i][j] = INF;
                cost[j][i] = INF;
            }
        }
    }
    for(int i=0; i<p; i++)
    {
        cin>>s>>t>>l;
        //可能有重边
        cost[s-1][t-1]=min(l, cost[s-1][t-1]);
        cost[t-1][s-1]=min(l, cost[s-1][t-1]);
    }
    for(int i=0; i<c; i++)
    {
        cin>>cows[i];
    }
    dijkstra(0, f);
    for(int i=0; i<c; i++)
    {
        if(lowcost[cows[i]-1] <= m) ans.push(i+1);
    }
    cout<<ans.size()<<endl;
    while(!ans.empty())
    {
        cout<<ans.top()<<endl;
        ans.pop();
    }
//    printPath(4);
}
