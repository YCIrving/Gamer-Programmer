//ƽ�������� �㷨��ҵ�����ε�һ��

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

bool cmpx(Point a, Point b) //��ʼ����
{
    return a.x < b.x;
}

bool cmpy(Point a, Point b) //��temp��������
{
    return a.y < b.y;
}
double closest_pair(int left, int right)
{
    /*
    * ���η�����С����
    * ��һ����Ϊ�˳��ж��͵ݹ��ж�
    * �ڶ�����Ϊ�����������㼯֮�����С����
    * �㷨���̣�
    * ȷ���������䣬�õ����������н�С��d
    * ��ʣ��㣬�����x��p[mid]+-d֮�ڣ������temp���飬ͬʱ�������k����¼����
    * ��temp�еĵ㰴��y�������򣬱���ÿ�����е�ÿ���ԣ�ÿ����ֻ��Ҫ�����������8������бȽ�
    * ������룬���С��d�������d
    * ����d����
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
    * �������mid����x���ϵľ����dС�������temp����
    */
    int k = 0; //��¼����
    for(int i = left; i<= right; i++)
    {
        if(fabs(p[mid].x - p[i].x) <= d) //ע��Ⱥ�
        {
            temp[k++] = p[i];
        }
    }
    sort(temp, temp+k, cmpy); //����
    for(int i=0; i<k; i++) // ��temp�е�ÿ���㣬�����໥֮��ľ��룬�ҵ���Сֵ
    {
//        for(int j = i+1; j< k && temp[j].y - temp[i].y <d; j++)  // ע�⣬����ڶ����ж���������Ҫ�����Ż����ص�
        for(int j =i+1; j<min(k, i+8); j++) // Ҳ���������Ż���ֻ����8���㼴��
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
            scanf("%lf %lf", &p[i].x, &p[i].y); //ע���ȡһ��Ҫ��scanf����cin����
//            cin>>p[i].x>>p[i].y;
        }
        sort(p, p+n, cmpx);
        printf("%.6f\n", closest_pair(0, n-1));
    }
}

