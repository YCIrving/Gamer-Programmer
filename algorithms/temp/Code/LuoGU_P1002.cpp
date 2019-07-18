/*
本题需要注意三个点：
1.数据较大时可以用long long int 来求得结果，也可用double，但是要注意不用科学计数法、不输出小数点后面的位数
2.还是坐标与数组下标的转换，很容易出错，一定不要大意，目前认为最好的方法就是在写程序时直接交换位置来遍历数组
3.这是我自己第一次敲动态规划，算法完全是自己想出来的，很有成就感（虽然有人说这不算是动规，但是还是有一定的思想在里面的^_^）
*/

#include<iostream>
//#include <iomanip>
using namespace std;
int main()
{
    int x_b,y_b,x_h,y_h;
    long long map[21][21]={0};
    map[0][0]=1;//地图初始化
    cin>>x_b>>y_b>>x_h>>y_h;
    //设置马的行走范围
    map[y_h][x_h]=-1;//x、y坐标与数组的转换十分重要！！！
    if(x_h-2<=x_b&&y_h+1<=y_b)
        map[y_h+1][x_h-2]=-1;
    if(x_h-2<=x_b&&y_h-1<=y_b)
        map[y_h-1][x_h-2]=-1;
    if(x_h+2<=x_b&&y_h+1<=y_b)
        map[y_h+1][x_h+2]=-1;
    if(x_h+2<=x_b&&y_h-1<=y_b)
        map[y_h-1][x_h+2]=-1;
    if(x_h-1<=x_b&&y_h+2<=y_b)
        map[y_h+2][x_h-1]=-1;
    if(x_h-1<=x_b&&y_h-2<=y_b)
        map[y_h-2][x_h-1]=-1;
    if(x_h+1<=x_b&&y_h+2<=y_b)
        map[y_h+2][x_h+1]=-1;
    if(x_h+1<=x_b&&y_h-2<=y_b)
        map[y_h-2][x_h+1]=-1;
    for(int i=0;i<=y_b;i++)//i为行数、纵坐标,j为列数、横坐标
        for(int j=0;j<=x_b;j++)
    {
        if(map[i][j]!=-1)
        {
            if(i-1>=0&&map[i-1][j]!=-1)
                map[i][j]+=map[i-1][j];
            if(j-1>=0&&map[i][j-1]!=-1)
                map[i][j]+=map[i][j-1];
        }
        //cout<<'('<<j<<','<<i<<')'<<':'<<map[i][j]<<endl;
    }
    cout<<map[y_b][x_b];//std::fixed<<setprecision(0)<<map[x_b][y_b];//<<;
    return 0;
}

