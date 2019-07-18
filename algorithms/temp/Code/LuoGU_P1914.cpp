/*
这道题目看似简单，其实有很多值得注意的点
1.首先是cin.ignore()的用法，可以忽略掉输入缓存中的那个换行符;
这里补充一下，首先是为什么要使用ignore函数，原因是getline函数会从缓冲区中读取\n之前的所有字符，
而cin读取时会读到\n之前，把\n留在缓冲区中，所以这样使用cin之后再用getline就会读到一个空行。
这也就是需要用ignore的原因，需要丢弃掉缓冲区中的\n。而ignore其实有两个参数，(int n,char c),
第一个参数是忽略的长度，第二个是读到该字符时ignore终止（c也被丢弃），不加参数默认为(1,EOF)，
即丢弃下一个字符（这里就可以用来丢弃缓冲区中剩下的那个换行符）。
2.移位的数字n有可能大于26，所以在移位之前需要先模26，这个很容易忽略 ;
3.最后就是ascii的值如果超过127就无效了(实际上是回到了-128)，
也就是说该字符已经溢出，不能再用常规方法进行判断了。
所以在对字符进行操作时，要先判断其ascii值是否在有效范围内，之后再进行操作。
这里放一个测试例程：
*/
//测试例程
//#include<iostream>
//using namespace std;
//int main()
//{
//    char c='z';
//    cout<<c<<endl;
//    c+=6;
//    cout<<c<<endl;
//    if(c>'z')//判断失效
//    c-=6;
//    cout<<c<<(int)c<<endl;
//    return 0;
//}
//程序源码
#include<iostream>
#include<string>
using namespace std;
int main()
{
    int n,ascii;
    string str;
    cin>>n;
    n%=26;
    cin.ignore();
    //getline(cin,str);
    getline(cin,str);
    for(int i=0;i<str.length();i++)
    {
        if(str[i]>='a'&&str[i]<='z')
        {
        ascii=str[i];
        ascii+=n;
        if(ascii>'z')
            ascii-=26;
        str[i]=ascii;
        }
    }
    cout<<str;
    return 0;
}

