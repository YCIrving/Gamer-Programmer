//OneNote 数学->错位排列

#include <iostream>
#include <iomanip>
using namespace std;
int main()
{
    int d[100000]={0};
    int n;
    int frac=1;
    cin>>n;
    d[1]=0;
    d[2]=1;
    for (int i=3;i<=n;i++)
    {
        d[i]=(i-1)*(d[i-1]+d[i-2]);
    }
    double ans;
    for(int i=2;i<=n;i++)
    {
        frac*=i;
    }
    ans=((double)(frac-d[n])/frac);
    cout<<setiosflags(ios::fixed)<<setprecision(4)<<ans<<endl;
    return 0;
}
