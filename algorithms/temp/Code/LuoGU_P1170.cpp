/*
这道题有四个需要注意的地方：
1.本题的算法十分巧妙，利用了数论里面的一个知识点，即坐标系中，如果一个点的横纵坐标互质，则
它跟原点的连线不经过整点（即横纵坐标均为整数的点），仔细想想就能理解这一点了。此题类似，
是求两个点之间是否存在整点；
2.在求互质时，最常用的就是求最大公约数，如果是1则二者互质，否则二者不互质，另外有一个需要
注意的地方，此题中，0和任何数的最大公约数都为任何数，这样当这个数为1时，二者互质，不为1时
二者不互质。在算法方面，有两种，一种递归（代码只有一行），一种非递归，二者在效率上应该差不多
但都应该了解；
3.本题在求绝对值时，还用到了一个点，就是数学函数库中的abs，abs根据其参与运算的值的类型有两种，
一种是int，一种是long long，另外fabs有三种，double、float、long double。此处应该注意一下，选择合适
的函数进行使用。
4.最后一个值得注意的地方就是此题一开始，我用cin读取四个数时，后两组数据总是提示超时，但是换成
scanf后就能通过。搜索之后发现，cin、cout在输入输出时用到了缓冲区，某种程度上减缓了读写速度。
但是在程序中加上这行代码：
std::ios::sync_with_stdio(false);
就会提高速度，从而AC。因此，以后在写程序时如果超时，不妨检查一下是不是读写出了问题。
*/
#include<iostream>
#include<math.h>
using namespace std;
//int gcd(int a,int b)//递归求法
//{
//  return (b>0)?gcd(b,a%b):a;
//}
int gcd(int a,int b)//非递归求法
{
   while(b!=0)
    {
        int c=b;
        b=a%b;
        a=c;
    }
    return a;
}
int main()
{
    std::ios::sync_with_stdio(false);//重点！！
    int n,a,b,c,d,x,y;
    cin>>n;
    for(int i=0;i<n;i++)
    {
        //scanf("%d%d%d%d",&a,&b,&c,&d);
        cin>>a>>b>>c>>d;
        x=abs(a-c);
        y=abs(b-d);
        if(gcd(x,y)==1)
            cout<<"no"<<endl;
            //printf("no\n");
        else
            //printf("yes\n");
            cout<<"yes"<<endl;
    }
    return 0;
}
