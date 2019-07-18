//Onenote 动态规划：棋盘分割

#include <iostream>
#include <math.h>
#include <stdio.h>

#define INF 1e9
using namespace std;

int board[10][10]={0},sum[10][10]={0};
double f[16][10][10][10][10]={0};
// 使用math时，接收数要符合函数输出的类型；
int main()
{
    int n;
    double avg=0,ans;
    cin>>n;
    for(int i=1;i<=8;i++)
    {
        for(int j=1;j<=8;j++)
        {
            cin>>board[i][j];
            avg+=board[i][j];
            sum[i][j]=board[i][j]+sum[i-1][j]+sum[i][j-1]-sum[i-1][j-1];
        }
    }
    avg/=n;
    for(int i=0;i<=7;i++)
    {
        for(int j=0;j<=7;j++)
        {
            for(int k=i+1;k<=8;k++)
            {
                for(int l=j+1;l<=8;l++)
                {
                    f[0][i][j][k][l]=pow(sum[k][l]+sum[i][j]-sum[i][l]-sum[k][j],2);
                }
            }
        }
    }
    for(int m=1; m<n; m++)
    {
        for(int i=0;i<=7;i++)
        {
            for(int j=0;j<=7;j++)
            {
                for(int k=i+1;k<=8;k++)
                {
                    for(int l=j+1;l<=8;l++)
                    {
                        //任意给一种状态
                        f[m][i][j][k][l]=f[0][i][j][i+1][l]+f[m-1][i+1][j][k][l];
                        //四种方法拼成f
                        //竖切
                        for(int x=i+1;x<k;x++)
                        {
                            f[m][i][j][k][l]=min(f[m][i][j][k][l],min(f[0][i][j][x][l]+f[m-1][x][j][k][l],f[m-1][i][j][x][l]+f[0][x][j][k][l]));
                        }
                        //横切
                        for(int y=j+1;y<l;y++)
                        {
                            f[m][i][j][k][l]=min(f[m][i][j][k][l],min(f[0][i][j][k][y]+f[m-1][i][y][k][l],f[m-1][i][j][k][y]+f[0][i][y][k][l]));
                        }
                    }
                }
            }
        }
    }
    ans=sqrt((f[n-1][0][0][8][8]/n)-avg*avg);
    printf("%.3f\n",ans);
//    printf("%.0f\n",f[n-1][0][0][8][8]);
    return 0;
}
