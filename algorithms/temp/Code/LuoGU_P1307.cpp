/*本题思考的时间很短，所以程序还可以优化很多
借鉴网上的一个思路：大体思路是读取一个字符串，然后反转输出。
先判断第一个字符是不是’-‘，如果就先输出，并且不输出反转后的最后一个字符（即-）。
在循环中，遇到非0数前，遇到0直接跳到下次循环。

另外这段代码更厉害：
不需要使用字符串，每次取余存到另一个数中即可
int main()
{
  int n,m=0;
  scanf("%d",&n);
  while(n!=0)
  {
    m=m*10+n%10;
    n/=10;
  }
  printf("%d\n",m);
  return 0;
}
*/

#include<iostream>
#include<string>
using namespace std;
int main()
{
    string str;
    cin>>str;
    string ans=str+' ';
    int i=0;
    int flag=0,length=str.length();
    if(str[0]=='-')
        flag=1;
    for( i=0;i<length;i++)
    {
        ans[length-i]=str[i];
    }
    for(i=1;i<length;i++)
    {
        if(ans[i]!='0')
            break;
    }
    if(str[0]=='0')
        cout<<'0'<<endl;
    else
    {
        if(flag==1)
        {
            ans[i-1]='-';
            cout<<ans.substr(i-1,length-i+1);
        }
        else
        {
            cout<<ans.substr(i,length-i+1);

        }
    }
    return 0;
}

