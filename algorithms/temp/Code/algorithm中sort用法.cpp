#include<iostream>
#include<algorithm>
using namespace std;
bool compare(int a,int b)//定义sort算子，表示如何比较两个元素，其中a>b，表示降序排列
{
    return a>b;
}
int main()
{
    int a[10]={1,2,5,6,9,3,4,7,8,0};
    sort(a,a+10,compare);
    for(int i=0;i<10;i++)
    {
        cout<<a[i]<<' ';
    }
    cout<<endl;
    sort(a,a+10);//默认是升序排列
        for(int i=0;i<10;i++)
    {
        cout<<a[i]<<' ';
    }
    cout<<endl;
}
