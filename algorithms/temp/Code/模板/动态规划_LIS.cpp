// 百炼 合唱队形

/*
* 正反求两次最长子序列。对每个数，首先初始化dp[i] =1，
* 之后遍历找到前面比它小的数j，比较dp[i]和dp[j]+1的大小，取最大即可。

*一个细节在于正反两个数组下标的对应和最后的ans-1以及n-ans
*/
#include <iostream>
#include <algorithm>

#define MAXN 110

using namespace std;

int height[MAXN];
int heightRev[MAXN];
int dp[MAXN];
int dpRev[MAXN];



int solve(int n)
{
    int ans = 0;

    // init
    for(int i=0; i<n; i++)
    {
        dp[i] = 1;
        dpRev[i] =1; //记忆点1：初始化
        for(int j=i-1; j>=0; j--)
        {
            if(height[i] > height[j])
            {
                dp[i] = max(dp[i], 1 + dp[j]); //记忆点2：max自身和1+dp[j]，而不是max(1, 1+dp[j])
            }
            if(heightRev[i] > heightRev[j])
            {
                dpRev[i] = max(dpRev[i], 1 + dpRev[j]);
            }
        }
    }
    int p=0;
    for(int i=0; i<n; i++)
    {
        if(ans < dp[i] + dpRev[n-i-1])
        {
            ans = dp[i] + dpRev[n-i-1];
            p=i;
        }
    }
//    cout<<endl;
//    for(int i=0; i<n; i++)
//    {
//        cout<<dp[i]<<' ';
//    }
//    cout<<endl;
//    cout<<p<<' '<<ans-1<<endl;
//    cout<<dp[p]<<' '<<dpRev[n-p-1]<<endl;
    return ans-1; //别忘了-1，不然自身会被计算两次

}

int main()
{
    int n;
    cin>>n;
    for(int i=0; i<n; i++)
    {
        cin>>height[i];
        heightRev[n-1-i] = height[i];
    }
//    for(int i=0; i<n; i++)
//    {
//        cout<<height[i]<<' ';
//    }
//    cout<<endl;
//    for(int i=0; i<n; i++)
//    {
//        cout<<heightRev[i]<<' ';
//    }
    int ans = solve(n);
    cout<<n - ans<<endl; //易错点

}
