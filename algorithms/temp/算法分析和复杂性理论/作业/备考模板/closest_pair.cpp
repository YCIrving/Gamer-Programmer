//平面最近点对 算法作业第三次第一题

#include <stdio.h>
#include <algorithm>
#include <iostream>
#include <math.h>

#define MAXN 100010
#define INF 1000000000
using namespace std;

struct Point
{
    double x, y;
};

double cal_dis(Point a, Point b)
{
    return sqrt(pow(a.x-b.x,2)+pow(a.y-b.y,2));
}
Point p[MAXN];
Point temp[MAXN];

bool cmpx(Point a, Point b) //初始排序
{
    return a.x < b.x;
}

bool cmpy(Point a, Point b) //对temp数组排序
{
    return a.y < b.y;
}
double closest_pair(int left, int right)
{
    /*
    * 分治法求最小距离
    * 第一部分为退出判定和递归判定
    * 第二部分为求左右两个点集之间的最小距离
    * 算法流程：
    * 确定左右区间，得到左右区间中较小的d
    * 对剩余点，如果其x在p[mid]+-d之内，则加入temp数组，同时定义变量k，记录点数
    * 对temp中的点按照y进行排序，遍历每个其中的每组点对，每个点只需要跟后面的至多8个点进行比较
    * 计算距离，如果小于d，则更新d
    * 返回d即可
    */
    double d = INF;
    if(left == right) return d;
    if(left +1 == right)
        return cal_dis(p[left], p[right]);
    int mid = (left + right) /2;
    double d1 = closest_pair(left, mid);
    double d2 = closest_pair(mid+1, right);
    d = min(d1, d2);


    /*
    * 如果点与mid点在x轴上的距离比d小，则加入temp数组
    */
    int k = 0; //记录点数
    for(int i = left; i<= right; i++)
    {
        if(fabs(p[mid].x - p[i].x) <= d) //注意等号
        {
            temp[k++] = p[i];
        }
    }
    sort(temp, temp+k, cmpy); //排序
    for(int i=0; i<k; i++) // 对temp中的每个点，计算相互之间的距离，找到最小值
    {
//        for(int j = i+1; j< k && temp[j].y - temp[i].y <d; j++)  // 注意，这里第二个判定条件很重要，是优化的重点
        for(int j =i+1; j<min(k, i+8); j++) // 也可以这样优化，只看后8个点即可
        {
            d = min (d, cal_dis(temp[i], temp[j]));
        }
    }
    return d;
}

int main()
{
//    ios::sync_with_stdio(false);
    int n;
    int t;
    cin>>t;
    while(t--)
    {
        cin>>n;
        for(int i =0; i<n; i++)
        {
            scanf("%lf %lf", &p[i].x, &p[i].y); //注意读取一定要用scanf，与cin差距大
//            cin>>p[i].x>>p[i].y;
        }
        sort(p, p+n, cmpx);
        printf("%.6f\n", closest_pair(0, n-1));
    }
}

