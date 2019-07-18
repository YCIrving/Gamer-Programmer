// poj 1088 滑雪
// 记忆化搜索避免了每次都重新迭代计算，使用vis数组表示当前dp值是否是最新的。
/*
* 算法流程：
* 初始化vis和dp数组，dp数组全为1，vis全为false
* DFS中，对于一个点，如果其已经被访问过，则直接返回dp值
* 如果没有被访问，则需要访问其四周的大于它的节点，得到其dp值，然后取最大的一个加上去
*/



#include <iostream>
#include <algorithm>
#include <memory.h>

#define MAXN 110

using namespace std;

int maze[MAXN][MAXN];
int dp[MAXN][MAXN];
int vis[MAXN][MAXN];
int go[][2] = {0,1, 0,-1, 1,0, -1,0};
int r, c;

int DFS(int pos_r, int pos_c) //记忆点1：DFS算法需要有返回值
{
    if(vis[pos_r][pos_c]) return dp[pos_r][pos_c];
    int new_r, new_c;
    for(int i=0; i<4; i++)
    {
        new_r = pos_r + go[i][0];
        new_c = pos_c + go[i][1];
        if(new_r>=0 && new_r <r && new_c >=0 && new_c <c && maze[new_r][new_c] > maze[pos_r][pos_c] )
        {
            dp[pos_r][pos_c] = max(dp[pos_r][pos_c], DFS(new_r, new_c) +1); //记忆点2：这里是DFS(new_r, new_c) 而不是dp[new_r][new_c]
        }
    }
    vis[pos_r][pos_c] = true; //记忆点3：修改vis值
    return dp[pos_r][pos_c]; //记忆点4：返回最终的dp值
}

int main()
{
    cin>>r>>c;
    for(int i=0; i<r; i++)
    {
        for(int j=0; j<c; j++)
        {
            cin>>maze[i][j];
            dp[i][j] =1; //memset 只能赋值0和-1，故这里使用遍历来赋值
        }
    }
    memset(vis, 0, sizeof(vis));
    int ans =0;
    for(int i=0; i<r; i++)
    {
        for(int j=0; j<c; j++)
        {
            ans = max(ans, DFS(i, j)); //必须要对每个点进行一次DFS，这样才能保证整个图的dp数组全部被计算出来
                                       //可以想象一下，如果第一个点搜索的就是全局最小点，则它只能得到自己的dp值
        }
    }
    cout<<ans<<endl;
}
