// hdu 1232 畅通工程

#include <iostream>
#include <stdio.h>

#define MAXN 1100

using namespace std;

int root[MAXN];

void init(int n)
{
    for(int i = 1; i<=n; i++)
    {
        root[i] = i;
    }
}

int unionset_find(int x)
{
    int ans=x, temp=x;
    while(root[ans]!= ans)
    {
        ans = root[ans];
    }
    while(root[x] != ans)
    {
        temp = root[x];
        root[x] = ans;
        x =temp;
    }
    return ans;
}
void unionset_union(int x, int y)
{
    int root_x=unionset_find(x), root_y=unionset_find(y);
    if(root_x!=root_y)
    {
        root[root_y] =root_x; //这里对树根操作，而不是对x和y操作
    }
}
int main()
{
    int n, m;
    int ans, cnt;
    int u, v;
    while(1)
    {
        scanf("%d", &n);
        if(n ==0) break;
        scanf("%d", &m);
        ans = 0;
        init(n);
        for(int i=0; i<m; i++)
        {
            scanf("%d %d", &u, &v);
            unionset_union(u, v);
        }
        for(int i=1; i<=n; i++)
        {
            if(root[i]==i) ans++;
        }
        cout<<ans-1<<endl;
    }
    return 0;

}
