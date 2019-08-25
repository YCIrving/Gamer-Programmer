#include <queue>
#include <iostream>
#include <stdlib.h>
#include <algorithm>

#define N 20
using namespace std;

struct point
{
    double x, y;
    friend bool operator <(point a, point b) // ��ʹ��prority_queueʱҪ�õ�������ʱ������bool�ؼ��֣�ֻ�к�������cmp��һ�������඼��ͬ
    {
        return a.x < b.x; // ��sort�����Ĳ���cmp�����෴���������ص�С�ںţ�������������ұߵ���ǰ�档�������ǴӴ�С����
    }
};

bool cmp(point p1, point p2) //��С��������
{
    return p1.x < p2.x;
}

struct cmp2
{
    bool operator () (int x, int y)
    {
        return x > y;
//    return p[x] > p[y]; //���x��y���±꣬��p[x]��p[y]���Ա�ʾ�±��ӦԪ�ص����ȼ�
    }
};
point pt[N];
point ptSort[N];
/*
�ṹ�����飬����ʹ��sort������������
*/
priority_queue <int, vector<int>, greater<int> > q;
priority_queue <point> q2;
priority_queue <int, vector<int>, cmp2> q3;

/*���ֲ�ͬ�Ķ��У�
��һ������ͨ��С����
�ڶ����ǽṹ��ѣ�����������壬��ṹ���б�������С�ں�
������Ϊ�ⲿ����ȽϺ����Ķѣ����ڸ���Ԫ�����ȼ������
�������յ�һ�ֺ͵ڶ��֣���Ϊ�����ֿ��԰����ڵڶ�������
*/

int main()
{
    /*
    �������20�����꣬ʹ�ýṹ������ͽṹ�����ȶ��н���������������
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
