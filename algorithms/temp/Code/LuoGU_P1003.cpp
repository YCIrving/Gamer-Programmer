/*洛谷上第一次1Y，虽然之前做的题目比较简单，但是都没有一次成功过
所以还是很惊讶这次能够1Y！
不过这道题的思想是看了论坛才想出来的，因为自己的思路（开辟一个相当于地板的二维数组
然后根据将地毯覆盖的区域赋值为地毯编号）会爆掉，所以直接放弃实现，看了一眼论坛才知道本题
的正确思路：先把地毯的铺设情况存储起来，然后从最上面一张地毯开始验证点是否被其覆盖即可。
*/

#include<iostream>
using namespace std;
int main()
{
    int floor[100000][4]={0};
    int n,a,b,ans;
    cin>>n;
    for(int i=0;i<n;i++)
    {
        cin>>floor[i][0]>>floor[i][1]>>floor[i][2]>>floor[i][3];
    }
    cin>>a>>b;
    for(ans=n-1;ans>=0;ans--)
    {
        if(a>=floor[ans][0]&&a<=floor[ans][0]+floor[ans][2]&&b>=floor[ans][1]&&b<=floor[ans][1]+floor[ans][3])
            break;
    }
    if(ans==-1)
        cout<<ans;
    else
    cout<<++ans;
    return 0;
}
