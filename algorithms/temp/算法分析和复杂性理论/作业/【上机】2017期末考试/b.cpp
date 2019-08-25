#include <iostream>
#include <memory.h>
#include <queue>

#define MAX 1000000
#define INF 1000000000
using namespace std;


// memset¡¢memory.h¡¢queue
// 13 24 ÌØÅÐ

int dis[MAX];
int n, k;

int init()
{
    memset (dis, -1, sizeof(dis));
    dis[n]=0;
//    for(int i = n; i>=0; i--)
//    {
//        dis[i] = n-i;
//    }
}
int bfs()
{
    if (n >= k)
    {
        return n-k;
    }
    queue<int> q;
    q.push(n);
    int id;
    int t1, t2, t3;
    while(!q.empty())
    {
        id = q.front();
        q.pop();
        t1 = id -1;
        t2 = id +1;
        t3 = id *2;
        cout<<id<<endl;
        if(t1 == k || t2 ==k || t3==k)
            return dis[id]+1;
        if(t1<=2*k-n && dis[t1]==-1)
        {
            dis[t1] = dis[id]+1;
            q.push(t1);
        }
        if(t2<=2*k-n && dis[t2]==-1)
        {
            dis[t2] = dis[id]+1;
            q.push(t2);
        }
        if(t3<=2*k-n && dis[t3]==-1)
        {
            dis[t3] = dis[id]+1;
            q.push(t3);
        }
    }

}
int main()
{
    cin>>n>>k;
    init();
    cout<<bfs()<<endl;
//    for(int i=0; i<=k; i++)
//    {
//        cout<<dis[i]<<' ';
//    }
    return 0;
}
