#ifndef ZZB_H_INCLUDED
#define ZZB_H_INCLUDED
#include<string.h>
#include<stdlib.h>
#include<iostream>
using namespace std;
int array[100][30]={0};
int counter =0;
class Proposition
{
	int data;char data1;  //data:�������ֵ��0/1�� data1��������
public:
	Proposition(){data=2;data1='A';};
	friend Proposition operator !(Proposition &q); //����������!
	friend Proposition operator &(Proposition &p,Proposition &q);//�����ȡ�����&
	friend Proposition operator |(Proposition &p,Proposition &q);//������ȡ�����|
	friend Proposition operator >(Proposition &p,Proposition &q);//�����̺������>
	friend Proposition operator <(Proposition &p,Proposition &q);//�����ֵ�����<
	void input1(int x){data=x;}      //����data
	void input2(char x){data1=x;}   //����data1
	int get(){return data;}         //��ȡ��ֵ
	int get1(){return data1;}       //��ȡ������
};
Proposition operator !(Proposition &q)
{q.input1(1-q.get()); return q;}
Proposition operator &(Proposition &p,Proposition &q)
{p.input1((p.get()+q.get())/2); return p;}
Proposition operator |(Proposition &p,Proposition &q)
{if(p.get()+q.get()>0) p.input1(1);
else p.input1(0);
return p;}
Proposition operator >(Proposition &p,Proposition &q)
{if(p.get()==1&&q.get()==0)
p.input1(0);
else p.input1(1);
return p;}
Proposition operator <(Proposition &p,Proposition &q)
{if(p.get()==q.get())  p.input1(1);
else p.input1(0);
return p;}
void fuzhi(string A,int N,int M,Proposition a[]);
int bds(string A,int N,Proposition a[]);
int jisuan(string A,int &i,int L,int N,Proposition a[]);
void fuzhi(string A,int N,int M,Proposition a[])//���⸳ֵ��������ֵ
{
	for(int j=0;j<2;j++)
	{
		a[N-1].input1(j);
		if(N>1)
		{
		fuzhi(A,N-1,M,&a[0]);
		}
		else
		{
		    if(bds(A,M,&a[0])==1)
            {
                for(int p=0;p<M;p++)
                {
                    array[counter][p]=a[p].get();
                }
                counter++;
            }
            for(int i=0;i<M;i++)
            {
                cout<<a[i].get()<<"  ";
            }

			cout<<bds(A,M,&a[0])<<endl;
		}
	}
}
int bds(string A,int N,Proposition a[]) //ʶ������ı��ʽ��������ֵ
{
	Proposition ANS,Temp;
	char d[5]={'!','|','&','>','<'};
	int L;
	int i=0;
	L=strlen(&A[0]);  //������ʽ����
	while(i<L)
	{
		if(A[i]=='(')  //���ŵĴ���
		{
		int k=1;
		for(int j=i+2;j<L;j++)
		{
			if(k!=0)
			{
				if(A[j]=='(')
					k++;
				if(A[j]==')')
					k--;
			}
			if(k==0)
			{
				int l=j-i-1;
				char *p=new char[l+1];
				string B;
			for(int m=0;m<l;m++)
				p[m]=A[i+1+m];
			p[l]='\0';
			B=p;
			delete p;
			Temp.input1(bds(B,N,&a[0]));
			if(i==0)
				ANS.input1(Temp.get());
			i=j+1;
			break;
			}
		}
		}
	else
	{
		if(A[i]=='!')   //�񶨵ļ���
		{
				Temp.input1(jisuan(A,i,L,N,&a[0]));
				ANS=!Temp;
				i++;
				continue;
		}
		else if(A[i]=='|')
		{
			Temp.input1(jisuan(A,i,L,N,&a[0]));
			ANS=ANS|Temp;
			i++;
			continue;
		}
		else if(A[i]=='&')
		{
			Temp.input1(jisuan(A,i,L,N,&a[0]));
			ANS=ANS&Temp;
			i++;
			continue;
		}
		else if(A[i]=='<')
		{
			Temp.input1(jisuan(A,i,L,N,&a[0]));
			ANS=ANS<Temp;
			i++;
			continue;
		}
		else if(A[i]=='>')
		{
			Temp.input1(jisuan(A,i,L,N,&a[0]));
			ANS=ANS>Temp;
			i++;
			continue;
		}
		else
		{
			for(int j=0;j<N;j++)
			{if(A[i]==char(a[j].get1()))
			{
				Temp.input1(a[j].get());
				if(i==0)
					ANS.input1(Temp.get());
				i++;
				break;
			}
			}
		}
	}
	}
	return ANS.get();
}
int jisuan(string A,int &i,int L,int N,Proposition a[])
{
	Proposition Temp;
	if(A[i+1]=='(')
	{
		int k=1;
		for(int j=i+2;j<L;j++)
		{
			if(k!=0)
			{
				if(A[j]=='(')
					k++;
				if(A[j]==')')
					k--;
			}
			if(k==0)
			{
				int l=j-i-1;
				char *p=new char[l+1];
				string B;
				for(int m=0;m<l-1;m++)
					p[m]=A[i+2+m];
				p[l-1]='\0';
				B=p;
				delete p;
				Temp.input1(bds(B,N,&a[0]));
				i=i+j;
				break;
			}
		}
	}
	else
	{
		int j=0;
		while(j<L)
		{
			if(A[i+1]==char(a[j].get1()))
			{
				i++;
				Temp=a[j];
				break;
			}
			j++;
		}
	}
	return Temp.get();
}



#endif // ZZB_H_INCLUDED
