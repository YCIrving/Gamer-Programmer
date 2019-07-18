//poj 2349 Arctic Network
// �Զ��� https://blog.csdn.net/mengxiang000000/article/details/51482790

#include <iostream>
#include <memory.h>
#include <math.h>
#include <algorithm>
#include <stdio.h>
#define INF 100000000
#define MAXN 510

using namespace std;

bool vis[MAXN];
double cost[MAXN][MAXN];
double lowcost[MAXN];

struct Point
{
    double x, y;
}points[MAXN];

double mst_edge[MAXN];

bool cmp(double f1, double f2)
{
    return f1>f2;
}
void prim_init()
{
    memset(vis, false, sizeof(vis));
    for(int i=0; i<MAXN; i++)
    {
        for(int j=0; j<MAXN; j++)
        {
            if(i == j)
            {
                cost[i][j] = 0;
            }
            else
            {
                cost[i][j] = INF;
            }
        }
    }
}

double cal_dis(Point p1, Point p2)
{
    return sqrt(pow(p1.x-p2.x, 2) + pow(p1.y-p2.y, 2));
}

double prim(int n) //��С���������ⲻ��Ҫ���
{
    /*
    * �Խڵ���б������ʺϳ���ͼ
    * ����˵����nԭͼ�нڵ�������ڵ��Ŵ�0��n-1
    * ������С��������Ȩֵ��-1��ʾ����ͨ
    * �㷨���̣�
    * ������ڵ㿪ʼ������Ĭ��Ϊ0��
    * ����ýڵ��lowcostֵ��Ϊֱ���ߵ�Ȩֵ���޸�vis
    * ����Щֵ�У�ѡ��һ����ֵ��С���յ㣬��Ϊ�ڵ�p���޸�vis,ͬʱ�������߼���mst�У��޸�ans
    * ����p�ĳ��ߣ����Ƿ��ܸ���lowcost
    * ֮���ټ�����lowcost��ѡ����С�ģ�����ѭ��
    */
    double ans = 0;

    vis[0] = true;
    for(int i=1; i<n; i++) lowcost[i] = cost [0][i];

    for(int i=1; i<n; i++)
    {
        double mincost = INF;
        int p = -1;
        for(int j =0; j<n; j++)
        {
            if(!vis[j] && mincost > lowcost[j])
            {
                mincost = lowcost[j];
                p =j;
            }
        }
        if(p == -1) return -1; //ԭͼ����ͨ
        ans +=mincost;
        mst_edge[i] = mincost;
        vis[p] = true;
        for(int j=0; j<n; j++)
        {
            if(!vis[j] && lowcost[j] > cost[p][j]) //ע��˴���dijkstra������û�м�lowcost[p]
            {
                lowcost[j] = cost[p][j];
            }
        }
    }
    return ans;
}

int main()
{
    int t, s, n;
    cin>>t;
    while(t--)
    {
        cin >>s>>n;
        for(int i=0; i<n; i++)
        {
            cin>>points[i].x>>points[i].y;
        }
        prim_init();
        for(int i=0; i<n; i++)
        {
            for(int j=0; j<n; j++)
            {
                if(i!=j)
                {
                    cost[i][j] = cal_dis(points[i], points[j]);
                    cost[j][i] = cost[i][j];
                }
            }
        }
        prim(n);
        sort(mst_edge+1, mst_edge+n);
        printf("%.2f\n", mst_edge[n-s]);
    }
    return 0;
}
