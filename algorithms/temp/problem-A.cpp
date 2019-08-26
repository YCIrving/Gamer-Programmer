#include <bits/stdc++.h>
#include <iostream>
using namespace std;

const int inf = 0x3f3f3f3f;
int n, m;
int dis[1010], vis[1010];
struct node {
    int v, dis;
    node(int a, int b): v(a), dis(b) {};
    bool operator <(const node &a) const {
        return a.dis < dis;
    }
} ;
int mp[1010][1010];

int Prim() {
    int sum = 0;
    int ans = 0;
    priority_queue<node>q;
    q.push(node(1, 0));
    while(!q.empty()) {
        int  tmp =  q.top().v;
        int cost = q.top().dis;
        q.pop();
        if(vis[tmp]) continue;
        sum += cost;
        ans++;
        vis[tmp] = 1;
        for(int i = 1; i <= n; i++) {
            if(!vis[i] && dis[i] > mp[tmp][i]) {
                dis[i] = mp[i][tmp];
                q.push(node(i, dis[i]));
            }
        }
    }
    return sum;
}

int main()
{
    int t;
    cin>>t;
    for(int caseID=1; caseID<=t; caseID++)
    {
        int s, e;
        cin>>n>>m;
        for(int i=1; i<=n; i++)
        {
            for(int j=1; j<=n; j++)
            {
                mp[i][j] = 2;
            }
        }
        memset(dis, inf, sizeof(dis));
        memset(vis,0,sizeof(vis));
        for(int i=0; i<m; i++)
        {
            cin>>s>>e;
            mp[s][e] = 1;
            mp[e][s] = 1;
        }
        cout<<"Case #"<<caseID<<": "<<Prim()<<endl;
    }
    return 0;
}
