#include<iostream>
using namespace std;
void move(int n,char source,char trans,char dest)
{
    if(n==1)
        cout<<source<<" to "<<dest<<endl;
    else
    {
        move(n-1,source,dest,trans);//将source通过dest移动到mid
        move(1,source,trans,dest);
        move(n-1,trans,source,dest);//将mid通过source移动到dest
    }
}
int main()
{
    int n;
    while(cin>>n)
    {
        move(n,'A','B','C');
        cout<<endl;
    }
    return 0;
}
/*容易犯的错误就是忽略将用于过渡的柱子放在参数中，其实当n>2时，移动柱子是需要过渡的。
A）将A上的n-1（等于2，令其为n‘）个圆盘移到B（借助于C），步骤如下：
（1）将A上的n’-1（等于1）个圆盘移到C上。//重点！
（2）将A上的一个圆盘移到B。
（3）将C上的n‘-1（等于1）个圆盘移到B。//重点！
B）将A上的一个圆盘移到C。
C）将B上的n-1（等于2，令其为n‘）个圆盘移到C（借助A），步骤如下：
（1）将B上的n‘-1（等于1）个圆盘移到A。//重点！
（2）将B上的一个盘子移到C。
（3）将A上的n‘-1（等于1）个圆盘移到C。//重点！
到此，完成了三个圆盘的移动过程。*/
