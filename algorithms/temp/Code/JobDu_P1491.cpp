//这道题不太清楚怎么做，以后来看吧
/*
按我理解他的大致计算方式是：比如输入12345，那么先算F(1)，再由F(1)算出F(12)，
依次再算出F(123)，F(1234)，F(12345)；
主要计算的就是rslt=(rslt-cnt)*10+num*2+cnt*(t+1)+min(t,2);这一句了。
rslt是结果；num是实际输入的数；cnt是仅仅num这个数中1,2的数量；
首先认为已经算出了F(12)=rslt，需要算F(123)
1.显然12变成了12X的形式，所以原来1-11中每一个数都可以加上一个个位
(因为不知道个位是不是9，所以最后一个12不能乘10，以后单独计算)，
而个位是从0-9，所以原来的每个数都有10中变化，而最后一个数12的1,2的数量为cnt，
所以有(rslt-cnt)*10,；
2.上一步中余有一个12没有计算，显然12X这个数的个位只能是0-3，所以有cnt*(t+1)；
4.上面的记算中都没有考虑对个位为1,2的记数，10-120中个位为1,2的显然是每10个数中有2个，
所以有num*2
3.第一步还有一个0没计算，若为0则只有个位有值，直接判断就好了，min(t,2)；
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
            num%=20123;//这句话必须加
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
