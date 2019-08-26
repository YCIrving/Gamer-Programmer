#include <iostream>
#include <math.h>
using namespace std;
int judge(int x)
{
    int s = 0;
    int cnt = 0;

    for (int i = 1; i*i <= x; ++i)
    {
        if (x%i == 0)
        {
            if(i%2 !=0) cnt++;
            else cnt--;

            if (i*i != x)
            {
                if((x / i)%2 !=0) cnt++;
                else cnt--;
            }
        }
    }

    if(abs(cnt)<=2) return 1;
    return 0;
}

int main()
{
    int t;
    int l, r, ans;
    cin>>t;
    for(int caseID=1; caseID<=t; caseID++)
    {
        ans = 0;
        cin>>l>>r;
        for(int x=l; x<=r; x++)
        {
            ans += judge(x);
        }
        cout<<"Case #"<<caseID<<": "<<ans<<endl;
    }
    return 0;
}
