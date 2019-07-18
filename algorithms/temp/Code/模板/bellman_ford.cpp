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
    * ��kruskal���ƣ�Ҳ�ǶԱ߽��б�����������Ҫ��¼���룬������lowcost���顣
    * ����˵����source ��㣬 n�ڵ���
    * �㷨���̣���ʼ������ֵlowcost[source]
    * ѭ��n-1�Σ�ÿ�ζ�ÿ���߽��б��������lowcost�����յ�ľ������lowcost�����ľ���ӱ߳��������lowcost�յ�
    * ����ʱ��¼���·��ţ���Ϊ��ͣ����
    * �ٽ���һ��ѭ����������и��£������false��˵���л�
    * ��ʽ����floyd�����ƣ�floyd�Ƕ�ÿ��������һ��n��ѭ����bellman-ford
    */

    lowcost[source] = 0;
    for(int i=0; i<n-1; i++)
    {
        bool flag = false; //������±��
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
        if(!flag) return true; //����ֵ�����£���ͣ������û�и���
    }
    //���һ��ѭ������������£���˵���и���
    for(int j =0; j<edge_num; j++)
    {
        if(lowcost[edge[j].v] > lowcost[edge[j].u] + edge[j].w )
        {
            return false; //�и���
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
     * �����Ҽ��˳���Դ�㣬�������нڵ��ϣ���ȨΪ0���������Լ��ͼ�����еĽڵ��Ƿ���һ�����ϡ�
     * ���ڱ���˵����ũ����Դ�����ڵ����������Ҫ������нڵ㡣
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
