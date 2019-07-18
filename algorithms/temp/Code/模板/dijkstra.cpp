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
    ����˵����
    cost��ԭʼ���ڽӾ��󣬱�Ŵ�0��ʼ
    lowcost�����յ���̾���
    n�ǽڵ���
    source����ʼ��
    */

    /*
    * �㷨���̣�
    * ��ʼ����
    * ע�⣬����ֻ��Դ���lowcost����Ϊ0�����ܳ�ʼ��vis��pre��
    * һ������n�飬ÿ�������ҵ�lowcost��С�Ľڵ�p��֮���޸�vis��
    * �ٴα������нڵ㣬���Ƿ��нڵ��ܹ�ͨ��p�ٵ��ʹ��lowcost��С��
    * �������lowcost��ֵ��ע�⣬�����lowcost��prim��ͬ����cost+mincost�ĺͣ���primֱ��ʹ��cost����
    */
    for(int i=0; i<n; i++)
    {
        lowcost[i]= INF;
        vis[i]=false;
        pre[i]=-1;
    }
    lowcost[source] = 0; //��Ҫ��ע�������prim�㷨�Ĳ�ͬ��prim��MSTʱ��ֻ��Ҫn-1���ߣ���n-1��ѭ����
                         //��ʼ��ʱ�ͽ���һ������lowcostȫ�����
                         //���������ǽ���source�ڵ����lowcost

    for(int i=0; i<n; i++) //���ѭ��n��
    {
        int p=-1; //������¼��ǰlowcost��̵Ľڵ㣬���뵽�ѷ��ʼ�����
        int mincost = INF;
        for(int j=0; j<n; j++)
        {
            if(!vis[j] && lowcost[j]< mincost)
            {
                mincost = lowcost[j];
                p=j;
            }
        }
        if ( p==-1) break; //������нڵ㶼���ʹ������˳�
        vis[p] = true;
        for(int j=0; j<n; j++) //�������нڵ㣬���½ڵ�k�ܵ�������нڵ��lowcost��������Ψһʹ�õ�cost�ĵط�
        {
            if (!vis[j] && lowcost[p]+ cost[p][j] < lowcost[j]) //��prim����һ������
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
    * ��ӡsource��target�����·�������Ľڵ��·�����ȡ�
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
        //�������ر�
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
