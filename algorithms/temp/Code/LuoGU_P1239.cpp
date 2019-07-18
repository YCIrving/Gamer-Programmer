/*
这道题值得注意的是，如果用int来定义变量，则程序会超时，
而如果用unsigned int 来定义变量，则不会超时

*/
#include<iostream>
using namespace std;
int main()
{
    std::ios::sync_with_stdio(false);//加快cin和cout
    unsigned int n=0,page;//关键变量定义，可能在取余和除法时，unsigned int要比int快一些
    unsigned int ans[10]={0};
    cin>>n;
    for(unsigned int i=1;i<=n;i++)
    {
        page=i;
        while(page)
        {
            ans[page%10]++;
            page/=10;
        }
    }
    for(unsigned int i=0;i<10;i++)
        cout<<ans[i]<<endl;
    return 0;
}
