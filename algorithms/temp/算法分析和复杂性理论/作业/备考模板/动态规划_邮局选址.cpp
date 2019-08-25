// poj 1160 Post Office

#include <iostream>
#include <algorithm>
#include <memory.h>
#define MAXV 310
#define MAXP 310
#define INF 100000000
using namespace std;

int v, p;
int sum[MAXV][MAXP];
int dp[MAXV][MAXP];
int pos[MAXV];
int main()
{
    while(cin>>v>>p) //本题没有说明输入有多行，但其实是多行的，所以只读一次会wa
    {
        for(int i=1; i<=v; i++) //记忆点1：编号从1开始到v结束，不能从0开始，否则计算sum会很复杂
        {
            cin>>pos[i];
        }

        memset(sum, 0, sizeof(sum));

        for(int i =1; i<v; i++) //记忆点2：计算sum数组
        {
            for(int j=i+1; j<=v; j++)
            {
                sum[i][j] = sum[i][j-1] + pos[j] - pos[(i+j)/2]; // 在村庄i到j之间建立一个邮局所需的最少开销
                                                                 // 为i到j-1的开销，加上最后一个点到终点的开销
            }
        }

        for(int i=1; i<=v; i++) //记忆点3：初始化dp数组
        {
            dp[i][i] = 0; // i个村庄建立i个邮局，开销为0
            dp[i][1] =sum[1][i]; // i个村庄建立一个邮局，就等于sum[1][i]，即第一个村庄到第i个村庄的最少开销
                                 // 注意这里不是sum[i][1]， sum下标和dp下标有着本质区别，一定要区分开，sum默认是建立一个邮局
        }

        for(int j=2; j<=p; j++) //记忆点4：计算dp数组，遍历方向与下标相反，先遍历第二维，即j为邮局数
        {
            for(int i=j+1; i<=v; i++) // i为村庄数 因为合法的dp数组要求第一维下标必须大于等于第二维
                                      // 而等于时已经计算过，所以这里只考虑比第二维大的情况
            {
                dp[i][j] = INF;
                for(int k = j-1; k<i; k++) // 假设邮局数一定，遍历所有可能的分位点k，k要将i个村庄分成两份，即1到k和k+1到i
                                           // 而邮局数j也被分成j-1和1，所以1到k至少要包含j-1个村庄，即k至少为j-1
                {
                    dp[i][j] = min(dp[i][j], dp[k][j-1] + sum[k+1][i]);
                }
            }
        }
        cout<<dp[v][p]<<endl;
    }
    return 0;
}
