#include <iostream>
#include <memory.h>
#include <stdio.h>
#include <math.h>
#include <vector>
#include <algorithm>

#define MAX 110
#define INF 100000000
using namespace std;

double cost[MAX][MAX];
double lowcost[MAX];
bool vis[MAX];

struct node
{
    double x, y , z, r;
}nodes[MAX];

struct edge
{
    int s, t;
    double dis;
};

bool cmp (edge e1, edge e2)
{
    return e1.dis < e2.dis;
}

vector <edge> edges;

//double dis[MAX][MAX];

double cal_dis_two(int i, int j)
{
    double dis = sqrt(pow(nodes[i].x - nodes[j].x, 2) + pow(nodes[i].y - nodes[j].y, 2) + pow(nodes[i].z - nodes[j].z, 2));
    if(dis <= (nodes[i].r+nodes[j].r) )
    {
        return 0.0;
    }
    else
    {
        return dis - nodes[i].r - nodes[j].r;
    }
}
void cal_dis(int n)
{
    for(int i=0; i<n; i++)
    {

        for(int j=0; j<n; j++)
        {

            if(i==j)
                cost[i][j] =0;
            else
                cost[i][j] =INF;
        }
    }
    edge e;
    for(int i=0; i<n-1; i++)
    {
        for(int j=i+1; j<n; j++)
        {
//            dis[i][j]= cal_dis_two(i, j);
            e.s = i;
            e.t = j;
            e.dis = cal_dis_two(i, j);
            edges.push_back(e);
            cost[i][j] = cost[j][i] =e.dis;
        }
    }
}

double prim(int n)
{
    for(int i=0; i<n; i++)
    {
        vis[i] = false;
        lowcost[i] = cost[0][i];
    }
    vis[0] = true;

    int cnt = 0;
    double ans =0;
    for(int i=0; i<n-1; i++)
    {
        int p=-1;
        double mincost = INF;
        for(int j=0; j<n; j++)
        {
            if(!vis[j] && lowcost[j] < mincost)
            {
                mincost = lowcost[j];
                p =j;
            }
        }
        if(p==-1) return -1;
        ans +=mincost;
        vis[p] = true;
        for(int j=1; j<n; j++)
        {
            if(!vis[j] && lowcost[j]> cost[p][j])
            {
                lowcost[j] = cost[p][j];
            }
        }
    }
    return ans;
}

int main()
{
    int n;
    double x, y, z, r;
    while(cin>>n && n)
    {
        edges.clear();
        for(int i=0; i<n ; i++)
        {
            cin>>nodes[i].x>>nodes[i].y>>nodes[i].z>>nodes[i].r;
        }
        cal_dis(n);
        double ans = prim(n);
        printf("%.3f\n", ans);
    }
}

