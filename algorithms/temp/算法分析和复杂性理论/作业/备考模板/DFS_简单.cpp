// poj 1562 Oil Deposits

#include <iostream>
#include <memory.h>
#include <stdio.h>
#define MAXN 150
using namespace std;

char maze[MAXN][MAXN];
bool vis[MAXN][MAXN];

int go [8][2] = {1,0,-1,0,0,1,0,-1,1,1,-1,-1,-1,1,1,-1};

void DFS(int r, int c, int m, int n)
{
    int new_r, new_c;
    for(int i=0; i<8; i++)
    {
        new_r = r+go[i][0];
        new_c = c+go[i][1];
        if(new_r >=1 && new_r <=m && new_c >=1 && new_c <=n && vis[new_r][new_c] == false && maze[new_r][new_c] == '@')
        {
            vis[new_r][new_c] =true;
            DFS(new_r, new_c, m, n);
        }

    }
}

int main()
{
    int m, n;
    while(cin >>m >>n)
    {
        if(m==0 && n==0) return 0;
        for(int i=1; i<=m; i++)
        {
            scanf("%s", maze[i]+1);
        }
        memset(vis, false, sizeof(vis));
        int ans = 0;
        for(int i=1; i<=m; i++)
        {
            for(int j=1; j<=n; j++)
            {
                if(vis[i][j]==false && maze[i][j] =='@')
                {
                    ans ++;
                    vis[i][j] =true;
                    DFS(i, j, m, n);
                    // 这三个操作一定是绑定的，类比BFS
                }
            }
        }
        cout<<ans<<endl;
    }



}

