//Onenote 动态规划：最长公共子序列

#include <iostream>
#include <string>
#include <math.h>

//输入包括多组测试数据。每组数据包括一行，给出两个长度不超过200的字符串，表示两个序列。两个字符串之间由若干个空格隔开。
//M矩阵还能优化
//注意M下标和字符串下标的对应关系，主要是0的位置
using namespace std;
string s1,s2;

int M[201][201];

int solve(int i, int j)
{
    if(M[i][j]==-1)
    {
        if(i==0 || j==0)
        {
            M[i][j]=0;
        }
        else
        {
            if(s1[i-1]==s2[j-1])
            {
                M[i][j]=1+(solve(i-1,j-1));
            }
            else
            {
                M[i][j]=max(solve(i,j-1),solve(i-1,j));
            }
        }
    }
    return M[i][j];

}
int main()
{
    int ans=0;
    ios::sync_with_stdio(false);
    while(cin>>s1>>s2)
    {
        for(int i=0;i<=s1.length();i++)
        {
            for(int j=0;j<=s2.length();j++)
            {
                M[i][j]=-1;
            }
        }
        ans=solve(s1.length(),s2.length());
        cout<<ans<<endl;
//        cout<<s1<<endl<<s2<<endl;

    }

}
