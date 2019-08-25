#include <queue>
#include <iostream>
#include <stdlib.h>
#include <algorithm>

#define N 20
using namespace std;

struct point
{
    double x, y;
    friend bool operator <(point a, point b) // 在使用prority_queue时要用到，定义时别忘了bool关键字，只有函数名和cmp不一样，其余都相同
    {
        return a.x < b.x; // 与sort函数的参数cmp正好相反，这里重载的小于号，如果成立，则右边的在前面。故这里是从大到小排列
    }
};

bool cmp(point p1, point p2) //从小到大排列
{
    return p1.x < p2.x;
}

struct cmp2
{
    bool operator () (int x, int y)
    {
        return x > y;
//    return p[x] > p[y]; //如果x和y是下标，则p[x]、p[y]可以表示下标对应元素的优先级
    }
};
point pt[N];
point ptSort[N];
/*
结构体数组，用于使用sort函数进行排序
*/
priority_queue <int, vector<int>, greater<int> > q;
priority_queue <point> q2;
priority_queue <int, vector<int>, cmp2> q3;

/*三种不同的队列：
第一种是普通的小顶堆
第二种是结构体堆，如果这样定义，则结构体中必须重载小于号
第三种为外部定义比较函数的堆，用于给定元素优先级的情况
建议掌握第一种和第二种，因为第三种可以包含在第二种里面
*/

int main()
{
    /*
    随机生成20组坐标，使用结构体数组和结构体优先队列进行排序，输出结果。
    */
    for(int i=0; i< N; i++)
    {
        pt[i].x = rand();
        pt[i].y = rand();
        q2.push(pt[i]);
        cout << i<< ' '<< pt[i].x << ' '<<pt[i].y<<endl;
        ptSort[i].x = pt[i].x;
        ptSort[i].y = pt[i].y;
    }
    cout << "-------------------"<<endl;
    sort(ptSort, ptSort+N, cmp);
    for(int i=0; i<N; i++)
    {
        cout << i<<' '<< ptSort[i].x << ' '<<ptSort[i].y<<endl;
    }

    for(int i=0; i<N; i++)
    {
        cout << i<<' '<< q2.top().x << ' '<<q2.top().y <<endl;
        q2.pop();
    }


}
