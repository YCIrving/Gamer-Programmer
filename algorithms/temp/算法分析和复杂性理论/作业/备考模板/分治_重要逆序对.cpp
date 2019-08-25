// ���������η�����Ҫ�����

#include <iostream>
#include <stdio.h>
#define MAXN 210000
using namespace std;

int a[MAXN];
int n;

long long solve(int l, int r)
{
//    cout<<l<<' '<<r<<endl;
    if(l ==r) return 0;
    long long ans=0;
    int mid = (l+r)/2;
    ans += solve(l, mid);
    ans += solve(mid+1, r);
    int temp[r-l+1]; //�˴������ö��ٽ����٣�����MAXN������ᵼ�³�����
    int k=0, pos_l=l, pos_r = mid+1;
    // ��һ��ѭ���������¼��Ҫ�����
    while(pos_l <= mid && pos_r <=r)
    {
        if(a[pos_l] <= 2*a[pos_r])
        {
            pos_l ++;
        }
        else
        {
            ans+= mid - pos_l +1;
            pos_r++;
        }
    }

    // �ڶ���ѭ��������鲢����
    pos_l=l;
    pos_r = mid+1;
    while(pos_l <= mid && pos_r <=r)
    {
        if(a[pos_l] <= a[pos_r])
        {
            temp[k++] = a[pos_l];
            pos_l ++;
        }
        else
        {
            temp[k++] = a[pos_r];
            pos_r++;
        }
    }
    while( pos_l<=mid)
    {
        temp[k++] =a[pos_l++];
    }
    while(pos_r<=r)
    {
        temp[k++] = a[pos_r++];
    }
    for(int i=l; i<=r; i++)
    {
        a[i] = temp[i-l];
    }
    return ans;
}
int main()
{
    long long ans;
    while(cin>>n && n)
    {
        for(int i=0; i<n; i++)
        {
            cin>>a[i];
        }
        ans = solve(0, n-1);
        printf("%lld\n", ans);
    }

}
