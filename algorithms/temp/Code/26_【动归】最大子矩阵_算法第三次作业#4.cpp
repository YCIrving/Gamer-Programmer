//Onenote 动态规划：最大子矩阵

#include <iostream>
#include <string>
#include <vector>
#include <math.h>
using namespace std;

int mat[110][110]={0};
int f[110][110][110]={0}, s[110]={0};

void calculateS(int n, int i)
{
    int sum=0;
    s[0]=0;
    for(int j=1;j<=n;j++)
    {
        sum+=mat[i][j];
        s[j]=sum;
    }

}

void calculateF(int n, int i)
{
    for(int j=1;j<=n;j++)
    {
        for(int k=j;k<=n;k++)
        {
            f[i][j][k]=max(s[k]-s[j-1],s[k]-s[j-1]+f[i-1][j][k]);
        }
    }
}

int getans(int n)
{
    int ans=f[1][1][1];
    for(int i=1;i<=n;i++)
    {
        for(int j=1;j<=n;j++)
        {
            for(int k=j;k<=n;k++)
            {
                if(ans<f[i][j][k])
                {
                    ans=f[i][j][k];
                }
            }
        }
    }
    return ans;
}
int main()
{
    ios::sync_with_stdio(false);
    int n,ans;
    cin>>n;
    for(int i=1;i<=n;i++)
    {
        for(int j=1;j<=n;j++)
        {
            cin>>mat[i][j];
        }
    }
    for(int i=1;i<=n;i++)
    {
        calculateS(n, i);
        calculateF(n, i);
    }
    cout<< getans(n)<<endl;

}
