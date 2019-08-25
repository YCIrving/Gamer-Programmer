//POJ 1679 The Unique MST

#include <iostream>
#include <vector>
#include <memory.h>
#include <algorithm>

#define MAXN 110 //点数
#define MAXM 20000 //边数

using namespace std;
int F[MAXN]; //并查集
vector <int> edge_id;

struct Edge
{
    int u, v, w;
}edge[MAXM];

int num_edge;
void addedge(int u, int v, int w)
{
    edge[num_edge].u = u;
    edge[num_edge].v = v;
    edge[num_edge++].w = w;
}

bool cmp (Edge a, Edge b)
{
    return a.w < b.w;
}

int find(int x)
{
    if(F[x] == -1)
        return x;
    else
        return F[x] = find(F[x]);
}
void kruskal_init()
{
    memset(F, -1, sizeof(F));
    edge_id.clear();
    num_edge = 0;
}
int kruskal (int n)
{
    /*
    * 对边进行遍历，适合稀疏图
    * 参数说明：n是节点数
    * 返回值：ans是最小生成树的权值。-1表示不连通
    * edge_id是最小生成树中边的id，注意该id是排序后的。
    * 算法流程：
    * 对每条边从小到大排序；
    * 得到每条边的起止点，根据并查集判断是否在一个集合中，
    * 不是的话将其并查集节点合并，之后修改ans和cnt，最后将边的id加入到vector中
    * 判断cnt是否为n-1，是则提前退出
    *
    */
    sort(edge, edge+num_edge, cmp);
//    for(int i =0; i<num_edge; i++)
//    {
//        cout<<edge[i].u << "-->"<<edge[i].v<<": "<<edge[i].w<<endl;
//    }
    int cnt =0;
    int ans =0;
    for(int i=0; i<num_edge; i++)
    {
        int u =edge[i].u;
        int v =edge[i].v;
        int w =edge[i].w;
        int t1 = find(u);
        int t2 = find(v);
        if(t1!=t2)
        {
            edge_id.push_back(i); //记录边的序号
            ans +=w;
            F[t1] = t2;
            cnt ++;
        }
        if(cnt == n-1) break;
    }
    if(cnt < n-1) return -1; //不连通
    else return ans;
}

int kruskal_b (int n, int id) //不属于模板，仅用于本题检验
{
    memset(F, -1, sizeof(F));
    int cnt =0;
    int ans =0;
    for(int i=0; i<num_edge; i++)
    {
        if( i == id) continue;
        int u =edge[i].u;
        int v =edge[i].v;
        int w =edge[i].w;
        int t1 = find(u);
        int t2 = find(v);
        if(t1!=t2)
        {
            ans +=w;
            F[t1] = t2;
            cnt ++;
        }
        if(cnt == n-1) break;
    }
    if(cnt < n-1) return -1; //不连通
    else return ans;
}

int main()
{
    int t;
    int n,m;
    int u, v, w;
    int ans, ans_b, id; //特判：对于只有一个结点的图，输出也是零，无需特判
    cin>>t;
    while(t--)
    {
        cin>>n>>m;
        bool unique_tag = true; //放在函数中
        kruskal_init();
        for(int i = 0; i<m; i++)
        {
            cin >>u >> v >> w;
            addedge(u, v, w);
        }
        ans = kruskal(n); //默认一定存在最小生成树
        for(int i = 0; i<edge_id.size(); i++)
        {
            id = edge_id[i];
            ans_b = kruskal_b(n, id);
//            cout << id <<' '<<ans_b<<endl;
            if(ans_b == ans)
            {
                unique_tag = false;
                break;
            }
        }
        if(unique_tag == true) cout<<ans<<endl;
        else cout<<"Not Unique!"<<endl;
    }
}
