/*�ó���ʾ��ּ��˵���ڳ�������ο���С����������
����iomanipͷ�ļ���Ȼ�������ǰ����std::fixed���ɲ��ÿ�ѧ�����������
Ȼ����setprecision����������ʾ��С����λ����Ĭ��Ϊ6λ*/



#include<iostream>
#include <iomanip>
using namespace std;
int main()
{
    double a=0;
    cin>>a;
    cout<<a<<endl;
    cout<<std::fixed<<a<<endl;
    cout<<std::fixed<<setprecision(0)<<a<<endl;//����������
    cout<<std::fixed<<setprecision(2)<<a;
    return 0;
}


