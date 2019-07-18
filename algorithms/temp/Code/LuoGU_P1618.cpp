//自己的代码，很朴实的算法，就是一个个遍历数字；
//但也不需要全部遍历，根据比例，遍历到某个点为止即可；
//要注意的是不要忽略987这个上限（重点1）
//另外以后不要在函数里定义数组，会报RunTime Error

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
            if(p<=987&&q<=987)//重点1
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


//利用C++的全排列计算数字的组合，可以在组合时就将比例考虑进去，减少组合的次数。
//全排列也可以用深度优先搜索来模拟。
//三个数字的比例性质需要留意一下（重点1），然后重点2就是全排列的书写方式，
//注意函数参数其实是地址，而不是数值，+1和+10（+10目前当成是结尾的位置）
//排序是按照字典序来进行的，如果开始的字典序最大，则不会继续执行

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
        if(x*b==y*a&&x*c==z*a&&y*c==b*z)//重点1
        {
            tag=true;
            cout<<x<<' '<<y<<' '<<z<<endl;
        }
    }while(next_permutation(num+1,num+10));//重点2
    if(!tag)
        cout<<"No!!!";
    return 0;
}
*/
