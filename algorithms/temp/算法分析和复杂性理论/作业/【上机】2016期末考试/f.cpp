#include <iostream>
#include <vector>
#include <math.h>
#include <algorithm>
#include <string>

#define MAXN 1100

using namespace std;
int num_interval;
struct interval
{
    double left, right;
}intervals[MAXN];

bool cmp(interval v1, interval v2)
{
    return v1.left < v2.left;
}

interval cal_interval(int x, int y, int d) // ÅÐ¶ÏÊÇ·ñ³ö½ç~
{
    interval ret;
    double temp = sqrt(d*d - y*y);
    ret.left = x-temp;
    ret.right =x+temp;
    return ret;
}
int main()
{
    int n, d;
    int x,y;
    bool tag;
    int cnt =1, ans;
    double left_now, right_now, left_temp, right_temp;
    string str;
    while(cin >> n >>d)
    {
        if(n==0 && d==0) return 0;
        num_interval =0;
        ans = 1;
        if (d>0) tag = true; else tag = false;
        for(int i=0; i<n; i++)
        {
            cin>>x>>y;
            if(y>d || y<0) tag = false;
            if(!tag) continue;
            intervals[num_interval++] = cal_interval(x, y, d);
        }
        if(!tag) {cout<< "Case " << cnt <<": " <<-1<<endl; getline(cin, str); cnt++; continue;}
        sort(intervals, intervals+num_interval, cmp);
//        for(int i=0; i<num_interval; i++)
//        {
//            cout<<intervals[i].left <<' '<<intervals[i].right<<endl;
//        }
//        cout<<"--------"<<endl;

        left_now = intervals[0].left; right_now = intervals[0].right;
//        cout<<left_now<<' '<<right_now<<endl;

        for(int i=1; i<num_interval; i++)
        {

            left_temp = intervals[i].left;
            right_temp = intervals[i].right;

            if(right_temp <= right_now) right_now = right_temp;
            else
            {
                if(left_temp > right_now)  { ans++; right_now = right_temp;}
            }
            left_now = left_temp;
//            cout<<left_now<<' '<<right_now<<endl;
        }
        cout<< "Case " << cnt <<": " <<ans<<endl; getline(cin, str); cnt ++;
    }
    return 0;
}
