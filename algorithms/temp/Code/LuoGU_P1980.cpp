/*感觉此题可以用数学方法，直接根据n来进行判断
目前这个程序还比较简单，用的是遍历和搜索*/

#include<iostream>
using namespace std;
int main()
{
    int n,x,ans=0;
    cin>>n>>x;
    for(int i=n;i>0;i--)
    {
        int j=i;
        while(j!=0)
        {
            if(j%10==x)
                ans++;
            j/=10;
        }
    }
    cout<<ans;
    return 0;
}
/*另外附上将数字转换为字符串的代码，超时了*/
//#include<iostream>
//#include<sstream>//需引用的头文件
//using namespace std;
//int main()
//{
//    int n=0,ans=0;
//    char x;
//    cin>>n>>x;
//    string str;
//    for(int i=1;i<=n;i++)
//    {
//        stringstream ss;//第一步
//        ss<<i;//第二步
//        str=ss.str();//第三步
//        for(int j=0;j<str.length();j++)
//        {
//            if(x==str[j])
//                ans++;
//        }
//    }
//    cout<<ans;
//    return 0;
//}
