#include<iostream>
#include<string>
using namespace std;
/*思路整理
1.读取两个小数(正数可能没有'+'号)x和y；
2.求得第一个数的相反数；
3.将三个数规范化，变为用符号位表示的形式(0.x,1.x)；
4.求三个数的补码(构造一个求补码的函数)；
5.将x和-x变成双符号位，同时去掉小数点；
6.将y去掉小数点并和附加位构成一个新数；
7.进入Booth算法主体，定义部分积，其长度为x的长度。定义循环次数counter。
8.定义加法函数，不需要考虑对齐问题；
9.定义移位函数；
10.计算后，将结果转换成原码输出（带符号）。
*/

/*测试数据
0.1101,0.1011
output=+0.10001111

-0.1011,-0.1101
output=+0.10001111

-0.11011011,-0.100101101
output=+0.10000000101111111

+0.11011011,-0.100101101
output=-0.10000000101111111

-0.1101,+0.1001
output=-0.01110101

+0.1001,-0.1101
output=-0.01110101
*/

/*语法重点
1.代码中主要涉及的数据类型就是字符串，所以对字符串的操作是语法的重点；
2.string::find_last_of(char c)，找到c最后一次出现的位置；
3.string::insert(int n,char* s)，注意s为字符串，在原字符串[n]前插入字符串s；
4.字符串不能用switch判定；
5.除了字符串，程序中还有一处用到了引用，调用时不需要加"&"，只在函数声明时写就行了。
*/

string opposite(string str)//求相反数
{
    if(str[0]=='-')
        str[0]='+';
    else if(str[0]=='+')
        str[0]='-';
    else if(str[0]=='0')
        str='-'+str;
    return str;
}
string normalize(string str)//规范化
{
    if(str[0]=='-')
        str='1'+str.substr(2,str.length()-2);
    else if(str[0]=='+')
        str=str.substr(1,str.length()-1);
    return str;
}
string getComplement(string str)//求补码
{
    if(str[0]=='0')
        return str;
    int pos=0;
    pos=str.find_last_of('1');
    for(int i=2;i<pos;i++)
    {
        if(str[i]=='1')
            str[i]='0';
        else if(str[i]=='0')
            str[i]='1';
    }
    return str;
}
string extendSignBit(string x)//扩展为双符号位
{
    x[1]=x[0];
    return x;
}
string normalizeY(string y)//去掉乘数的小数点，并增添附加位
{
    y=y[0]+y.substr(2,y.length()-2)+'0';
    return y;
}
string getOriginal(string str)//求原码
{
    if(str[0]=='+')
        return str;
    int pos=0;
    pos=str.find_last_of('1');
    for(int i=3;i<pos;i++)
    {
        if(str[i]=='1')
            str[i]='0';
        else if(str[i]=='0')
            str[i]='1';
    }
    return str;
}
string add(string s1,string s2)//加函数
{
    int carry=0;
    int sum=0;
    string output(s1.length(),'0');
    for(int i=s1.length()-1;i>=0;i--)
    {
        sum=carry+s1[i]-'0'+s2[i]-'0';
        if(sum==0)
        {
            carry=0;
            output[i]='0';
        }
        else if(sum==1)
        {
            carry=0;
            output[i]='1';
        }
        else if(sum==2)
        {
            carry=1;
            output[i]='0';
        }
        else if(sum==3)
        {
            carry=1;
            output[i]='1';
        }
    }
    return output;
}
void rightShift(string &output,string &y_com)//右移函数
{
    y_com=output.substr(output.length()-1,1)+y_com.substr(0,y_com.length()-1);
    output=output.substr(0,2)+output[1]+output.substr(2,output.length()-3);
}
void Booth(string x_com,string x_oppCom,string y_com)//Booth算法
{
    string output(x_com.length(),'0');//结果初始化
    string temp;
    int counter=y_com.length()-2;//设置循环次数
    for(int i=0;i<=counter;i++)
    {
        temp=y_com.substr(y_com.length()-2,2);
        if(temp=="10")
        {
            output=add(output,x_oppCom);
            if(i!=counter)
            rightShift(output,y_com);
        }
        else if(temp=="01")
        {
            output=add(output,x_com);
            if(i!=counter)
            rightShift(output,y_com);
        }
        else
        {
            if(i!=counter)
            rightShift(output,y_com);
        }
    }
    if(output[0]=='0')
        output[0]='+';
    else if(output[0]=='1')
        output[0]='-';
    output[1]='0';
    output.insert(2,".");
    output+=y_com.substr(0,counter);
    output=getOriginal(output);
    cout<<output;
}
int main()
{
    string x,x_opposite,y;
    string x_com,x_oppCom,y_com;
    string input;
    getline(cin,input);
    //cin>>input;
    int pos=0;
    pos=input.find(',');
    x=input.substr(0,pos);
    y=input.substr(pos+1,input.length()-pos-1);
    x_opposite=opposite(x);
    //相反数
    x=normalize(x);
    y=normalize(y);
    x_opposite=normalize(x_opposite);
    //得到规范化的三个数
    x_com=getComplement(x);
    x_oppCom=getComplement(x_opposite);
    y_com=getComplement(y);
    //得到运算所需的三个补码形式的数
    x_com=extendSignBit(x_com);
    x_oppCom=extendSignBit(x_oppCom);
    y_com=normalizeY(y_com);
    //得到运算所需的所有数值
    Booth(x_com,x_oppCom,y_com);
    return 0;
}
