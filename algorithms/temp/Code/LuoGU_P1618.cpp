//�Լ��Ĵ��룬����ʵ���㷨������һ�����������֣�
//��Ҳ����Ҫȫ�����������ݱ�����������ĳ����Ϊֹ���ɣ�
//Ҫע����ǲ�Ҫ����987������ޣ��ص�1��
//�����Ժ�Ҫ�ں����ﶨ�����飬�ᱨRunTime Error

#include<iostream>
using namespace std;
int tag=0;
int flag[10]={0};
int gcd(int m,int n)
{
    if(m%n==0)
        return n;
    else
        return gcd(n,m%n);
}
int gcd3(int a,int b,int c)
{
    int gcd_two=gcd(a,b);
    return gcd(gcd_two,c);
}
bool setFlag(int *flag,int n)
{
    int n1,n2,n3;
    n1=n%10;
    if(n1!=0&&flag[n1]==0)
    {
        flag[n1]++;
    }
    else
        return false;
    n2=(n/10)%10;
    if(n2!=0&&flag[n2]==0)
    {
        flag[n2]++;
    }
    else
        return false;
    n3=n/100;
    if(n3!=0&&flag[n3]==0)
    {
        flag[n3]++;
    }
    else
        return false;

    return true;

}
bool test(int o,int p,int q)
{
    for(int i=0;i<10;i++)
    {
        flag[i]=0;
    }
    if(setFlag(flag,o)&&setFlag(flag,p)&&setFlag(flag,q))
    {
        cout<<o<<' '<<p<<' '<<q<<endl;
        tag=1;
        return true;
    }
    else
        return false;

}
int main()
{
    int a,b,c;
    int o,p,q;
    cin>>a>>b>>c;
    int gcd_three=gcd3(a,b,c);
    a/=gcd_three;
    b/=gcd_three;
    c/=gcd_three;
    int end=((987/c)+1)*c;
    for(int o=123;o<=end;o++)
    {
        if(o%a==0)
        {
            p=o/a*b;
            q=o/a*c;
            if(p<=987&&q<=987)//�ص�1
            {
                test(o,p,q);
            }
            else
                continue;
        }
        else
            continue;
    }
    if(tag==0)
    {
        cout<<"No!!!";
    }
    return 0;
}


//����C++��ȫ���м������ֵ���ϣ����������ʱ�ͽ��������ǽ�ȥ��������ϵĴ�����
//ȫ����Ҳ�������������������ģ�⡣
//�������ֵı���������Ҫ����һ�£��ص�1����Ȼ���ص�2����ȫ���е���д��ʽ��
//ע�⺯��������ʵ�ǵ�ַ����������ֵ��+1��+10��+10Ŀǰ�����ǽ�β��λ�ã�
//�����ǰ����ֵ��������еģ������ʼ���ֵ�������򲻻����ִ��

/*
#include<iostream>
#include<algorithm>
using namespace std;

int main()
{
    int num[10]={0,1,2,3,4,5,6,7,8,9};
    int a,b,c;
    cin>>a>>b>>c;
    bool tag=false;
    do
    {
        int x=num[1]*100+num[2]*10+num[3],
        y=num[4]*100+num[5]*10+num[6],
        z=num[7]*100+num[8]*10+num[9];
        if(x*b==y*a&&x*c==z*a&&y*c==b*z)//�ص�1
        {
            tag=true;
            cout<<x<<' '<<y<<' '<<z<<endl;
        }
    }while(next_permutation(num+1,num+10));//�ص�2
    if(!tag)
        cout<<"No!!!";
    return 0;
}
*/
