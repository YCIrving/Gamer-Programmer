/*输入N个（N<=10000）数字，求出这N个数字中的最大值和最小值。
每个数字的绝对值不大于1000000。*/
#include<iostream>
using namespace std;
int main()
{
    int n,min,max,a[10000];
    while(cin>>n)
    {
        cin>>a[0];
        min=max=a[0];
        for(int i=1;i<n;i++)
        {
            cin>>a[i];
            if(a[i]<min)
                min=a[i];
            if(a[i]>max)
                max=a[i];
        }
        cout<<max<<' '<<min<<endl;
    }
    return 0;
}
