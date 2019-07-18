//Onenote 动态规划：最大双子序列和

#include <iostream>
#include <algorithm>
#include <string>
#define MAX 50100

using namespace std;
int a[MAX], f1[MAX], g1[MAX], f2[MAX];
int main()
{
    int t;
    string str;
    cin>>t;
    int n;
    while(t--)
    {
        getline(cin, str);
        getline(cin, str);
        cin>>n;
        for(int i=0; i<n; i++)
        {
            cin>>a[i];
        }
        // f1[0] = max(0, a[0]);
        // 必须要选择一个元素相加，即不允许买空
        f1[0] = a[0];
        g1[0] = a[0];
        int g= g1[0], ans = 0;
        for(int i=1; i<n; i++)
        {
            // f1[i] = max(0, a[i] + f1[i-1])
            // 不允许买空
            f1[i] = a[i] + max(0, f1[i-1]);
            if( f1[i] > g)
            {
                g = f1[i];
            }
            g1[i] = g;
        }
        // f2[0] = max(0, a[0])
        // f2[1] = g1[0] + max(0, a[1])
        // 同样不允许买空
        f2[1] = g1[0] + a[1];
        ans = f2[1];
        for(int i=2; i<n; i++)
        {
            // f2[i] = max(g1[i-1], a[i] + f2[i-1]);
            // 不允许买空
            f2[i] = a[i] + max(g1[i-1], f2[i-1]);
            if( f2[i] > ans)
            {
                ans = f2[i];
            }
        }
        cout<<ans<<endl;
    }
    return 0;
}
