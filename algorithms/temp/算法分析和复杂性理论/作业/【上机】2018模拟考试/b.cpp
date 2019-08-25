#include <iostream>
#include <algorithm>
// max函数、dir数组、return +赋值、max+遍历

using namespace std;

#define MAXINT 105

int maze[MAXINT][MAXINT]={0};
int dp[MAXINT][MAXINT] ={0};
int r, c;
int dir[4][2] ={ {-1,0}, {1,0}, {0, -1}, {0, 1} }; //上下左右

bool judge (int r_new, int c_new)
{
    if (c_new>=0 && c_new<c && r_new>=0 && r_new<r)
    {
        return true;
    }
    else
    {
        return false;
    }
}

int dfs(int i, int j)
{
    //cout<<i <<' '<<j<<endl;
    if(dp[i][j]!=0)
    {
        return dp[i][j];
    }
    int max_len=1, tmp_len, i_new, j_new;
    for(int k = 0; k<4; k++)
    {
        i_new = i + dir[k][0];
        j_new = j + dir[k][1];
        if(judge(i_new, j_new) && maze[i][j]> maze[i_new][j_new])
        {
            max_len = max(dfs(i_new, j_new)+1, max_len);
        }
    }
    return dp[i][j] = max_len;
}
int main()
{
    int ans=0;
    cin>>r>>c;
    for(int i=0; i<r; i++)
    {
        for(int j = 0; j<c; j++)
        {
            cin>>maze[i][j];
        }
    }

    for(int i=0; i<r; i++)
    {
        for(int j = 0; j<c; j++)
        {
            ans = max(ans, dfs(i, j));
        }
    }
    cout<<ans<<endl;
}
