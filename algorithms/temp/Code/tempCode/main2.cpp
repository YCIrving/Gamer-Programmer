#include <cstdio>
#include <cstring>
int dp[147][147];
int ans[47];
//dp[i][j] = dp[i-1][j]*j + dp[i-1][j-1]*j;
void init()
{
    memset(ans, 0,sizeof(ans));
    //dp[1][0] = 120;
    dp[1][1] = 1;
    for(int i = 2; i <= 10; i++)
    {
        for(int j = 1; j <= i; j++)
        {
            dp[i][j] = dp[i-1][j]*j + dp[i-1][j-1]*j;
            ans[i] += dp[i][j];
        }
    }
}
int main()
{
    int n;
    while(~scanf("%d",&n))
    {
        if(n == -1)
            break;
        init();
        printf("%d\n",ans[n]);
    }
    return 0;
}
