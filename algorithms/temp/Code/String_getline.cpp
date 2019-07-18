#include<iostream>
#include<string>
/*字符串处理，读取一整行字符串，而不是到空格截止
CCF因为这个小点，第三题没有出来，特此记录*/
using namespace std;
int main()
{
    string str;
    getline(cin,str);
    cout<<str;
}
