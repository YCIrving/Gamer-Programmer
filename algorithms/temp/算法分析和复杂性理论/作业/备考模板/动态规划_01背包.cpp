//poj 3624 Charm Bracelet

/*
* 背包问题中最简单的一种，先初始化为0，之后对每件物品进行遍历
* 每件物品中，遍历大于等于该物品的所有重量，但要倒着遍历
* 根据转移方程，修改dp值，dp[j] = max(dp[j], dp[j-goods[i].weight] + goods[i].value)
*/

#include <iostream>
#include <algorithm>
#include <memory.h>

#define MAXM 13000
#define MAXN 3500

using namespace std;

struct good
{
    int value;
    int weight;
}goods[MAXN];
int dp[MAXM];
int main()
{
    int n,m;
    cin>>n>>m;
    for(int i=0; i<n; i++)
    {
        cin>>goods[i].weight>>goods[i].value;
    }
    memset(dp, 0, sizeof(dp)); //记忆点1：初始化

    for(int i=0; i<n; i++) //记忆点2：遍历物品
    {
        int weight_temp = goods[i].weight;
        for(int j=m; j>=weight_temp; j--) //记忆点3：逆序遍历大于该物品重量的每个一重量
        {
            dp[j] = max(dp[j], dp[j-weight_temp] + goods[i].value);
        }
    }
    cout<<dp[m]<<endl;
    return 0;
}
