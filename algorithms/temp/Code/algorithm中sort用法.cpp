#include<iostream>
#include<algorithm>
using namespace std;
bool compare(int a,int b)//����sort���ӣ���ʾ��αȽ�����Ԫ�أ�����a>b����ʾ��������
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
    sort(a,a+10);//Ĭ������������
        for(int i=0;i<10;i++)
    {
        cout<<a[i]<<' ';
    }
    cout<<endl;
}
