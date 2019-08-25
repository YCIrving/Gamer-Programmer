#include <iostream>
#define MAXN 110
#define INF 100000000
using namespace std;

int cost[MAXN][MAXN];
int lowcost[MAXN];
bool vis[MAXN];

int prim(int n)
{
    for(int i=0; i<n; i++)
    {
        lowcost[i] = INF;
        vis[i] =false;
    }
    lowcost[0] =0;
    int ans =0;
    int p;
    int mincost;
    for(int i=0; i<n; i++)
    {
        mincost = INF;
        p=-1;
        for(int j=0; j<n; j++)
        {
            if(!vis[j] && mincost > lowcost[j])
            {
                p = j;
                mincost = lowcost[j];
            }
        }
        if( p== -1) return -1;
        ans += mincost;
        vis[p] = true;
        for(int j=0; j<n; j++)
        {
            if(!vis[j] && lowcost[j] > cost[p][j])
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
    while(cin>>n)
    {
        for(int i=0; i<n; i++)
        {
            for(int j=0; j<n; j++)
            {
                cin>>cost[i][j];
            }
        }

//        for(int i=0; i<n; i++)
//        {
//            for(int j=0; j<n; j++)
//            {
//                cout<<cost[i][j]<<' ';
//            }
//            cout<<endl;
//        }
        cout<<prim(n)<<endl;
    }


}
