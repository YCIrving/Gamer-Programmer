#include <iostream>
#include <algorithm>

#define MAXINT 100005

using namespace std;
// 注意二分查找停止条件，返回左值
int n, m;
int a[MAXINT] ={0};

bool check(int mid)
{
    int t=1, sum=0;
    for(int i=0; i<n; i++)
    {
        if(sum+a[i]>mid)
        {
            t++;
//            sum =0;
//            i--;
            sum = a[i];
        }
        else
        {
            sum+=a[i];
        }
    }
    if(t>m)
    {
        return false;
    }
    return true;
}

int getans(int max_a, int sum)
{
    int l = max_a,  r = sum, mid;
    while(l<=r)
    {
        mid = (l + r)/2;
        if(check(mid))
        {
            r = mid -1;
        }
        else
        {
            l = mid +1;
        }
    }
    return l;
}
int main()
{
    int sum=0, max_a=0, ans;
    cin>>n>>m;
    for(int i=0; i<n; i++)
    {
        cin>>a[i];
        sum+=a[i];
        max_a = max(max_a, a[i]);
    }
    ans = getans (max_a, sum);
    cout<<ans<<endl;
    return 0;
}
