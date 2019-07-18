/*
这道题重点在理解题目上：
1.n必须为偶数，奇数无法完成任务；
2.n个硬币需要翻转n次；
3.第i次翻转时，除了第i个硬币外，其余硬币都需要翻转；
4.翻转可以用a[j]=1-a[j]%2;这个式子巧妙地构造。
*/

#include<iostream>
#include<string>
using namespace std;
int main()
{
    int n=0;
    int a[101]={0};
    cin>>n;
    cout<<n<<endl;
    for(int i=0;i<n;i++)
    {
        for(int j=0;j<n;j++)
        {
            if(j!=i)
                a[j]=1-a[j]%2;
            cout<<a[j];
        }
        cout<<endl;
    }
    return 0;
}
