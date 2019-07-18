/*
题目不是很难，不知道为什么之前没有做……
首先，提示很重要。产生回文数的时候用到的循环很重要。
另外，就是偶数位的回文数并且是质数的只有11，4、6、8等都不是质数
还有，判断质数的时候，循环的结束条件是i*i<=n，等号很重要
*/

#include<iostream>
#include<math.h>
using namespace std;
int getNum(int n)
{
    int i=1;
    for(;;i++)
    {
        if((n/(int)(pow(10,i))==0))
            break;
    }
    return i;
}
bool judge(int n)
{
    if(n<2)
        return false;
    for(int i=2;i*i<=n;i++)
    {
        if((n%i)==0)
            return false;
    }
    return true;
}
void out1(int min,int max)
{
    if(2>=min&&2<=max)
        cout<<2<<endl;
    if(3>=min&&3<=max)
        cout<<3<<endl;
    if(5>=min&&5<=max)
        cout<<5<<endl;
    if(7>=min&&7<=max)
        cout<<7<<endl;
}
void out2(int min,int max)
{
    if(11>=min&&11<=max)
        cout<<11<<endl;
}
void out3(int min,int max)
{
    int temp;
    for(int i=1;i<10;i+=2)
        for(int j=0;j<10;j++)
    {
        temp=i*100+j*10+i;
        if((judge(temp)))
        {
            if(temp>=min&&temp<=max)
                cout<<temp<<endl;
        }
    }
}
void out5(int min,int max)
{
    int temp;
    for(int i=1;i<10;i+=2)
        for(int j=0;j<10;j++)
            for(int k=0;k<10;k++)
    {
        temp=i*10000+j*1000+k*100+j*10+i;
        if(judge(temp))
        {
            if(temp>=min&&temp<=max)
                cout<<temp<<endl;
        }
    }
}
void out7(int min,int max)
{
    int temp;
    for(int i=1;i<10;i+=2)
        for(int j=0;j<10;j++)
            for(int k=0;k<10;k++)
                for(int l=0;l<10;l++)
    {
        temp=i*1000000+j*100000+k*10000+l*1000+k*100+j*10+i;
        if(judge(temp))
        {
            if(temp>=min&&temp<=max)
                cout<<temp<<endl;
        }
    }
}
int main()
{
    int low,high;
    int min,max;
    cin>>low>>high;
    min=getNum(low);
    max=getNum(high);
    if(min<=1&&max>=1)
        out1(low,high);
    if(min<=2&&max>=2)
        out2(low,high);
    if(min<=3&&max>=3)
        out3(low,high);
    if(min<=5&&max>=5)
        out5(low,high);
    if(min<=7&&max>=7)
        out7(low,high);

    return 0;
}
