//****************************真值表.CPP*****************************
// 真值表.cpp : 定义控制台应用程序的入口点。
//#include "stdafx.h"
#include"zzb.h"
int main()
{
	Proposition a[30];
	char x='1';
	int i=0,N;
	cout<<"请输入涉及的命题变元(输入‘0’按回车结束)"<<endl;
	while(int(x)!=48)
	{
		cin>>x;
	    if(i>19)
		{cout<<"命题变元个数超过30，无法处理"<<endl;break;}
		if(x!='0')
		{
			a[i].input2(x);
			i++;
		}
	}
	N=i;
	int M;
	M=N;
	string A;
	cout<<"请输入命题公式(  否定：！，合取：&，析取：|，蕴含>，等值：<   )"<<endl;
	cin>>A;
	cout<<A<<"的真值表为："<<endl;
	for(int j=0;j<M;j++)
	cout<<char(a[j].get1())<<"  ";
	cout<<"真值"<<endl;
	fuzhi(A,N,M,&a[0]);
	cout<<A<<"的主析取范式为："<<endl;
	for(int i=0;i<counter;i++)
    {
        cout<<'(';
        for(int j=0;j<M;j++)
        {
            if(array[i][j]==1)
                cout<<char(a[j].get1())<<'&';
            else
                cout<<'!'<<char(a[j].get1())<<'&';
        }
        cout<<'\b'<<')';
        if(i!=counter-1)
            cout<<'|';
    }
	return 0;
}
//*******************************************************************
//********************************zzb.h******************************

//*******************************************************************
