// Onenote 数学->平面最近点对

#include <iostream>
#include <vector>
#include <math.h>
#include <stdio.h>
#include <algorithm>

#define MIN3(x,y,z) (x<y?(x<z?x:z):(y<z?y:z))
#define MIN(x,y) (x<y?x:y)

//取消对y排序
//取消动态添加vector
//取消动态temp

using namespace std;
struct point
{
    int x;
    int y;
};

struct point x_order[100000], y_order[100000], temp[100000];
//千万注意temp不能在多次递归中被同时修改，不然结果会出错。
//如果不放心，本题可使用vector<struct point>来实现temp，同样可以AC

bool cmp_x(point p1, point p2)
{
    return p1.x<p2.x;
}

bool cmp_y(point p1, point p2)
{
    return p1.y<p2.y;
}

double calculateDis(point p1, point p2)
{
    return sqrt(pow(p1.x-p2.x,2)+pow(p1.y-p2.y,2));
}
double solve(int n, int s, int e)
{
    if(e - s ==1) // 两个点
    {
        return calculateDis(x_order[s],x_order[e]);
    }
    else if (e - s ==2) //三个点
    {
        double a=calculateDis(x_order[s],x_order[e]);
        double b=calculateDis(x_order[s+1],x_order[e]);
        double c=calculateDis(x_order[s],x_order[s+1]);
        return MIN3(a,b,c);
    }

    int m=(s+e)/2, cnt=0;
    double dis_left=solve(n, s,m);
    double dis_right=solve(n, m+1,e);
    double dis=MIN(dis_left,dis_right);
    double interval_left=x_order[m].x-dis, interval_right=x_order[m].x+dis, dis_temp;
//    for(int i=0;i<n;i++)
//    {
//        if(y_order[i].x>=interval_left && y_order[i].x<=interval_right)
//        {
//            temp[cnt]=y_order[i];
//            cnt++;
//        }
//    }
    for(int i=s;i<=e;i++)
    {
        if(x_order[i].x>=interval_left && x_order[i].x<=interval_right)
        {
            temp[cnt]=x_order[i];
            cnt++;
        }
    }
    sort(temp,temp+cnt,cmp_y);

    //计算最小点
    for(int i=0;i<cnt-1;i++)
    {
        for(int j=i+1;j<MIN(cnt,i+8);j++)
        {
            dis_temp=calculateDis(temp[i],temp[j]);
            if(dis_temp<dis)
            {
                dis=dis_temp;
            }
        }
    }
    return dis;

}
int main()
{
    ios::sync_with_stdio(false);
    int t;
    scanf("%d",&t);
//    t=1;
    for(int i=0;i<t;i++)
    {
        int n,x,y;
        scanf("%d",&n);
        for(int j=0;j<n;j++)
        {

            scanf("%d%d",&x_order[j].x,&x_order[j].y);
            y_order[j]=x_order[j];
//            x_order.push_back(make_pair(x,y));
//            y_order.push_back(make_pair(x,y));
        }
        sort(x_order,x_order+n,cmp_x);
//        sort(y_order,y_order+n,cmp_y);

//        for(int i=0;i<n;i++)
//        {
//            cout<<"("<<x_order[i].x<<", "<<x_order[i].y<<")"<<endl;
//        }
//        cout<<endl;
//        for(int i=0;i<n;i++)
//        {
//            cout<<"("<<y_order[i].x<<", "<<y_order[i].y<<")"<<endl;
//        }
        double ans=solve(n, 0, n-1);
        printf("%.6f\n",ans);
    }
    return 0;
}
