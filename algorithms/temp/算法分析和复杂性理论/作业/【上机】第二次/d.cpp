//OneNote 贪心->区间问题1

#include <iostream>
#include <math.h>
#include <algorithm>
#include <string>

#define MAX 1100
using namespace std;

struct interval
{
    double l, r;
}itv[MAX];

bool cmp (interval v1, interval v2)
{
    return v1.l < v2.l;
}

int main()
{
    int n,d, tag, ans, cnt = 1;
    double x, y, temp;
    double left, right;
    string str;
    while(cin >>n>>d )
    {
        if(n == 0 && d == 0)
        {
            return 0;
        }
        ans = 1;
        tag = 0;
        for(int i=0; i<n; i++)
        {
            cin >> x >> y;
            temp = pow(d, 2) - pow(y, 2);
            if (temp < 0 || d < 0 || y < 0 )
            {
                tag = -1;
                continue;
            }
            temp = sqrt(temp);
            itv[i].l = x - temp;
            itv[i].r = x + temp;
        }
        if(tag == -1)
        {
            cout << "Case "<< (cnt++) <<": -1" <<endl;
            getline(cin, str);
            continue;
        }
        sort(itv, itv + n, cmp);
        left = itv[0].l;
        right = itv[0].r;
        for(int i=1; i<n; i++)
        {

            if(itv[i].l > right)
            {
                ans ++;
                right = itv[i].r;
            }
            else
            {
                right = min(right, itv[i].r);
            }
            left = itv[i].l;

        }
        cout <<"Case "<< (cnt++) <<": " <<ans<<endl;
        getline(cin, str);
    }
    return 0;
}
