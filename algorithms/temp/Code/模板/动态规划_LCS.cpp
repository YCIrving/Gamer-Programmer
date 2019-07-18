// poj 1458 Common Subsequence

/*
* ����Ҫ��һ�㣬dp[i][j]��ʾ���ǣ���һ���ַ�������Ϊi���ڶ����ַ�������Ϊjʱ��LCS������i��j��ȡֵ��ΧΪ0-l1, 0-l2��
* �ⲻͬ�������±��ȡֵ��Χ���мǡ�
* �㷨��ʼ�������Ƚ�dp�����ʼ��Ϊ0��֮��������������n^2����������״̬ת�Ʒ��̺�s1[i-1]��s2[j-1]�Ƿ���Ƚ���ת��
* ������dp[l1][l2]����
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
            if(s1[i-1]!=s2[j-1]) //ע�������ǱȽ�i-1��j-1����Ϊ��i=1ʱ����ʾ���ǵ�һ���Ӵ�����Ϊ1���������0���ַ���
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

