#include<iostream>
using namespace std;
void move(int n,char source,char trans,char dest)
{
    if(n==1)
        cout<<source<<" to "<<dest<<endl;
    else
    {
        move(n-1,source,dest,trans);//��sourceͨ��dest�ƶ���mid
        move(1,source,trans,dest);
        move(n-1,trans,source,dest);//��midͨ��source�ƶ���dest
    }
}
int main()
{
    int n;
    while(cin>>n)
    {
        move(n,'A','B','C');
        cout<<endl;
    }
    return 0;
}
/*���׷��Ĵ�����Ǻ��Խ����ڹ��ɵ����ӷ��ڲ����У���ʵ��n>2ʱ���ƶ���������Ҫ���ɵġ�
A����A�ϵ�n-1������2������Ϊn������Բ���Ƶ�B��������C�����������£�
��1����A�ϵ�n��-1������1����Բ���Ƶ�C�ϡ�//�ص㣡
��2����A�ϵ�һ��Բ���Ƶ�B��
��3����C�ϵ�n��-1������1����Բ���Ƶ�B��//�ص㣡
B����A�ϵ�һ��Բ���Ƶ�C��
C����B�ϵ�n-1������2������Ϊn������Բ���Ƶ�C������A�����������£�
��1����B�ϵ�n��-1������1����Բ���Ƶ�A��//�ص㣡
��2����B�ϵ�һ�������Ƶ�C��
��3����A�ϵ�n��-1������1����Բ���Ƶ�C��//�ص㣡
���ˣ����������Բ�̵��ƶ����̡�*/
