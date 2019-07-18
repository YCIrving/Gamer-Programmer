/*
    这道题的重点在于理解题意，就可以用递归做了。
（模拟的话涉及到字符串和数字之间的转换，比较复杂）
模拟的话，这道题开始错误的原因在于把原数错误地理解成了数字的最高位，导致答案偏小。
所以一开始要计算出数字的大小，然后除2，这是能加在左边最大的数字。
然后将加在左边的数字再除2，依次类推。
实际上，明白了模拟的过程就知道这道题无非就是在计算数字i本身，
这种将数字填加在左边的方法只是在故弄玄虚罢了。所以用数学就能解决这个问题。
*/

#include<iostream>
using namespace std;
int Recursion(int a)
{
    int ans=1;
    for(int i=a/2;i>0;i--)
    {
        ans+=Recursion(i);
    }
    return ans;
}
int main()
{
    int ans=0;
    int a=0;
    cin>>a;
    ans=Recursion(a);
    cout<<ans;
    return 0;
}
