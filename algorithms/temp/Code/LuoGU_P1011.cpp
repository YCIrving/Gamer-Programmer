/*
�������ĺô���������ͨģ�⣬�Լ�Ҳ���������⣬���Ǿ�Ȼû��������쳲��������У�
���˺þã��������������������Ļ����϶������ˡ�
�Լ�һ��ʼ�Ĵ���ע���ˣ�дһ���µĳ���


�ܽ�һ�¹��ɣ�
1.��iվ�³��������ڵ�i-1վ���ϳ������������м�����³����ֲ��ùܾͺ�
���ؼ������n-1վ���ϳ��������³����ùܣ���Ҫ�����1վ�ϳ����Ǹ�a�͵�2վ�³���b��
2.�ٿ�һ���ϳ����ɣ�
��fibo[]={0,1,1,2,3,5 ...}
��1վ���ϳ�1a + 0b;
��2վ���ϳ�0a + 1b;
��3վ���ϳ�1a + 1b;
��4վ���ϳ�1a + 2b;
��5վ���ϳ�2a + 3b��
��6վ���ϳ�3a + 5b��
����
��n-1վ���ϳ�fibo(n-2)*a+fibo(n-1)*b
���Ϲ���������a-b+fibo(n-1)*a+fibo(n)*b=m
���xվ����ʱ������Ϊ��1վ��a�͵�xվ�ϳ�������֮��,ͬ��Ҳ����
*/

#include<iostream>
using namespace std;
int a,n,m,x;
int ans=0;
int b=0;
int fibo[21]={0};
int get_b()
{
    fibo[2]=1;
    for(int i=3;i<=n;i++)
    {
        fibo[i]=fibo[i-2]+fibo[i-1];
    }
    b=(m-a*(fibo[n-2]+1))/(fibo[n-1]-1);
}
int get_ans()
{
    ans=a-b+fibo[x-1]*a+fibo[x]*b;
}
int main()
{
    cin>>a>>n>>m>>x;
    if(n==x)
    {
        cout<<0;
        return 0;
    }
    get_b();
    get_ans();
    cout<<ans;
    return 0;
}
/*
#include<iostream>
using namespace std;
int a,n,m,x,ans=0;
int y=0;
int up[21]={0};
int down[21]={0};
int fac_a=1,fac_y=0;
int factor_a_up[21]={0},factor_y_up[21]={0};
int factor_a_down[21]={0},factor_y_down[21]={0};
int getFactor()
{

    factor_a_up[1]=1;
    factor_y_up[2]=1;
    factor_y_down[2]=1;
    for(int i=3;i<n;i++)
    {
        factor_a_up[i]=factor_a_up[i-2]+factor_a_up[i-1];
        factor_y_up[i]=factor_y_up[i-2]+factor_y_up[i-1];
        factor_a_down[i]=factor_a_up[i-1];
        factor_y_down[i]=factor_y_up[i-1];
        //cout<<factor_a_up[i]<<' '<<factor_y_up[i]<<' '<<factor_a_down[i]<<' '<<factor_y_down[i]<<endl;
        fac_a+=(factor_a_up[i]-factor_a_down[i]);
        fac_y+=(factor_y_up[i]-factor_y_down[i]);
    }
    factor_a_up[n]=0;
    factor_y_up[n]=0;
    return 0;
}
int solve()
{
    y=(m-fac_a*a)/fac_y;
    fac_a=fac_y=0;
    for(int i=1;i<=x;i++)
    {
        fac_a+=(factor_a_up[i]-factor_a_down[i]);
        fac_y+=(factor_y_up[i]-factor_y_down[i]);
    }
    ans=fac_a*a+fac_y*y;
}
int main()
{

    cin>>a>>n>>m>>x;
    if(x==n)
    {
        cout<<0;
        return 0;
    }
    getFactor();
    solve();
    cout<<ans;
    return 0;
}
*/
