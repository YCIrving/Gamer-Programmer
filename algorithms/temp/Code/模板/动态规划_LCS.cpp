// poj 1458 Common Subsequence

/*
* 最重要的一点，dp[i][j]表示的是，第一个字符串长度为i，第二个字符串长度为j时的LCS，所以i和j的取值范围为0-l1, 0-l2，
* 这不同与数组下标的取值范围，切记。
* 算法初始化：首先将dp数组初始化为0。之后对两个数组进行n^2遍历。根据状态转移方程和s1[i-1]与s2[j-1]是否相等进行转移
* 最后输出dp[l1][l2]即可
*/

#include <iostream>
#include <string>
#include <algorithm>
#include <memory.h>
#define MAXN 500
using namespace std;

string s1, s2;
int dp[MAXN][MAXN];

int solve()
{
    int l1=s1.length(), l2=s2.length();
    memset(dp, 0 , sizeof(dp));
    for(int i=1; i<=l1 ;i++)
    {
        for(int j=1; j<=l2; j++)
        {
            if(s1[i-1]!=s2[j-1]) //注意这里是比较i-1和j-1，因为当i=1时，表示的是第一个子串长度为1，即考察第0个字符。
            {
                dp[i][j] = max(dp[i-1][j], dp[i][j-1]);
            }
            else
            {
                dp[i][j] = dp[i-1][j-1]+1;
            }

        }

    }
    return dp[l1][l2];
}
int main()
{
    while(cin>>s1>>s2)
    {
        cout<<solve()<<endl;
    }

}

