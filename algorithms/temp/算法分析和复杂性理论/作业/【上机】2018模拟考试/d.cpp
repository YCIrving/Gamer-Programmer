#include <iostream>
#include <algorithm>
#include <memory.h>
#include <limits.h>

#define MAXWEIGHT 10005
#define MAXNUM 505
#define INF 100000000
//cout <<(1<<30) <<endl;
using namespace std;


int opt[MAXWEIGHT], weight[MAXNUM], value[MAXNUM];
int e, f, n, ans;
int main()
{
    int t;
    cin>>t;
    for(int cnt=0; cnt < t; cnt ++)
    {
        memset(opt, 0, sizeof(opt));
        memset(weight, 0, sizeof(weight));
        memset(value, 0, sizeof(value));
        cin>>e>>f;
        e= f-e;
        cin >>n;
        for(int i=1; i<=n; i++)
        {
            cin>>value[i]>>weight[i];
        }
        for (int w=1; w<=e; w++)
        {
            opt[w]=INF;
        }

        for(int i=1; i<=n; i++)
        {
            for(int w=weight[i]; w <=e; w++)
            {
                opt [w] = min(opt[w], opt[w-weight[i]] + value[i]);
            }
        }

        ans = opt[e];
        if(ans == INF)
            cout << "This is impossible."<<endl;
        else
            cout << "The minimum amount of money in the piggy-bank is "<<ans<<"."<<endl;
    }
    return 0;

}
