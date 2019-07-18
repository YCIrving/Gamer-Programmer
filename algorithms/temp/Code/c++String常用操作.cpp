//字符串常用操作，增删改查+子串，应该够用了
//详见http://www.cppblog.com/lmlf001/archive/2006/04/19/5883.html

#include<iostream>
#include<string>
using namespace std;
int main()
{
    string s1="abcdefg",s2;
    s1.insert(3,1,'c');
    cout<<s1<<endl;
    cin>>s2;
    s1+=s2;
    cout<<s1<<endl;
    s1.erase(3,1);
    cout<<s1<<endl;
    s1.replace(7,3,"abc");//删除第7个字符开始的3个字符，用abc代替
    cout<<s1<<endl;
    s1.replace(7,3,3,'z');//删除第7个字符开始的3个字符，用3个z代替
    cout<<s1<<endl;
    cout<<s1.substr(2,5)<<endl;
    cout<<s1.find('z',8)<<endl;
    cout<<s1.find("zzz",5)<<endl;
    cout<<s1.find_first_of("bzzz",0)<<endl;
    /*注意，与find的区别是，find_first_of不是完全匹配，
    只需部分匹配即可，如这个例子，在abcdefgzzz中找bzzz，会先匹配b在1的位置，
    zzz就不匹配了*/
    cout<<s1.find_last_of('z')<<endl;
    cout<<s1.rfind("zzz")<<endl;

    return 0;
}
