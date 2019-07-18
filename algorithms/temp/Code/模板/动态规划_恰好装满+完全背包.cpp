// poj1384 恰好装满+完全背包+最小

/*
* 完全背包与01背包问题只有一处不同，即遍历顺序。因此如果遇到完全背包，换一个遍历顺序即可
* 恰好装满问题与01背包或完全背包并不冲突，恰好装满要求在初始化dp数组时，只将dp[0]初始化为0
* 其余根据求最大还是最小设置为负无穷或正无穷
* 如本题是求最小，所以就设置为正无穷，表示不存在，一旦有更小的值，就能更新。
* 反之，求最大，就设置为负无穷
*/

#include <iostream>
#include <algorithm>

#define INF 1000000
#define MAXN 600
#define MAXM 11000

using namespace std;

struct good
{
    int value;
    int weight;
}goods[MAXN];

int dp[MAXM];

int main()
{
    int t;
    int n,m;
    cin>>t;
    while(t--)
    {
        cin>>n>>m;
        m=m-n;
        cin>>n;
        for(int i=0; i<n; i++)
        {
            cin>>goods[i].value>>goods[i].weight;
        }
        for(int i=1; i<=m; i++)
        {
            dp[i] =INF; //记忆点1：初始化
        }
        dp[0] = 0;

        for(int i=0; i<n; i++)
        {
            int weight_temp = goods[i].weight;
            for(int j=weight_temp; j<=m; j++) //记忆点2：正序遍历
            {
                dp[j] = min(dp[j], dp[j-weight_temp] + goods[i].value);
            }
        }
        if(dp[m]==INF) cout<<"This is impossible."<<endl;
        else cout<<"The minimum amount of money in the piggy-bank is "<<dp[m]<<'.'<<endl;
    }
}
