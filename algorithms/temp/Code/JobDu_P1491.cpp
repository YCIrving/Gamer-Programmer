//����ⲻ̫�����ô�����Ժ�������
/*
����������Ĵ��¼��㷽ʽ�ǣ���������12345����ô����F(1)������F(1)���F(12)��
���������F(123)��F(1234)��F(12345)��
��Ҫ����ľ���rslt=(rslt-cnt)*10+num*2+cnt*(t+1)+min(t,2);��һ���ˡ�
rslt�ǽ����num��ʵ�����������cnt�ǽ���num�������1,2��������
������Ϊ�Ѿ������F(12)=rslt����Ҫ��F(123)
1.��Ȼ12�����12X����ʽ������ԭ��1-11��ÿһ���������Լ���һ����λ
(��Ϊ��֪����λ�ǲ���9���������һ��12���ܳ�10���Ժ󵥶�����)��
����λ�Ǵ�0-9������ԭ����ÿ��������10�б仯�������һ����12��1,2������Ϊcnt��
������(rslt-cnt)*10,��
2.��һ��������һ��12û�м��㣬��Ȼ12X������ĸ�λֻ����0-3��������cnt*(t+1)��
4.����ļ����ж�û�п��ǶԸ�λΪ1,2�ļ�����10-120�и�λΪ1,2����Ȼ��ÿ10��������2����
������num*2
3.��һ������һ��0û���㣬��Ϊ0��ֻ�и�λ��ֵ��ֱ���жϾͺ��ˣ�min(t,2)��
*/

#include<iostream>
#include<string>
using namespace std;
int main()
{
    string str(102,'\0');
    int rslt,num,cnt;
    while(cin>>str)
    {
       rslt=0,num=0,cnt=0;
        int n=str.length();
        for(int i=0;i<n;i++)
        {
            int t=str[i]-'0';
            rslt=(rslt-cnt)*10+num*2+cnt*(t+1)+min(t,2);
            num=num*10+t;
            if(t==1||t==2)
                cnt++;
            num%=20123;//��仰�����
            rslt%=20123;
        }
          cout<<rslt<<endl;
    }
    return 0;
}
/*
#include <stdio.h>
#include <string.h>

int min(int a,int b)    {return (a<b)?a:b;}
int main()
{
    const int MOD=20123;
    const int MAX_LEN=101;
    char str[MAX_LEN+1];
    int n;
    while(scanf("%s",str)>0)
    {
        int i,t;
        int rslt=0,num=0,cnt=0;
        n=strlen(str);
        for(i=0;i<n;i++)
        {
            t=str[i]-'0';
            rslt=(rslt-cnt)*10+num*2+cnt*(t+1)+min(t,2);
            num=num*10+t;
            if(t==1 || t==2)    cnt++;

            num%=MOD;
            rslt%=MOD;
        }
        printf("%d\n",rslt);
    }
    return 0;
}
