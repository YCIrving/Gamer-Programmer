//OneNote 模拟->螺旋输出
#include <iostream>
using namespace std;

#define MIN(x,y) (x<y?x:y)
#define MAXNUM 500
int main()
{
    int n, m;
    // n为行数，m为列数
    int maze[MAXNUM][MAXNUM]={0};
    //int fitness[MAXNUM][MAXNUM]={0};
    cin>>n>>m;
    int num=1;
    for(int i=1;i<=n;i++)
    {
        for(int j=1;j<=m;j++)
        {
            maze[i][j]=num;
            cout<< num<<' ';
            num++;
        }
        cout<<endl;
    }
    int r=1,c=1,tag=1;
    //tag 1向右，2向下，3向左，4向上
    for(int k=0;k<MIN(n,m);k++)
    {
        int cnt=0;
        if (tag==1)
        //第一次遍历
        {
            while(cnt<m)
            {
                cout<<maze[r][c+cnt]<<' ';
                cnt++;
            }
            c=c+cnt-1;
            tag=2;
            continue;
        }
        else if (tag==2)
        //向下和左遍历
        {
            cnt=1;
            while(cnt<=n-k)
            {
                cout<<maze[r+cnt][c]<<' ';
                cnt++;
            }
            r=r+cnt-1;
            tag=3;

            cnt=1;
            while(cnt<=m-k)
            {
                cout<<maze[r][c-cnt]<<' ';
                cnt++;
            }
            c=c-cnt+1;
            tag=4;
        }
        else
        //向上和右遍历
        {
            cnt=1;
            while(cnt<=n-k)
            {
                cout<<maze[r-cnt][c]<<' ';
                cnt++;
            }
            r=r-cnt+1;
            tag=1;

            cnt=1;
            while(cnt<=m-k)
            {
                cout<<maze[r][c+cnt]<<' ';
                cnt++;
            }
            c=c+cnt-1;
            tag=2;
        }
    }
    //对于方阵的遍历，输入只有n
//    for(int k=n;k>=1;k--)
//    {
//        int cnt=0;
//        if (tag==1)
//        {
//            while(cnt<k)
//            {
//                cout<<maze[r][c+cnt]<<' ';
//                cnt++;
//            }
//            c=c+cnt-1;
//            tag=2;
//            continue;
//        }
//        else if (tag==2)
//        {
//            cnt=1;
//            while(cnt<=k)
//            {
//                cout<<maze[r+cnt][c]<<' ';
//                cnt++;
//            }
//            r=r+cnt-1;
//            tag=3;
//
//            cnt=1;
//            while(cnt<=k)
//            {
//                cout<<maze[r][c-cnt]<<' ';
//                cnt++;
//            }
//            c=c-cnt+1;
//            tag=4;
//        }
//        else
//        {
//            cnt=1;
//            while(cnt<=k)
//            {
//                cout<<maze[r-cnt][c]<<' ';
//                cnt++;
//            }
//            r=r-cnt+1;
//            tag=1;
//
//            cnt=1;
//            while(cnt<=k)
//            {
//                cout<<maze[r][c+cnt]<<' ';
//                cnt++;
//            }
//            c=c+cnt-1;
//            tag=2;
//        }
//    }
    return 0;
}
