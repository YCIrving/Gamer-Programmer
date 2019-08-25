#include <iostream>
#include <algorithm>
#include <vector>
#include <queue>
#include <memory.h>

#define MAXN 10000
#define MAXM 100000000
#define INF 1000000000
int dis [MAXN][MAXN];
bool vis_s[MAXN];
bool vis_t[MAXN];
int lowcost_s[MAXN];
int lowcost_t[MAXN];
using namespace std;

void dijskral_s(int s, int n)
{
    for(int i=0; i<n; i++)
    {
        lowcost_s[i] =INF;
        vis_s[i]= false;
    }
    lowcost_s[s] = 0;
    for(int i =0; i<n; i++)
    {
        int mincost =INF;
        int p =-1;
        for(int j=0; j<n; j++)
        {
            if(!vis_s[j] && lowcost_s[j]<mincost)
            {
                mincost = lowcost_s[j];
                p =j;
            }
        }
        if(p == -1) return;
        vis_s[p] =true;
        for(int j=0; j<n; j++)
        {
            if(!vis_s[j] && lowcost_s[j]> lowcost_s[p]+ dis[p][j])
            {
                lowcost_s[j] = lowcost_s[p] + dis[p][j];
            }
        }
    }
    return;
}

void dijskral_t(int t, int n)
{
    for(int i=0; i<n; i++)
    {
        lowcost_t[i] =INF;
        vis_t[i]= false;
    }
    lowcost_t[t] = 0;
    for(int i =0; i<n; i++)
    {
        int mincost =INF;
        int p =-1;
        for(int j=0; j<n; j++)
        {
            if(!vis_t[j] && lowcost_t[j]<mincost)
            {
                mincost = lowcost_t[j];
                p =j;
            }
        }
        if(p == -1) return;
        vis_t[p] =true;
        for(int j=0; j<n; j++)
        {
            if(!vis_t[j] && lowcost_t[j]> lowcost_t[p]+ dis[j][p])
            {
                lowcost_t[j] = lowcost_t[p] + dis[j][p];
            }
        }
    }
    return;
}
int main()
{
    int t;
    int n,m;
    int u, v, w;
    cin>>t;
    while(t--)
    {
        memset(vis_s,0,sizeof(vis_s));
        memset(vis_t,0,sizeof(vis_t));
        cin>>n>>m;
        for(int i =0; i<=n; i++)
        {
            for(int j=0; j<=n; j++)
            {
                if(i==j) dis[i][j] =0;
                else dis[i][j] =INF;
            }
        }

        for(int i=0; i<n; i++)
        {
            dis[i][i]=0;
        }
        for(int i=0; i<m; i++)
        {
            cin>>u>>v>>w;
            if(v==1)
            {
                dis[u][0]=w;
            }
            else
            {
                dis[u][v] = w;
            }
        }
        dijskral_s(1, n+1);
        dijskral_t(0, n+1);
        int sum=0;
        for(int i=2; i<n+1; i++)
        {
            sum+=lowcost_s[i]+lowcost_t[i];
        }
        cout<<sum<<endl;
    }
    return 0;
}
