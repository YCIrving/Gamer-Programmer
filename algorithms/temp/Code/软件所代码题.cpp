#include<iostream>
#include<iomanip>
using namespace std;
int ans=0;//�����н�������
int cnt=0;//������������
double probability=0;//�������ո���
char symbol[4]={'+','-','*','/'};//�������
int calculateOutput(int a,char ch,int b);//������������һ������Ľ��
bool judgeGroup(int a,int b,int c,int d);//����һ��ָ��˳������Ƿ��н�
bool judge(int a,int b,int c,int d);//����һ�����Ƿ��н⣨���޶�˳��

bool judge(int a,int b,int c,int d)
{
    //�ĸ����������Ƿ������ͬ��������4!=24��
    //�������������ţ�ÿ�������������������4^3=64�֣���������˳��Ĭ�ϴ������Ҽ��㣩
    //����һ����Ҫ����24*64=1536�����
    int arr[4]={a,b,c,d};
    for(int i=0;i<4;i++)
    {
        for(int j=0;j<4;j++)
        {
            for(int k=0;k<4;k++)
            {
                for(int l=0;l<4;l++)
                {
                    if(i!=j&&i!=k&&i!=l&&j!=k&&j!=l&&k!=l)//��֤�ĸ���ȡ����ͬ������
                    {
                        if(judgeGroup(arr[i],arr[j],arr[k],arr[l]))
                            return true;
                    }
                }
            }
        }
    }
    //cout<<a<<' '<<b<<' '<<c<<' '<<d<<endl;��������޽���ϣ�������
            return false;

}
bool judgeGroup(int a,int b,int c,int d)
{
    int output=0;
    for(int i=0;i<4;i++)
    {
        for(int j=0;j<4;j++)
        {
            for(int k=0;k<4;k++)
            {
                if(i!=3||(a%b==0))//��һ����֤����������ʱ�ܹ�����
                {
                    output=calculateOutput(a,symbol[i],b);
                }
                if(j!=3||(output%c==0))
                {
                    output=calculateOutput(output,symbol[j],c);
                }
                if(k!=3||(output%d==0))
                {
                    output=calculateOutput(output,symbol[k],d);
                }
                if(output==24)
                {
                    //cout<<a<<symbol[i]<<b<<symbol[j]<<c<<symbol[k]<<d<<"=24"<<endl;
                    return true;
                }
                output=0;//����output
            }
        }
    }
    return false;
}
int calculateOutput(int a,char ch,int b)//�򵥵���������
{
    int output=0;
    switch (ch)
    {
    case '+':
        output =a+b;
        break;
    case '-':
        output=a-b;
        break;
    case '*':
        output=a*b;
        break;
    case '/':
        output=a/b;
    }

    return output;
}
int main()
{
    int a[36];
    for(int i=0,j=1;i<36;i+=4,j++)//��ʼ������
    {
        a[i]=a[i+1]=a[i+2]=a[i+3]=j;
    }
//    int b,c,d,e;
//    cin>>b>>c>>d>>e;
//    judge(b,c,d,e);
//    for(int i=0;i<36;i++)
//        cout<<a[i]<<' ';
    for(int i=0;i<36-3;i++)
    {
        for(int j=i+1;j<36-2;j++)
        {
            for(int k=j+1;k<36-1;k++)
            {
                for(int l=k+1;l<36;l++)
                {
                    cnt++;//C(36,4)�����
                    if(judge(a[i],a[j],a[k],a[l]))
                    ans++;
                }
            }
        }
    }
    cout<<"Find "<<ans<<" solutions "<<"in "<<cnt<<" cases."<<endl;
    probability=(double)ans/cnt;
    cout<<fixed<<setprecision(2)<<"The probability is about "<<probability<<'.'<<endl;
    //cout<<judgeCase(5,'/',2);
    return 0;

}
